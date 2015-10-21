using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeypressAggregator
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;
            System.IO.StreamReader file =
            //new System.IO.StreamReader(Application.StartupPath + @"\log.txt");
            new System.IO.StreamReader("C:\\Users\\Michael Peterson\\Source\\Repos\\Keylogger\\HookKeylogger\\bin\\Debug\\log.txt");
            while ((line = file.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }

            file.Close();

            Console.ReadLine();
        }
    }
}
