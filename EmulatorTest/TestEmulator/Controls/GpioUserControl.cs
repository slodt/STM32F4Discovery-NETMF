using System;
using System.Windows.Forms;
using Microsoft.SPOT.Emulator;
using Microsoft.SPOT.Emulator.Gpio;
using Microsoft.SPOT.Hardware;

namespace TestEmulator.Controls
{
    public class GpioUserControl : UserControl, IEmulatorComponent
    {
        private GpioPort _port;
        public GpioPort Port
        {
            get
            {
                if(!DesignMode && _port == null)
                    _port = new GpioPort();
                
                return _port;
            }
        }

        private int _pin;
        public int Pin
        {
            get { return _pin; }
            set
            {
                _pin = value;
                if (!DesignMode)
                    Port.Pin = (Cpu.Pin) _pin;
            }
        }

        protected void UpdateUI(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        public EmulatorComponent GetComponent()
        {
            return Port;
        }
    }
}