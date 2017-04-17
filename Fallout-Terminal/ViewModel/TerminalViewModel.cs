using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fallout_Terminal.Model;
using System.ComponentModel;

namespace Fallout_Terminal.ViewModel
{
    /// <summary>
    /// This class handles everything that is related to linking the model and the view.
    /// 
    /// It stores the screen state, handles initializatoin, manages the powerstate, and fires several events
    /// related to these things.
    /// 
    /// Normally this would be too much responsibility for a single class, but in this case, the scope of the 
    /// program is known to be small and will not change, so this isn't a situation where this will eventually
    /// become some kind of god class if not refactored. I don't have the time at the moment to extract the
    /// power state management or initialiaztion handling to another class right now. Just pretend that this 
    /// class has a single responsibility, okay? Please?
    /// </summary>
    public class TerminalViewModel : INotifyPropertyChanged
    {
        public string RobcoTextCurrentlyDisplayed { get; private set; }
        public string AttemptsTextCurrentlyDisplayed{ get; private set; }
        public string LeftHexCurrentlyDisplayed { get; private set; }
        public string RightHexCurrentlyDisplayed { get; private set; }
        public string LeftMemoryDumpCurrentlyDisplayed { get; private set; }
        public string RightMemoryDumpCurrentlyDisplayed { get; private set; }
        public string InputColumnCurrentlyDisplayed { get; private set; }
        public bool PowerIsOn { get; private set; }
        public bool IsLockedOut { get; private set; } = false;

        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void MemoryDumpContentsChangedHandler(object sender, EventArgs args);
        public event MemoryDumpContentsChangedHandler MemoryDumpContentsChanged;
        public delegate void TerminalReadyHandler(object sender, EventArgs args);
        public event TerminalReadyHandler TerminalReady;
        public delegate void PowerOffEventHandler(object sender, EventArgs args);
        public event PowerOffEventHandler OnPowerOff;
        

        public const int DELAY_TIME = 20; // Milliseconds.

        private const string ROBCO_TEXT = "Welcome to ROBCO Industries (TM) Termlink " + "\u000D" + "\u000A" + "Password Required";
        private const string DEFAULT_ATTEMPTS_TEXT = "Attempts Remaining: \u25AE \u25AE \u25AE \u25AE";

        private TerminalModel TerminalModel;
        private bool InitializationComplete = false;

        /// <summary>
        /// Creates an instance of the TerminalModel class.
        /// </summary>
        public TerminalViewModel()
        {
            PowerIsOn = false;
            // The instance of terminal model is initialized in the PowerOn() method.
        }

        /// <summary>
        /// Powers on the terminal and initialzes the game and the screen.
        /// Notifies the program when ready. Can be interrupted by setting power to off.
        /// </summary>
        public async void PowerOn()
        {
            IsLockedOut = false;
            PowerIsOn = true;
            TerminalModel = new TerminalModel();
            InitializationComplete = false;
            await InitializeScreen();
            InitializationComplete = true;
            // This check is needed if the power has been turned off while booting.
            if (TerminalModel != null)
            {
                TerminalModel.AttemptsChanged += AttemptsChanged;
                TerminalModel.InputColumn.InputColumnChanged += this.InputColumnChanged;
                TerminalModel.MemoryDump.OnContentsChanged += OnMemoryDumpContentsChanged;
                TerminalModel.AccessGranted += OnAccessGranted;
                TerminalModel.Lockout += OnLockout;
                if (PowerIsOn)
                {
                    NotifyGameReady();
                }
            }
        }

        /// <summary>
        /// Turns the terminal power off. When the terminal is turned off, the game ends. 
        /// If turned on again, a new game will start.
        /// 
        /// Can now safely be called even while the terminal is still booting.
        /// </summary>
        public async Task PowerOff()
        {
            NotifyOnPowerOff();
            PowerIsOn = false;
            ClearScreen();
            // Garbage collect the game state, once intialization is done with it.
            while(!InitializationComplete)
            {
                await Task.Delay(1);
            }
            TerminalModel = null;
        }

