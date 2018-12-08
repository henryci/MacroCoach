using System.Speech.Synthesis;

namespace MacroCoach
{
    /// <summary>
    /// Contains all the relevant information for an alert
    /// </summary>
    public class Alert
    {
        public int delay;                      // How long after a press to fire this alert
        public string message;                 // Message to fire
        public System.Timers.Timer timer;      // The timer used to make this alert actually work
        private SpeechSynthesizer synthesizer; // Each alert gets its own synthesizer

        /// <summary>
        /// Stores the data necessary to track and fire an alert
        /// </summary>
        /// <param name="delayValue">How long between presses before this alert goes off</param>
        /// <param name="messageValue">Message to speak</param>
        public Alert(int delayValue, string messageValue)
        {
            delay = delayValue;
            message = messageValue;
            timer = null;
            synthesizer = new SpeechSynthesizer();
        }

        /// <summary>
        /// Speaks a message cancelling any queued up messages to avoid trailing alerts
        /// </summary>
        /// <param name="message">String to speak</param>
        public void SpeakAlert()
        {
            synthesizer.SpeakAsyncCancelAll();
            synthesizer.SpeakAsync(message);
        }

        /// <summary>
        /// Stops any running tasks
        /// </summary>
        public void Cleanup()
        {
            if(timer != null)
            {
                timer.Stop();
                timer = null;
            }

            synthesizer.SpeakAsyncCancelAll();
        }
    }
}
