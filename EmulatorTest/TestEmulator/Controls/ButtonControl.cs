using Microsoft.SPOT.Emulator.Gpio;

namespace TestEmulator.Controls
{
    public partial class ButtonControl : GpioUserControl
    {
        public ButtonControl()
        {
            InitializeComponent();

            Port.ModesExpected = GpioPortMode.InputPort;
            Port.ModesAllowed = GpioPortMode.InputPort;
        }

        private void Button1Click(object sender, System.EventArgs e)
        {
            Port.Write(true);
            Port.Write(false);
        }
    }
}