        /// <summary>
        /// Call to notify the view bindings that the property provided has changed.
        /// Necessary to update the text on screen.
        /// </summary>
        protected void NotifyBinding(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
         
        /// <summary>
        /// Begins populating the screen with characters, one at a time.
        /// We await the method calls within because we want them to be non-blocking, but also sequential (not simultaneous).
        /// </summary>
        async private Task InitializeScreen()
        {
            await InitializeRobcoText();
            await InitializeAttemptsText();
            await InitializeBodyText();
            await InitializeInputColumn();
        }

        /// <summary>
        /// Initializes the robco text at the top of the game screen, character by character.
        /// 
        /// Can be cancelled by setting PowerIsOn to false.
        /// </summary>
        async private Task InitializeRobcoText()
        {
            foreach (char character in ROBCO_TEXT)
            {
                if (!PowerIsOn)
                {
                    RobcoTextCurrentlyDisplayed = null;
                    NotifyBinding("RobcoTextCurrentlyDisplayed");
                    return;
                }
                await Task.Delay(DELAY_TIME);
                RobcoTextCurrentlyDisplayed += character;
                NotifyBinding("RobcoTextCurrentlyDisplayed");
            }
        }

        /// <summary>
        /// Initializes the attempts remaining text at the top of the screen character by character.
        /// 
        /// Can be cancelled by setting PowerIsOn to false.
        /// </summary>
        async private Task InitializeAttemptsText()
        {
            foreach (char character in DEFAULT_ATTEMPTS_TEXT)
            {
                if (!PowerIsOn)
                {
                    AttemptsTextCurrentlyDisplayed = null;
                    NotifyBinding("AttemptsTextCurrentlyDisplayed");
                    return;
                }
                await Task.Delay(DELAY_TIME);
                AttemptsTextCurrentlyDisplayed += character;
                NotifyBinding("AttemptsTextCurrentlyDisplayed");
            }
        }

        /// <summary>
        /// Triggers the initialization of all of the body text elements, one at a time.
        /// 
        /// Can be cancelled by setting PowerIsOn to false.
        /// </summary>
        async private Task InitializeBodyText()
        {
            for (int line = 0; line < TerminalModel.NUMBER_OF_LINES; line++)
            {
                if (!PowerIsOn)
                {
                    return;
                }
                bool isLeftColumn = true;
                await InitializeNextHexValue(isLeftColumn, line);
                await InitializeNextMemoryDumpLine(isLeftColumn, line);
                isLeftColumn = false;
                await InitializeNextHexValue(isLeftColumn, line);
                await InitializeNextMemoryDumpLine(isLeftColumn, line);
            }
        }

        /// <summary>
        /// Initializes the next hex value in the body, one character at a time.
        /// 
        /// Can be cancelled by setting PowerIsOn to false.
        /// </summary>
        /// <param name="isLeftColumn">When true, returns the value from the left column. False, the right.</param>
        /// <param name="line">The line number to return the value for.</param>
        /// <returns></returns>
        async private Task InitializeNextHexValue(bool isLeftColumn, int line)
        {
            List<string> values = TerminalModel.HexList.Values;
            string hexValue;
            hexValue = isLeftColumn ? values[line] : values[line + (values.Count / 2)];
            for(int character = 0; character < 6; character++)
            {
                await Task.Delay(DELAY_TIME);
                if (!PowerIsOn)
                {
                    LeftHexCurrentlyDisplayed = null;
                    RightHexCurrentlyDisplayed = null;
                    NotifyBinding("LeftHexCurrentlyDisplayed");
                    NotifyBinding("RightHexCurrentlyDisplayed");
                    return;
                }
                if (isLeftColumn)
                {
                    LeftHexCurrentlyDisplayed += hexValue[character];
                    NotifyBinding("LeftHexCurrentlyDisplayed");
                }
                else
                {
                    RightHexCurrentlyDisplayed += hexValue[character];
                    NotifyBinding("RightHexCurrentlyDisplayed");
                }
                // We need to add a newline at the end of each line here.
                if (character == 5)
                {
                    if(isLeftColumn)
                    {
                        LeftHexCurrentlyDisplayed += ("\u000D" + "\u000A");
                    }
                    else
                    {
                        RightHexCurrentlyDisplayed += ("\u000D" + "\u000A");
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the next line in the memory dump part of the body text,
        /// one character at a time.
        /// 
        /// Can be cancelled by setting PowerIsOn to false.
        /// </summary>
        /// <param name="isLeftColumn">bool representing whether or not the next value should come from the left column.</param>
        /// <param name="line">The line number to initialize.</param>
        /// <returns></returns>
        async private Task InitializeNextMemoryDumpLine(bool isLeftColumn, int lineNumber)
        {
            string contents = TerminalModel.MemoryDump.Contents;
            for (int character = 0; character < MemoryDump.LINE_LENGTH; character++)
            {
                await Task.Delay(DELAY_TIME);
                if (!PowerIsOn)
                {
                    LeftMemoryDumpCurrentlyDisplayed = null;
                    RightMemoryDumpCurrentlyDisplayed = null;
                    NotifyBinding("LeftMemoryDumpCurrentlyDisplayed");
                    NotifyBinding("RightMemoryDumpCurrentlyDisplayed");
                    return;
                }
                if (isLeftColumn)
                {
                    int offset = lineNumber * MemoryDump.LINE_LENGTH;
                    LeftMemoryDumpCurrentlyDisplayed += contents[character + offset];
                    NotifyBinding("LeftMemoryDumpCurrentlyDisplayed");
                }
                else
                {
                    int offset = (lineNumber * MemoryDump.LINE_LENGTH) + (contents.Length / 2);
                    RightMemoryDumpCurrentlyDisplayed += contents[character + offset];
                    NotifyBinding("RightMemoryDumpCurrentlyDisplayed");
                }
                if (character == MemoryDump.LINE_LENGTH - 1)
                {
                    // We need to add a new line after the last character in each line.
                    if (isLeftColumn)
                    {
                        LeftMemoryDumpCurrentlyDisplayed += ('\n');
                    }
                    else
                    {
                        RightMemoryDumpCurrentlyDisplayed += ('\n');
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the input column on the right of the screen.
        /// This column only has one character.
        /// 
        /// Can be cancelled by setting PowerIsOn to false.
        /// </summary>
        /// <returns></returns>
        async private Task InitializeInputColumn()
        {
            if (!PowerIsOn)
            {
                return;
            }
            await Task.Delay(DELAY_TIME);
            InputColumnCurrentlyDisplayed += ">";
            NotifyBinding("InputColumnCurrentlyDisplayed");
        }

        /// <summary>
        /// Clears the screen of all text.
        /// </summary>
        private void ClearScreen()
        {
            RobcoTextCurrentlyDisplayed = null;
            AttemptsTextCurrentlyDisplayed = null;
            LeftHexCurrentlyDisplayed = null;
            RightHexCurrentlyDisplayed = null;
            LeftMemoryDumpCurrentlyDisplayed = null;
            RightMemoryDumpCurrentlyDisplayed = null;
            InputColumnCurrentlyDisplayed = null;
            NotifyBinding("RobcoTextCurrentlyDisplayed");
            NotifyBinding("AttemptsTextCurrentlyDisplayed");
            NotifyBinding("LeftHexCurrentlyDisplayed");
            NotifyBinding("RightHexCurrentlyDisplayed");
            NotifyBinding("LeftMemoryDumpCurrentlyDisplayed");
            NotifyBinding("RightMemoryDumpCurrentlyDisplayed");
            NotifyBinding("InputColumnCurrentlyDisplayed");
        }

        /// <summary>
        /// Called whenever the number of attempts to guess the password remaining changes.
        /// </summary>
        private void AttemptsChanged(object sender, AttemptsChangedEventArgs args)
        {
            // We don't really need to use the StringBuilder class here, as performance is not a concern,
            // but it's good practice anyways.
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < args.AttemptsRemaining; i++)
            {
                s.Append("\u25AE ");
            }
            AttemptsTextCurrentlyDisplayed = "Attempts Remaining: " + s.ToString();
            NotifyBinding("AttemptsTextCurrentlyDisplayed");
        }

        /// <summary>
        /// Submits the entry passed to it for checking, whether that be a password or something else.
        /// </summary>
        public void Submit(string input)
        {
            TerminalModel.InputColumn.OverwriteLastLine("");
            TerminalModel.ProcessInput(input);
        }

        /// <summary>
        /// Should be called when the current user selection has changed.
        /// Notifies the model and updates the text in the input column to
        /// reflect the current selection.
        /// </summary>
        public async void SelectionChanged(string newSelection)
        {
            InputColumnCurrentlyDisplayed = TerminalModel.InputColumn.ToString();
            TerminalModel.InputColumn.OverwriteLastLine(">");
            foreach (char character in newSelection)
            {
                await Task.Delay(DELAY_TIME);
                TerminalModel.InputColumn.AppendToLastLine(character.ToString());
                NotifyBinding("InputColumnCurrentlyDisplayed");
            }
        }

        /// <summary>
        /// Called whenever the input column is changed, via events.
        /// Updates the input column displayed with the new contents.
        /// </summary>
        public void InputColumnChanged(object sender, EventArgs args)
        {
            StringBuilder b = new StringBuilder();
            foreach (string s in TerminalModel.InputColumn.Contents)
            {
                b.AppendLine(s);
            }
            InputColumnCurrentlyDisplayed = b.ToString();
            NotifyBinding("InputColumnCurrentlyDisplayed");
        }

        /// <summary>
        /// Refreshes the current memory dump contents displayed on the screen when they change.
        /// </summary>
        private void OnMemoryDumpContentsChanged(object sender, EventArgs args)
        {
            bool isLeftColumn = true;
            bool finished = false;
            int lineNumber = 0;
            LeftMemoryDumpCurrentlyDisplayed = "";
            RightMemoryDumpCurrentlyDisplayed = "";
            while (!finished)
            {
                string contents = TerminalModel.MemoryDump.Contents;
                for (int character = 0; character < MemoryDump.LINE_LENGTH; character++)
                {
                    if (isLeftColumn)
                    {
                        int offset = lineNumber * MemoryDump.LINE_LENGTH;
                        LeftMemoryDumpCurrentlyDisplayed += contents[character + offset];
                    }
                    else
                    {
                        int offset = (lineNumber * MemoryDump.LINE_LENGTH) + (contents.Length / 2);
                        RightMemoryDumpCurrentlyDisplayed += contents[character + offset];
                    }
                    if (character == MemoryDump.LINE_LENGTH - 1)
                    {
                        if (isLeftColumn)
                        {
                            LeftMemoryDumpCurrentlyDisplayed += ('\n');
                        }
                        else
                        {
                            RightMemoryDumpCurrentlyDisplayed += ('\n');
                        }
                    }
                }
                if (!isLeftColumn)
                {
                    lineNumber++;
                }
                if (lineNumber == TerminalModel.NUMBER_OF_LINES)
                {
                    finished = true;
                }
                isLeftColumn = !isLeftColumn;
            }
            NotifyBinding("LeftMemoryDumpCurrentlyDisplayed");
            NotifyBinding("RightMemoryDumpCurrentlyDisplayed");
            NotifyMemoryContentsChanged();
        }

        /// <summary>
        /// Called when the contents of the memory dump change. There is a nearly identical event handler in the model, 
        /// but I need this event to speak to the view, so that the selectionManager can see it and fix that pesky 
        /// selection bug that occurs when we replace the duds with dots.
        /// </summary>
        private void NotifyMemoryContentsChanged()
        {
            MemoryDumpContentsChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires the event to notify the rest of the program that the terminal initialization 
        /// is complete and everything is ready.
        /// </summary>
        private void NotifyGameReady()
        {
            TerminalReady?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Fires the event to notify the program that power off has been requested.
        /// This allows the program to take any actions necessary when this happens.
        /// </summary>
        private void NotifyOnPowerOff()
        {
            OnPowerOff?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Called when the player is locked out of the terminal. For now, we just shut off the terminal.
        /// In the game, there would be a timer before any action could be taken, but we aren't that concerned here.
        /// </summary>
        private async void OnLockout(object sender, EventArgs args)
        {
            IsLockedOut = true;
            await Task.Delay(5000);
            await PowerOff();
        }

        /// <summary>
        /// Called when the player enters the correct password.
        /// 
        /// This method just turns the power off and displays a little message to the player. It's a hack,
        /// but I have to study for finals, so meh.
        /// </summary>
        private async void OnAccessGranted(object sender, EventArgs args)
        {
            IsLockedOut = true; // This will freeze player input.
            ClearScreen();
            await Task.Delay(100);
            RobcoTextCurrentlyDisplayed = ">Access Granted. Greetings, Vault Dweller.";
            NotifyBinding("RobcoTextCurrentlyDisplayed");
            AttemptsTextCurrentlyDisplayed = "Ideas? Bugs? Please, help improve this project \n by making your own changes or filing bug reports at \n GitHub.com/maillouxc/Fallout-4-Terminal. \n Thanks for playing.";
            NotifyBinding("AttemptsTextCurrentlyDisplayed");
        }
    }
}
