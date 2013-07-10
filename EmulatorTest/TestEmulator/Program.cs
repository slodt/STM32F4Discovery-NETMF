using System.Windows.Forms;
using System.Threading;

namespace TestEmulator
{
    class Program : Microsoft.SPOT.Emulator.Emulator
    {
        private MainForm _mainForm;

        public override void SetupComponent()
        {
            _mainForm = new MainForm(this);
            base.SetupComponent();
        }

        public override void InitializeComponent()
        {
            base.InitializeComponent();

            // Start the UI in its own thread.
            var uiThread = new Thread(StartForm);
            uiThread.SetApartmentState(ApartmentState.STA);
            uiThread.Start();
        }

        public override void UninitializeComponent()
        {
            base.UninitializeComponent();

            // The emulator is stopped. Close the WinForm UI.
            Application.Exit();
        }

        private void StartForm()
        {
            // Some initial setup for the WinForm UI
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start the WinForm UI. Run() returns when the form is closed.
            Application.Run(_mainForm);

            // When the user closes the WinForm UI, stop the emulator.
            Stop();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            (new Program()).Start();
        }
    }
}
