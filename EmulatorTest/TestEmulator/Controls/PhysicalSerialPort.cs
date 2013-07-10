using System;
using System.IO.Ports;
using Microsoft.SPOT.Emulator.Com;

namespace TestEmulator.Controls
{
    /// <summary>
    /// PhysicalSerialPort is an serial port emulator component that maps an emulator
    /// serial port to a physical serial port on the host PC
    /// </summary>
    public class PhysicalSerialPort : ComPortToStream, ISerialPortToStream
    {
        SerialPort _port;
        String _physicalPortName;
        int _portIndex;
        int _baudRate;
        OnEmuSerialPortEvtHandler _evtHandler;

        /// <summary>
        /// The port name of the physical serial port, such as "COM1" or "COM2"
        /// </summary>
        public String PhysicalPortName
        {
            get
            {
                return _physicalPortName;
            }
            set
            {
                ThrowIfNotConfigurable();

                _physicalPortName = value;
            }
        }

        /// <summary>
        /// The baud rate for the physical serial port, defaults to 38400.
        /// </summary>
        public int BaudRate
        {
            get
            {
                return _baudRate;
            }
            set
            {
                ThrowIfNotConfigurable();
                _baudRate = value;
            }
        }

        /// <summary>
        /// Default constructor for PhysicalSerialPort.
        /// </summary>
        public PhysicalSerialPort()
        {
            _physicalPortName = null;
            _baudRate = 38400;
        }

        bool ISerialPortToStream.Initialize(int BaudRate, int Parity, int DataBits, int StopBits, int FlowValue)
        {
            // From MSDN:  The best practice for any application is to wait for some amount of time after calling the Close 
            // method  before attempting to call the Open method, as the port may not be closed instantly.

            for (int retries = 5; retries != 0; retries--)
            {
                try
                {
                    if (_port == null)
                    {
                        _port = new SerialPort(_physicalPortName, BaudRate, (Parity)Parity, DataBits);
                        switch (FlowValue)
                        {
                            case 0x18:
                                _port.Handshake = Handshake.XOnXOff;
                                break;

                            case 0x06:
                                _port.Handshake = Handshake.RequestToSend;
                                break;

                            default:
                                _port.Handshake = Handshake.None;
                                break;
                        }
                    }

                    _portIndex = int.Parse(_physicalPortName.Substring(3)) - 1;
                    _port.DataReceived += PortDataReceived;
                    _port.ErrorReceived += PortErrorReceived;
                    _port.Open();
                    Stream = new PhysicalSerialPortStream(_port);

                    return true;
                }
                catch (Exception)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }

            return false;
        }

        bool ISerialPortToStream.SetDataEventHandler(OnEmuSerialPortEvtHandler handler)
        {
            _evtHandler = handler;
            return true;
        }

        enum SerialPortEventErrors
        {
            USART_EVENT_TYPE_ERROR = 1,
            USART_EVENT_DATA_CHARS = 5,
        }

        void PortErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            if (_evtHandler != null)
            {
                _evtHandler(_portIndex, (uint)SerialPortEventErrors.USART_EVENT_TYPE_ERROR);
            }
        }

        void PortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (_evtHandler != null)
            {
                _evtHandler(_portIndex, (uint)SerialPortEventErrors.USART_EVENT_DATA_CHARS);
            }
        }


        bool ISerialPortToStream.Uninitialize()
        {
            if (_port != null)
            {
                Stream = null;
                _port.Close();
                _port.Dispose();
                _port = null;
            }

            return true;
        }

    }
}