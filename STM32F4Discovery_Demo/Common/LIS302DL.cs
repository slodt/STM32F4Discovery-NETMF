using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Common
{
    public class LIS302DL
    {
        /// <summary>
        /// Const Names consistent with STM Example Code
        /// </summary>
        #region Registers
        // ReSharper disable InconsistentNaming
        #pragma warning disable 169
        private const byte WHO_AM_I_ADDR            = 0x0F;
        private const byte CTRL_REG1_ADDR           = 0x20;
        private const byte CTRL_REG2_ADDR           = 0x21;
        private const byte CTRL_REG3_ADDR           = 0x22;
        private const byte HP_FILTER_RESET_REG_ADDR = 0x23;
        private const byte STATUS_REG_ADDR          = 0x27;
        private const byte OUT_X_ADDR               = 0x29;
        private const byte OUT_Y_ADDR               = 0x2B;
        private const byte OUT_Z_ADDR               = 0x2D;
        private const byte FF_WU_CFG1_REG_ADDR      = 0x30;
        private const byte FF_WU_SRC1_REG_ADDR      = 0x31;
        private const byte FF_WU_THS1_REG_ADDR      = 0x32;
        private const byte FF_WU_DURATION1_REG_ADDR = 0x33;
        private const byte FF_WU_CFG2_REG_ADDR      = 0x34;
        private const byte FF_WU_SRC2_REG_ADDR      = 0x35;
        private const byte FF_WU_THS2_REG_ADDR      = 0x36;
        private const byte FF_WU_DURATION2_REG_ADDR = 0x37;
        private const byte CLICK_CFG_REG_ADDR       = 0x38;
        private const byte CLICK_SRC_REG_ADDR       = 0x39;
        private const byte CLICK_THSY_X_REG_ADDR    = 0x3B;
        private const byte CLICK_THSZ_REG_ADDR      = 0x3C;
        private const byte CLICK_TIMELIMIT_REG_ADDR = 0x3D;
        private const byte CLICK_LATENCY_REG_ADDR   = 0x3E;
        private const byte CLICK_WINDOW_REG_ADDR    = 0x3F;
        #pragma warning restore 169
        // ReSharper restore InconsistentNaming
        #endregion

        //This is optional Debug Components
        //I use a GPIO to trigger a Logic Analyzer Capture - define pin here
        public bool EnableDebug = true;

        //Declare this at the class level
        readonly SPI _spi;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="csPin">CS pin for SPI interface</param>
        public LIS302DL(Cpu.Pin csPin)
        {
            //The 302DL is a mode 3 device
            var spiConfig = new SPI.Configuration(csPin, false, 0, 0, true, true, 10000, SPI.SPI_module.SPI1);
            _spi = new SPI(spiConfig);
            Init();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="csPin">CS pin for SPI interface</param>
        /// <param name="spiModule">SPI module</param>
        public LIS302DL(Cpu.Pin csPin, SPI.SPI_module spiModule)
        {
            //The 302DL is a mode 3 device
            var spiConfig = new SPI.Configuration(csPin, false, 0, 0, true, true, 10000, spiModule);
            _spi = new SPI(spiConfig);
            Init();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="csPin">CS pin for SPI interface</param>
        /// <param name="spiModule">SPI module</param>
        /// <param name="clockRateKhz">SPI clock rate (defaults to 1MHZ in other constructors)</param>
        public LIS302DL(Cpu.Pin csPin, SPI.SPI_module spiModule, uint clockRateKhz)
        {
            var spiConfig = new SPI.Configuration(csPin, false, 0, 0, true, true, clockRateKhz, spiModule);
            _spi = new SPI(spiConfig);
            Init();
        }

        private void Init()
        {
            if (EnableDebug) { Debug.Print("Starting Init"); }

            //Check for the device - Not required but a good practice
            if (ReadRegister(WHO_AM_I_ADDR) == 0x3b) //The proper response if the device is available is 3b (It does not need to be enabled for this to work)
            {
                if (EnableDebug) { Debug.Print("Device Responded correctly"); }
            }
            else
            {
                if (EnableDebug) { Debug.Print("Device did NOT respond to WHOAMI query correctly"); }
                throw new Exception("Device did not respond correctly to WHOAMI query");
            }

            //You need to write to the 0x20 and 0x21 control registers to activate the chip and the axes
            //This code enables x,y,z - if interested in one axis you can always just check the ones you want
            WriteRegister(CTRL_REG1_ADDR, 0xC7); //0xC7 is 11000111 (400hz,powerup,0,0,0,Zenable,yenable,Xenable)
            //Upon init you need to add some time for it to wake up
            if (EnableDebug) { Debug.Print("Pausing for Device Enable"); }
            Thread.Sleep(10); //Allow time to startup
            //FeeFallFilter and other options can be set here using Reg21 see Datasheet
            //WriteRegister(CTRL_REG2_ADDR, 0xC7); 
        }

        private byte ReadRegister(byte register)
        {
            //Reads and writes take 16 clock pulses so write 2 bytes and then read
            var txData = new byte[2];
            var rxData = new byte[2];
            txData[0] = (byte)(register | 0x80); //MSB needs to be 1 for Read (so OR the register address with hex 80 which is 10000000)
            txData[1] = 0; //You have to write 2 bytes to get the device to respond - so in byte 2 just write 0
            _spi.WriteRead(txData, rxData);
            return rxData[1];
        }


        private void WriteRegister(byte register, byte data)
        {
            //Reads and writes take 16 clock pulses so write 2 bytes and then read
            var txData = new byte[2];
            txData[0] = (byte)(register | 0x00); //MSB needs to be 0 for Write (so or with 00000000) - This isn't needed but is helpful for code/learning
            txData[1] = data;
            _spi.Write(txData); //Used Write here and Not WriteRead because the device does not respond when you change a register value
        }

        /// <summary>
        /// Returns the X reading (0-255)
        /// </summary>
        public int Xvalue
        {
            get
            {
                return ReadRegister(OUT_X_ADDR);
            }

        }

        /// <summary>
        /// Returns the Y reading (0-255)
        /// </summary>
        public int Yvalue
        {
            get
            {
                return ReadRegister(OUT_Y_ADDR);
            }

        }

        /// <summary>
        /// Returns the Z reading (0-255)
        /// </summary>
        public int Zvalue
        {
            get
            {
                return ReadRegister(OUT_Z_ADDR);
            }

        }
    }
}
