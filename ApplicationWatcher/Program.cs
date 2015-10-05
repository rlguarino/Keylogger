using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;

namespace ApplicationWatcher
{
    public class ApplicationWatcherAsync
    {
        //private void WmiEventHandler(object sender, EventArrivedEventArgs e)
        //{
        //    //in this point the new events arrives
        //    //you can access to any property of the Win32_Process class
        //    Console.WriteLine("TargetInstance.Handle :    " + ((ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value)["Handle"]);
        //    Console.WriteLine("TargetInstance.Name :      " + ((ManagementBaseObject)e.NewEvent.Properties["TargetInstance"].Value)["Name"]);

        //}

        //public ApplicationWatcherAsync()
        //{
        //    try
        //    {
        //        string ComputerName = "localhost";
        //        string WmiQuery;
        //        ManagementEventWatcher Watcher;
        //        ManagementScope Scope;

        //        Scope = new ManagementScope(String.Format("\\\\{0}\\root\\CIMV2", ComputerName), null);
        //        Scope.Connect();

        //        WmiQuery = "Select * From __InstanceCreationEvent Within 1 " +
        //        "Where TargetInstance ISA 'Win32_Process' ";

        //        Watcher = new ManagementEventWatcher(Scope, new EventQuery(WmiQuery));
        //        Watcher.EventArrived += new EventArrivedEventHandler(this.WmiEventHandler);
        //        Watcher.Start();
        //        Console.Read();
        //        Watcher.Stop();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Exception {0} Trace {1}", e.Message, e.StackTrace);
        //    }

        //}

        //public static void Main(string[] args)
        //{
        //    Console.WriteLine("Listening process creation, Press Enter to exit");
        //    ApplicationWatcherAsync eventWatcher = new ApplicationWatcherAsync();
        //    Console.Read();
        //}
        public static void Main()
        {
            ManagementEventWatcher startWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
            startWatch.Start();
            ManagementEventWatcher stopWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(stopWatch_EventArrived);
            stopWatch.Start();
            Console.WriteLine("Press any key to exit");
            while (!Console.KeyAvailable) System.Threading.Thread.Sleep(50);
            startWatch.Stop();
            stopWatch.Stop();
        }

        static void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("Process stopped: {0}", e.NewEvent.Properties["ProcessName"].Value);
        }

        static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            //Console.WriteLine("Process started: {0}", e.NewEvent.Properties["ProcessName"].Value);
            string name = e.NewEvent.Properties["ProcessName"].Value.ToString();
            Console.WriteLine("Process started: {0}", name);
            if (name == "Chrome.exe")
                Console.WriteLine("Found");
        }
    }
}
