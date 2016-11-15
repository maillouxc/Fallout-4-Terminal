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
        public String RobcoTextCurrentlyDisplayed
        {
            get;
            private set;
        }

        public String AttemptsTextCurrentlyDisplayed
        {
            get;
            private set;
        }

        private const String ROBCO_TEXT = "Welcome to ROBCO Industries (TM) Termlink " + "\u000D" + "\u000A" + "Password Required";
        private const String DEFAULT_ATTEMPTS_TEXT = "Attempts Remaining: \u25AE \u25AE \u25AE \u25AE";
        private const int DELAY_TIME = 20;

        // TEST
        public event PropertyChangedEventHandler PropertyChanged;

        protected void Notify(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        // END TEST

        private TerminalModel TerminalModel;

        public TerminalViewModel()
        {
            TerminalModel = new TerminalModel();
            InitializeCharacters();
        }

        /// <summary>
        /// Begins populating the screen with characters, one at a time.
        /// </summary>
        async public void InitializeCharacters()
        {
            await InitializeRobcoText();
            await InitializeAttemptsText();
        }

        async public Task InitializeRobcoText()
        {
            foreach (char character in ROBCO_TEXT)
            {
                await Task.Delay(DELAY_TIME);
                RobcoTextCurrentlyDisplayed += character;
                Notify("RobcoTextCurrentlyDisplayed");
            }
        }

        async public Task InitializeAttemptsText()
        {
            foreach (char character in DEFAULT_ATTEMPTS_TEXT)
            {
                await Task.Delay(DELAY_TIME);
                AttemptsTextCurrentlyDisplayed += character;
                Notify("AttemptsTextCurrentlyDisplayed");
            }
        }
        
    }
}
