using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyExe
{
    class Program
    {
        static void Main(string[] args)
        {

            string path = "C: \\Users\\kat_r_000\\Documents\\RIT\\CSCI642\\project\\Running\\";
            string visualDir = "C:\\Users\\kat_r_000\\Documents\\GitHub\\Keylogger\\";
            string name = "KeypressAggregator";
            System.IO.File.Copy(visualDir + name+"\\bin\\Debug\\"+name+".exe", path+name+".exe", true);
            name = "HookKeylogger";
            System.IO.File.Copy(visualDir + name + "\\bin\\Debug\\" + name + ".exe", path + name + ".exe", true);
            name = "UploadProxy";
            System.IO.File.Copy(visualDir + "ServerProxy\\bin\\Debug\\" + name + ".exe", path + name + ".exe", true);
            name = "Server";
            System.IO.File.Copy(visualDir + name + "\\bin\\Debug\\" + name + ".exe", path + name + ".exe", true);
            name = "ApplicationWatcher";
            System.IO.File.Copy(visualDir + name + "\\bin\\Debug\\" + name + ".exe", path + name + ".exe", true);

        }
    }
}
