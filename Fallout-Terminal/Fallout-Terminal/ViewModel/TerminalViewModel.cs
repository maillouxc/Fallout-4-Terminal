using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fallout_Terminal.Model;
using System.ComponentModel;

namespace Fallout_Terminal.ViewModel
{
    public class TerminalViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The ROBCO header line text that is CURRENTLY ON THE SCREEN.
        /// (The distinction is needed for the character by character animation of the terminal initialization.)
        /// </summary>
        public String RobcoTextCurrentlyDisplayed
        {
            get;
            private set;
        }

        /// <summary>
        /// The attempts remaining line of text that is currently displayed on the screen.
        /// Initially needed to do the character by character animation at the start of the game.
        /// </summary>
        public String AttemptsTextCurrentlyDisplayed
        {
            // TODO: Link in to an event somewhere in the model to stay updated on the number of attempts remaining.
            get;
            private set;
        }

        //public String LeftHexCurrentlyDisplayed { get; private set; }
        //public String RightHexCurrentlyDisplayed { get; private set; }

        private const String ROBCO_TEXT = "Welcome to ROBCO Industries (TM) Termlink " + "\u000D" + "\u000A" + "Password Required";
        private const String DEFAULT_ATTEMPTS_TEXT = "Attempts Remaining: \u25AE \u25AE \u25AE \u25AE";
        private const int DELAY_TIME = 20; // Milliseconds.

        // TODO: Remformat/Relocate/Refactor this event handling code.
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private TerminalModel TerminalModel;

        /// <summary>
        /// Creates an instance of the TerminalModel class, and intializes the character display on the screen for the game.
        /// </summary>
        public TerminalViewModel()
        {
            TerminalModel = new TerminalModel();
            InitializeCharacters();
        }

        /// <summary>
        /// Begins populating the screen with characters, one at a time.
        /// We await the method calls within because we want them to be non-blocking, but also sequential (not simultaneous).
        /// </summary>
        async public void InitializeCharacters()
        {
            await InitializeRobcoText();
            await InitializeAttemptsText();
        }

        /// <summary>
        /// Initializes the robco text at the top of the game screen, character by character.
        /// </summary>
        /// <returns></returns>
        async public Task InitializeRobcoText()
        {
            // TODO: Fix screen contents shifting as new lines are created.
            foreach (char character in ROBCO_TEXT)
            {
                await Task.Delay(DELAY_TIME);
                RobcoTextCurrentlyDisplayed += character;
                Notify("RobcoTextCurrentlyDisplayed");
            }
        }

        /// <summary>
        /// Initializes the attempts remaining text at the top of the screen character by character.
        /// </summary>
        /// <returns></returns>
        async public Task InitializeAttemptsText()
        {
            foreach (char character in DEFAULT_ATTEMPTS_TEXT)
            {
                await Task.Delay(DELAY_TIME);
                AttemptsTextCurrentlyDisplayed += character;
                Notify("AttemptsTextCurrentlyDisplayed");
            }
        }

        async public Task InitializeBodyText()
        {

        }
        
    }
}
