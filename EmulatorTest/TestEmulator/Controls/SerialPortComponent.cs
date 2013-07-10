using System;
using System.ComponentModel;
using System.IO;
using Microsoft.SPOT.Emulator;
using Microsoft.SPOT.Emulator.Com;

namespace TestEmulator.Controls
{
    public partial class SerialPortComponent : Component, IEmulatorComponent
    {
        public event EventHandler<SerialDataEventArgs> OnWrite;

        public SerialPortComponent()
        {
            InitializeComponent();
        }

        private EmulatorSerialPort _stream;
        protected EmulatorSerialPort Stream
        {
            get
            {
                if (!DesignMode && _stream == null)
                {
                    _stream = new EmulatorSerialPort();
                    _stream.ComPortHandle = Microsoft.SPOT.Emulator.Com.ComPortHandle.Parse("Usart1");
                    _stream.OnWrite += (s, e) => OnWrite(s, e);
                }

                return _stream;
            }
        }

        public SerialPortComponent(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        private string _comPortHandle;
        public string ComPortHandle
        {
            get { return _comPortHandle; }
            set
            {
                _comPortHandle = value;
                if (!DesignMode)
                    Stream.ComPortHandle = Microsoft.SPOT.Emulator.Com.ComPortHandle.Parse(value);
            }
        }

        protected sealed class EmulatorSerialPort : ComPortToStream, ISerialPortToStream
        {
            public event EventHandler<SerialDataEventArgs> OnWrite;

            bool ISerialPortToStream.Initialize(int baudRate, int parity, int dataBits, int stopBits, int flowValue)
            {
                var spy = new SpyStream();
                spy.OnWrite += (o, args) => OnWrite(o, args);
                Stream = spy;
                return true;
            }

            bool ISerialPortToStream.Uninitialize()
            {
                return true;
            }

            bool ISerialPortToStream.SetDataEventHandler(OnEmuSerialPortEvtHandler handler)
            {
                return true;
            }
        }

        private sealed class SpyStream : Stream
        {
            public event EventHandler<SerialDataEventArgs> OnWrite;

            public override void Flush()
            {
                throw new NotImplementedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotImplementedException();
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                OnWrite(this, new SerialDataEventArgs(buffer));
            }

            public override bool CanRead
            {
                get { return true; }
            }

            public override bool CanSeek
            {
                get { throw new NotImplementedException(); }
            }

            public override bool CanWrite
            {
                get { return true; }
            }

            public override long Length
            {
                get { throw new NotImplementedException(); }
            }

            public override long Position
            {
                get { throw new NotImplementedException(); }
                set { throw new NotImplementedException(); }
            }
        }

        public class SerialDataEventArgs : EventArgs
        {
            public byte[] Buffer { get; set; }

            public SerialDataEventArgs(byte[] buffer)
            {
                Buffer = buffer;
            }
        }

        public EmulatorComponent GetComponent()
        {
            return Stream;
        }
    }
}
