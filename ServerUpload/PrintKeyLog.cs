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
            string path = "C:\\path\\to\\keypress\\log";
            FileStream fs;
            fs = new FileStream(path, FileMode.OpenOrCreate);
            KeyPressBuffer kb3 = KeyPressBuffer.Parser.ParseFrom(fs);
            fs.Close();
            foreach(KeyPress ks in kb3.Keys)
            {
                Console.Write(ks.Key);
                Console.WriteLine((Keys)ks.Key);
            }
            Console.ReadKey();
        }
    }
}
