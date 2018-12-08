/* Keystroke Listening Bits 
 * Taken from this PasteBin found on Google:
 * https://pastebin.com/nPmn7wAW
 * Unfortunately I could not find an author to credit. */

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MacroCoach
{
    class KeyListener
    {
        // Delegate describing the function that gets called when a key is pressed
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        // Reference to the callback function
        public static LowLevelKeyboardProc _proc;

        public static IntPtr _hookID = IntPtr.Zero;
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
        IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// Enables the keystroke recorder, identifying which function to call on keystrokes
        /// </summary>
        /// <param name="proc">The method to call when a key is pressed</param>
        public static void startKeystrokeLogging(LowLevelKeyboardProc proc)
        {
            DebugOutput.WriteLine("Starting Keystroke logger");
            _proc = proc;
            _hookID = SetHook(_proc);
            DebugOutput.WriteLine(string.Format("proc: {0}, hook: {1}", _proc, _hookID));
        }

        /// <summary>
        /// Stops listening for keystrokes
        /// </summary>
        public static void stopKeystrokeLogging()
        {
            DebugOutput.WriteLine("Stopping keystroke logger");
            UnhookWindowsHookEx(_hookID);
        }

        /// <summary>
        /// Takes the callback input and returns the key that was pressed
        /// </summary>
        /// <returns>The key code, or -1 for no key (caused by a key lift)</returns>
        public static int GetKeyCode(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN) { return Marshal.ReadInt32(lParam); }
            return 0;
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }        

        //These Dll's will handle the hooks. Yaaar mateys! <-- Comment from original author. I <3 you!
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
        LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
