using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace MacroCoach
{
    // listAlertItem is a Tuple containing:
    // * the char representation of the keystroke
    // * the seconds that go by before firing an alert
    // * The message that is the alert
    // * The string to show in the listbox (for characters like tab)
    using listAlertItem = Tuple<char, int, string, string>;

    public partial class MainForm : Form
    {
        private static Hashtable alerts = null;        // A hash table which stores all the rules and rule data
        private static int delayBetweenAlertMessages;  // How long to wait after a message before repeating it
        private bool isAlerting = false;               // Whether alerting is currently happenning
        private ArrayList keystrokeList = new ArrayList(); // Maps keycode names to actual Keys so the UI can have a drop down
        private delegate void ipcActionDelegate(string text);  // Delegate used to make sure IPC actions happen on UI thread

        // A speech synthesizer for notifying about alert start / stops (useful when you are in a game)
        private SpeechSynthesizer synthesizer = new SpeechSynthesizer();

        public MainForm()
        {
            InitializeComponent();
        }

        #region startup and shut down

        /// <summary>
        /// Adds the tooltips, loads saved UI settings, and creates the keystroke list
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Only one instance of the application may live at a time. Others are just used for signaling.
            SendSignalAndCloseIfNotMainInstance();

            // You get a tooltip, and YOU get a tooltip, AND YOU GET A TOOLTIP!
            toolTip1.SetToolTip(this.lblSecondsBetweenAlerts, "How long after the first alert to wait until the next one. At least 2 seconds is recommended. If the delay is less time than it takes to say the message it will get cut off.");
            toolTip1.SetToolTip(this.numSecondsBetweenAlerts, "How long after the first alert to wait until the next one. At least 2 seconds is recommended. If the delay is less time than it takes to say the message it will get cut off.");

            toolTip1.SetToolTip(this.chkNotifyOnStartStop, "If checked, will notify out loud when starting / stopping the alerter");

            toolTip1.SetToolTip(this.lblAlertCharacter, "Enter the keypress that resets the alert timer. Note that it must be selected in the list.");
            toolTip1.SetToolTip(this.comboKeystroke, "Enter the keypress that resets the alert timer. Note that it must be selected in the list.");

            toolTip1.SetToolTip(this.lblDelayBeforeAlerting, "Seconds to wait after a key has been pressed before alerting that it hasn't been pressed again.");
            toolTip1.SetToolTip(this.numDelayBeforeAlerting, "Seconds to wait after a key has been pressed before alerting that it hasn't been pressed again.");

            toolTip1.SetToolTip(this.lbltxtAlertText, "The message that can be read to you if you miss your button");
            toolTip1.SetToolTip(this.txtAlertText, "Enter a message that can be read to you if you miss your button and add it to the list. Remove items by selecting them and pressing delete. Multiple messages help avoid sensory gating.");

            toolTip1.SetToolTip(this.listAlerts, "All the alerts to watch for. Select one and press delete to kill it");


            // Create a list in memory and in the UI of all keystrokes
            foreach(Keys key in Enum.GetValues(typeof(Keys)))
            {
                keystrokeList.Add(KeyToChar(key));
                this.comboKeystroke.Items.Add(key);
            }

            LoadSavedSettings();

            this.btnDeleteAlert.Enabled = this.listAlerts.Items.Count > 0;
        }

        /// <summary>
        /// Graceful application shut down. Makes sure nothing is running and saves the UI state
        /// </summary>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Only clean up if this is the main instance.
            // This avoids the unlikely scenario of the notifier exiting after the main thread and thus erasing settings.
            if (IsApplicationFirstInstance())
            {
                StopAlerting();
                SaveSettings();

                // Close and dispose our mutex.
                if (_mutexApplication != null)
                {
                    _mutexApplication.Dispose();
                }

                // Dispose the named pipe steam
                if (_namedPipeServerStream != null)
                {
                    _namedPipeServerStream.Dispose();
                }

                synthesizer.SpeakAsyncCancelAll();
            }
        }

        /// <summary>
        /// Saves the values on the UI controls
        /// </summary>
        private void SaveSettings()
        {
            // If only everything was this easy!
            Properties.Settings.Default.SecondsBetweenAlerts = (int)this.numSecondsBetweenAlerts.Value;
            Properties.Settings.Default.doStartStopSpeak = this.chkNotifyOnStartStop.Checked;

            // Serialize the list of alert Tuples into a string
            // Using this serializing solution: https://stackoverflow.com/questions/8688724/how-to-store-a-list-of-objects-in-application-settings
            if (listAlerts.Items.Count > 0 )
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    // Lists can be serialized, so convert the Listview's items into a List.
                    bf.Serialize(ms, listAlerts.Items.Cast<listAlertItem>().ToList());

                    // read the serialized list into a buffer
                    ms.Position = 0;
                    byte[] buffer = new byte[(int)ms.Length];
                    ms.Read(buffer, 0, buffer.Length);

                    // And base64 encode that buffer so it can be saved in an XML settings file nicely.
                    Properties.Settings.Default.listAlertsItems = Convert.ToBase64String(buffer);
                }
            }
            else
            {
                Properties.Settings.Default.listAlertsItems = "";
            }

            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Loads the saved UI state
        /// </summary>
        private void LoadSavedSettings()
        {
            this.numSecondsBetweenAlerts.Value = Properties.Settings.Default.SecondsBetweenAlerts;
            this.chkNotifyOnStartStop.Checked = Properties.Settings.Default.doStartStopSpeak;

            this.listAlerts.Items.Clear(); // Should be unnecessary, but this gaurantees clean state
            if (Properties.Settings.Default.listAlertsItems.Length > 0)
            {
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(Properties.Settings.Default.listAlertsItems)))
                {
                    // Deserialize the Tuple list from Memory
                    BinaryFormatter bf = new BinaryFormatter();
                    List<listAlertItem> alerts = (List<listAlertItem>)bf.Deserialize(ms);

                    // And add those items to the ListView
                    foreach (listAlertItem alert in alerts)
                    {
                        this.listAlerts.Items.Add(alert);
                    }
                }
            }
        }
        #endregion

        #region Mutex + IPC
        // Only one instance of this application will be allowed to run.
        // Further executions will simply signal this one
        // Inspiration from: https://www.autoitconsulting.com/site/development/single-instance-winform-app-csharp-mutex-named-pipes/
        
        // Whether or not this is the first application instance
        private bool _firstApplicationInstance;

        // The mutex used to gaurantee only one instance
        private Mutex _mutexApplication;

        // The name of the mutex
        private const string MutexName = "MUTEX_SINGLEINSTANCEANDNAMEDPIPE_FOR_MACROCOACH";

        // A named pipe are used for inter process communication (so a new instance can signal the existing one)

        // Name of the pipe
        private const string PipeName = "PIPE_SINGLEINSTANCEANDNAMEDPIPE_FOR_MACROCOACH";

        // A lock used so that only one signal can be processed at a time
        private readonly object _namedPiperServerThreadLock = new object();

        // Stream containing the received pipe data
        private NamedPipeServerStream _namedPipeServerStream;
        
        /// <summary>
        /// Checks whether or not this is the first instance of our app
        /// </summary>
        /// <returns>Whether or not we were able to grab the mutex</returns>
        private bool IsApplicationFirstInstance()
        {
            // Allow for multiple runs but only try and get the mutex once
            if (_mutexApplication == null)
            {
                _mutexApplication = new Mutex(true, MutexName, out _firstApplicationInstance);
            }

            return _firstApplicationInstance;
        }

        /// <summary>
        /// Checks the mutex to see if this is the first instance.
        /// If so create a server and wait for input.
        /// If not, send a message from the commandline and close.
        /// </summary>
        private void SendSignalAndCloseIfNotMainInstance()
        {
            // First instance
            if (IsApplicationFirstInstance())
            {
                // Create a new pipe - it will return immediately and async wait for connections
                NamedPipeServerCreateServer();
            }
            else
            {
                // Pass along a message specified by the commandline
                String[] CommandLineArguments = Environment.GetCommandLineArgs();
                foreach (string s in CommandLineArguments)
                {
                    var sl = s.ToLower(); // Save some poor soul with caps lock from a miserable debugging experience

                    // Currently only the "action:" argument is used
                    // But better to future proof this then one day break everybody's shortcuts
                    if (sl.StartsWith("action:"))
                    {
                        var action = sl.Split(':')[1];
                        if (action == "start" || action == "stop" || action == "toggle")
                        {
                            using (var namedPipeClientStream = new NamedPipeClientStream(".", PipeName, PipeDirection.Out))
                            {
                                namedPipeClientStream.Connect(3000); // Maximum wait 3 seconds
                                byte[] bytes = Encoding.ASCII.GetBytes(action);
                                namedPipeClientStream.Write(bytes, 0, action.Length);
                            }
                        }
                    }
                }
                Close();
            }
        }


        /// <summary>
        /// Creates a new named pipe server if one doesn't exist already.
        /// </summary>
        private void NamedPipeServerCreateServer()
        {
            // Create a new pipe accessible by local authenticated users, disallow network
            var sidNetworkService = new SecurityIdentifier(WellKnownSidType.NetworkServiceSid, null);
            var sidWorld = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

            var pipeSecurity = new PipeSecurity();

            // By default named pipes are accessible over the network. Deny this.
            var accessRule = new PipeAccessRule(sidNetworkService, PipeAccessRights.ReadWrite, AccessControlType.Deny);
            pipeSecurity.AddAccessRule(accessRule);

            // Alow Everyone to read/write
            accessRule = new PipeAccessRule(sidWorld, PipeAccessRights.ReadWrite, AccessControlType.Allow);
            pipeSecurity.AddAccessRule(accessRule);

            // Current user is the owner
            SecurityIdentifier sidOwner = WindowsIdentity.GetCurrent().Owner;
            if (sidOwner != null)
            {
                accessRule = new PipeAccessRule(sidOwner, PipeAccessRights.FullControl, AccessControlType.Allow);
                pipeSecurity.AddAccessRule(accessRule);
            }

            // Create pipe and start the async connection wait
            _namedPipeServerStream = new NamedPipeServerStream(
                PipeName,
                PipeDirection.In,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous,
                0,
                0,
                pipeSecurity);

            // Begin async wait for connections
            _namedPipeServerStream.BeginWaitForConnection(NamedPipeServerConnectionCallback, _namedPipeServerStream);
        }

        /// <summary>
        /// The callback that gets called when there is activity on the named pipe
        /// </summary>
        private void NamedPipeServerConnectionCallback(IAsyncResult iAsyncResult)
        {
            try
            {
                // End waiting for the connection
                _namedPipeServerStream.EndWaitForConnection(iAsyncResult);

                // Read data and prevent access to the payload during threaded operations
                lock (_namedPiperServerThreadLock)
                {
                    StreamReader reader = new StreamReader(_namedPipeServerStream);
                    ActionFromIPC(reader.ReadToEnd());
                }
            }
            catch (ObjectDisposedException)
            {
                // EndWaitForConnection will exception when someone closes the pipe before connection made
                // In that case we dont create any more pipes and just return
                // This will happen when app is closing and our pipe is closed/disposed
                return;
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                // Close the original pipe (we will create a new one each time)
                _namedPipeServerStream.Dispose();
            }

            // Create a new pipe for next connection
            NamedPipeServerCreateServer();
        }

        /// <summary>
        /// Performs an action that was passed via IPC (on the UI thread)
        /// </summary>
        /// <param name="action">action to perform</param>
        private void ActionFromIPC(string action)
        {
            if(action == null || action.Length == 0) { return;  }

            // The actions toggle the button states, and may in the future perform other UI actions
            // So make sure we have the UI thread
            if (this.btnStart.InvokeRequired)
            {
                ipcActionDelegate d = new ipcActionDelegate(ActionFromIPC);
                this.Invoke(d, new object[] { action });
            }
            else
            {
                DebugOutput.WriteLine(string.Format("Received action from IPC: {0}", action));

                if(action == "start")
                {
                    StartAlerting();
                }
                else if(action == "stop")
                {
                    StopAlerting();
                }
                else if(action == "toggle")
                {
                    if(this.btnStart.Enabled)
                    {
                        StartAlerting();
                    }
                    else
                    {
                        StopAlerting();
                    }
                }
            }
        }
        #endregion

        #region Buttons and UI

        /// <summary>
        /// Reformats the items in the list view so they are slightly prettier than the default Tuple view (a,b,c)
        /// </summary>
        private void listAlerts_Format(object sender, ListControlConvertEventArgs e)
        {
            listAlertItem t = (listAlertItem)e.ListItem;
            e.Value = string.Format("Key: '{0}' | Delay: {1} | Message: '{2}'", t.Item4, t.Item2, t.Item3 );
        }

        /// <summary>
        /// Brings up the debugger Form used to capture keystrokes ids
        /// </summary>
        private void keyDebuggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KeyDebugger k = new KeyDebugger();
            k.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAbout fa = new FormAbout();
            fa.ShowDialog();
        }

        /// <summary>
        /// Gaurantees the delay before alerting is always long enough for things to work
        /// </summary>
        private void numDelayBeforeAlerting_ValueChanged(object sender, EventArgs e)
        {
            if(numDelayBeforeAlerting.Value < 1)
            {
                showInputErrorMessage("You must wait at least one second before alerting.");
                numDelayBeforeAlerting.Value = 1;
            }
        }

        /// <summary>
        /// Gaurantees there is enough time between alerts to avoid infinite alert spam
        /// </summary>
        private void numSecondsBetweenAlerts_ValueChanged(object sender, EventArgs e)
        {
            if (numDelayBeforeAlerting.Value < 1)
            {
                showInputErrorMessage("You want at least one second between alerts");
                numSecondsBetweenAlerts.Value = 1;
            }
        }

        /// <summary>
        /// Validtes an alert and adds it to the list.
        /// </summary>
        private void btnAddAlert_Click(object sender, EventArgs e)
        {
            if(this.comboKeystroke.SelectedIndex == -1)
            {
                showInputErrorMessage("No keystroke is selected. If you typed something, make sure the matching entry is selected from the list.");
                return;
            }
            if(this.txtAlertText.Text.Length < 1)
            {
                showInputErrorMessage("What good is an alert with no message?");
                return;
            }

            //listAlerts.Items.Add(new listAlertItem(this.txtAlertCharacter.Text[0], (int)this.numDelayBeforeAlerting.Value, this.txtAlertText.Text));
            listAlerts.Items.Add(
                new listAlertItem((char)keystrokeList[comboKeystroke.SelectedIndex], 
                                  (int)this.numDelayBeforeAlerting.Value, 
                                  this.txtAlertText.Text, 
                                  this.comboKeystroke.Text));
            this.comboKeystroke.SelectedIndex = -1;
            this.txtAlertText.Text = "";
        }

        /// <summary>
        /// Catches delete key presses on the list box and removes the matching item
        /// </summary>
        private void listAlerts_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                // Make sure pressing delete w/ nothing selected doesn't blow things up.
                int messageIndex = this.listAlerts.SelectedIndex;
                if (messageIndex > -1)
                {
                    this.listAlerts.Items.RemoveAt(messageIndex);
                }
            }
        }

        /// <summary>
        /// If an item from the list is selected. Enable the delete button
        /// </summary>
        private void listAlerts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.btnDeleteAlert.Enabled = this.listAlerts.SelectedIndex > -1;
        }

        /// <summary>
        /// Deletes the selected Alert in the list
        /// </summary>
        private void btnDeleteAlert_Click(object sender, EventArgs e)
        {
            int index = this.listAlerts.SelectedIndex;
            if(index > -1)
            {
                this.listAlerts.Items.RemoveAt(index);
            }
            else
            {
                showInputErrorMessage("Select an alert to delete");
            }
        }

        /// <summary>
        /// Starts alerting.
        /// </summary>
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Do all setup, including swapping buttons, in a start method
            // this ensures that if other methods to start are added (like keystrokes) everything is set in one place.
            StartAlerting();
        }

        /// <summary>
        /// Stops the alerting
        /// </summary>
        private void btnStop_Click(object sender, EventArgs e)
        {
            // Same as with start, leave everything to a function for consistency.
            StopAlerting();
        }

        /// <summary>
        /// Consistent error message for UI input errors
        /// </summary>
        /// <param name="message">Error to display</param>
        private void showInputErrorMessage(string message)
        {
            MessageBox.Show(
                message,
                "Input is not valid",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
        #endregion

        #region Actual alerting logic
        /// <summary>
        /// Saves a snapshot of the user's intention from the UI and starts all the alerting jobs (timers, keylogger)
        /// </summary>
        private void StartAlerting()
        {
            if(isAlerting)
            {
                DebugOutput.WriteLine("Already alerting. Not starting again.");
                return;
            }

            if (listAlerts.Items.Count < 1)
            {
                showInputErrorMessage("At least one alert must be defined or else this app doesn't do very much");
                return;
            }

            DebugOutput.WriteLine("Starting the alerts");
            delayBetweenAlertMessages = (int)this.numSecondsBetweenAlerts.Value;

            // Create a hash of all the rules
            alerts = new Hashtable();
            foreach (listAlertItem alert in listAlerts.Items)
            {
                int key = Char.ToUpper(alert.Item1);
                Alert a = new Alert(alert.Item2, alert.Item3);
                alerts.Add(key, a);
                DebugOutput.WriteLine(String.Format("Added item to alert hash: {0}: {1}", key, a));
            }

            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;

            KeyListener.startKeystrokeLogging(HookCallbackAlerts);

            this.isAlerting = true;

            if (this.chkNotifyOnStartStop.Checked)
            {
                synthesizer.SpeakAsync("Alerting Started");
            }
        }

        /// <summary>
        /// Stops any running processes (timers, logger) and resets some variables
        /// </summary>
        private void StopAlerting()
        {
            if(!isAlerting)
            {
                DebugOutput.WriteLine("Not alerting, therefore won't stop again");
                return;
            }

            DebugOutput.WriteLine("Stopping the alerts");

            KeyListener.stopKeystrokeLogging();

            if (alerts != null)
            {
                foreach (DictionaryEntry e in alerts)
                {
                    Alert a = (Alert)e.Value;
                    a.Cleanup();
                }
            }

            alerts = null;
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;

            this.isAlerting = false;

            if (this.chkNotifyOnStartStop.Checked)
            {
                synthesizer.SpeakAsync("Alerting Stopped");
            }
        }

        /// <summary>
        /// Callback from the key logger code. Checks to see if the pressed key is being watched and resets the appropriate timer
        /// </summary>
        private static IntPtr HookCallbackAlerts(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            int vkCode = KeyListener.GetKeyCode(nCode, wParam, lParam);
            if (vkCode != 0 && alerts.ContainsKey(vkCode))
            {
                Alert a = (Alert)alerts[vkCode];

                // If this is the first time the key has been seen create and start a timer
                if(a.timer == null)
                {
                    a.timer = new System.Timers.Timer(a.delay * 1000 + 1); // +1 in case somehow the interval is 0
                    a.timer.Elapsed += (sender, e) => FireAlert(sender, e, a); //), ref a.timer, a.message);
                    a.timer.Start();
                }
                else // Otherwise just reset the timer
                {
                    a.timer.Interval = a.delay * 1000 + 1;

                }
            }
            return KeyListener.CallNextHookEx(KeyListener._hookID, nCode, wParam, lParam);
        }

        /// <summary>
        /// When a timer expires, an alert is fired and the next one is queued up
        /// </summary>
        /// <param name="alert">The alert that was fired</param>
        static private void FireAlert(object sender, System.Timers.ElapsedEventArgs e, Alert alert)
        {
            alert.SpeakAlert();
            alert.timer.Interval = delayBetweenAlertMessages * 1000 + 1;
        }
        #endregion

        /// <summary>
        /// Maps a Virtual key to a character
        /// </summary>
        /// <param name="key">The Key to map</param>
        /// <returns>A char equivalent of that key</returns>
        // Thank you: https://social.msdn.microsoft.com/Forums/vstudio/en-US/582165f8-ff32-4cb9-907e-fe968a841b5d/converting-keys-to-characters?forum=csharpgeneral
        private static char KeyToChar(Keys key)
        {
            return unchecked((char)MapVirtualKeyW((uint)key, MAPVK_VK_TO_CHAR)); // Ignore high word.
        }

        // KeyToChar uses MapVirtualKeyW API call
        private const uint MAPVK_VK_TO_CHAR = 2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern uint MapVirtualKeyW(uint uCode, uint uMapType);
    }
}