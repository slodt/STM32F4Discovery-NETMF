using System;
using System.Drawing;
using Microsoft.SPOT.Emulator.Gpio;

namespace TestEmulator.Controls
{
    public partial class LedControl : GpioUserControl
    {
        public LedControl()
        {
            InitializeComponent();

            Port.ModesExpected = GpioPortMode.OutputPort;
            Port.ModesAllowed = GpioPortMode.OutputPort;
            Port.OnGpioActivity += Port_OnGpioActivity;
        }

        void Port_OnGpioActivity(GpioPort sender, bool edge)
        {
            Action action = () => BackColor = edge ? Color.Red : Color.White;
            UpdateUI(action);
        }
    }
}
