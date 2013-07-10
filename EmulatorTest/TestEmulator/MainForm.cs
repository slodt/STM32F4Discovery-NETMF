using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TestEmulator.Controls;

namespace TestEmulator
{
    public partial class MainForm : Form
    {
        public MainForm(Microsoft.SPOT.Emulator.Emulator emulator)
        {
            InitializeComponent();

            foreach (IEmulatorComponent control in Controls.OfType<IEmulatorComponent>())
                emulator.RegisterComponent(control.GetComponent());

            foreach (IEmulatorComponent control in components.Components.OfType<IEmulatorComponent>())
                emulator.RegisterComponent(control.GetComponent());
        }

        private void serialPortComponent1_OnWrite(object sender, SerialPortComponent.SerialDataEventArgs e)
        {
            string val = Encoding.UTF8.GetString(e.Buffer);
            Action action = () => richTextBox1.Text += val;
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }
    }
}
