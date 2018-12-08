using System;
using System.Timers;
using System.Windows.Forms;

// This form provides a simple interface for testing the logger
// Every second the input from the keyboard is dumped to a textbox
// It is my hope that nobody needs to spend any significant time here

namespace MacroCoach
{
    public partial class KeyDebugger : Form
    {
        private System.Timers.Timer debugTimer;     // Timer used to wake up the function which prints the output.
        delegate void SetTextCallback(string text); // Delegate used to notify the correct thread to update the textbox
        private static string debugInputBuffer;     // The characters that have been captured
        private bool isRunning = false;             // Is the key recorder running?

        public KeyDebugger()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closing the window should have same behavior as clicking Stop
        /// </summary>
        private void KeyDebugger_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopAndCleanup();
        }

        /// <summary>
        /// Stops the logger and timers if they are running
        /// </summary>
        private void StopAndCleanup()
        {
            DebugOutput.WriteLine("Stopping key debugger and cleaning up.");

            // Stop the timer if it's running
            if(debugTimer != null)
            {
                debugTimer.Stop();
                debugTimer.Dispose();
                debugTimer = null;
            }

            // Stop the actual logger
            if(isRunning)
            {
                KeyListener.stopKeystrokeLogging();
            }

            isRunning = false;

            // Reset the buttons
            this.btnDebugStop.Enabled = false;
            this.btnDebugStart.Enabled = true;
        }

        /// <summary>
        /// Starts the timer and Keystroke recorder
        /// </summary>
        private void btnDebugStart_Click(object sender, EventArgs e)
        {
            DebugOutput.WriteLine("Starting Key debugger");
            this.btnDebugStart.Enabled = false;
            this.btnDebugStop.Enabled = true;

            debugTimer = new System.Timers.Timer(1000);
            debugTimer.Elapsed += timedDebugEvent;
            debugTimer.Start();

            KeyListener.startKeystrokeLogging(HookCallbackKeystrokeDebugger);
            isRunning = true;
        }

        /// <summary>
        /// Stops the Debugger
        /// </summary>
        private void btnDebugStop_Click(object sender, EventArgs e)
        {
            StopAndCleanup();
        }

        /// <summary>
        /// Callback from the logger. Grabs the keystroke and stores it in a buffer that eventually gets printed on the screen
        /// </summary>
        private static IntPtr HookCallbackKeystrokeDebugger(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = KeyListener.GetKeyCode(nCode, wParam, lParam);
            DebugOutput.WriteLine(string.Format("Received Code was: {0} Keys Cast: {1}", vkCode, (Keys)vkCode));
            if (vkCode != 0)
            {
                debugInputBuffer = String.Concat(debugInputBuffer, (Keys)vkCode);
            }
            return KeyListener.CallNextHookEx(KeyListener._hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// Timer fires this event every second to print the keys the callback stored in the buffer
        /// </summary>
        private void timedDebugEvent(Object source, ElapsedEventArgs e)
        {
            AppendToDebugOutput(debugInputBuffer);
            debugInputBuffer = "";
        }

        /// <summary>
        /// Uses the UI thread to draw the buffer key on the screen
        /// </summary>
        /// <param name="output">String to print</param>
        private void AppendToDebugOutput(string output)
        {
            if (output == null)
            {
                return;
            }

            if (this.txtKeystrokeDebugger.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AppendToDebugOutput);
                this.Invoke(d, new object[] { output });
            }
            else
            {
                this.txtKeystrokeDebugger.AppendText(debugInputBuffer);
            }
        }
    }
}
