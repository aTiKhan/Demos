using System;
using System.Windows.Forms;
using Coderr.Client;
using Coderr.Client.WinForms;

namespace DemoApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ConfigureCoderr();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());


        }

        private static void ConfigureCoderr()
        {
            Err.Configuration.Credentials(new Uri("http://localhost:54252/"),
                "5a617e0773b94284bef33940e4bc8384",
                "3fab63fb846c4dd289f67b0b3340fefc");

            var url = new Uri("http://localhost:50473/");
            Err.Configuration.Credentials(url,
                "12922e2757174492adc68b68b8fb0f51",
                "202d29911add4e759893aa5d4b86fba6");

            Err.Configuration.CatchWinFormsExceptions();
            Err.Configuration.TakeScreenshots();

            Err.Configuration.UserInteraction.AskForEmailAddress = true;
            Err.Configuration.SetErrorForm(x=> new SampleErrorForm(x));
        }
    }
}
