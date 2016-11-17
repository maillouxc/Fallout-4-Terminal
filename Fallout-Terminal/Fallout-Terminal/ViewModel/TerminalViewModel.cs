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
            // TODO: Link in to an event somewhere in the model to stay updated on the number of attempts remaining.
            get;
            private set;
        }
        public String LeftHexCurrentlyDisplayed { get; private set; }
        public String RightHexCurrentlyDisplayed { get; private set; }
        public String LeftMemoryDumpCurrentlyDisplayed { get; private set; }
        public String RightMemoryDumpCurrentlyDisplayed { get; private set; }
        public String InputColumnCurrentlyDisplayed { get; private set; }

        private const String ROBCO_TEXT = "Welcome to ROBCO Industries (TM) Termlink " + "\u000D" + "\u000A" + "Password Required";
        private const String DEFAULT_ATTEMPTS_TEXT = "Attempts Remaining: \u25AE \u25AE \u25AE \u25AE";
        private const int NUMBER_OF_LINES = 16; // The number of lines of body text.
        private const int DELAY_TIME = 20; // Milliseconds.

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Call to notify the view bindings that the property provided has changed.
        /// Necessary during initialization of text on screen at beginning.
        /// </summary>
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
        }

        /// <summary>
        /// Begins populating the screen with characters, one at a time.
        /// We await the method calls within because we want them to be non-blocking, but also sequential (not simultaneous).
        /// </summary>
        async public void InitializeCharacters()
        {
            await InitializeRobcoText();
            await InitializeAttemptsText();
            await InitializeBodyText();
            await InitializeInputColumn();
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
            for (int line = 0; line < NUMBER_OF_LINES; line++)
            {
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
                if (isLeftColumn)
                {
                    LeftHexCurrentlyDisplayed += hexValue[character];
                    Notify("LeftHexCurrentlyDisplayed");
                }
                else
                {
                    RightHexCurrentlyDisplayed += hexValue[character];
                    Notify("RightHexCurrentlyDisplayed");
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
                if (isLeftColumn)
                {
                    int offset = lineNumber * MemoryDump.LINE_LENGTH;
                    LeftMemoryDumpCurrentlyDisplayed += contents[character + offset];
                    Notify("LeftMemoryDumpCurrentlyDisplayed");
                }
                else
                {
                    int offset = (lineNumber * MemoryDump.LINE_LENGTH) + (contents.Length / 2);
                    RightMemoryDumpCurrentlyDisplayed += contents[character + offset];
                    Notify("RightMemoryDumpCurrentlyDisplayed");
                }
                if (character == MemoryDump.LINE_LENGTH - 1)
                {
                    // We need to add a new line after the last character in each line.
                    if (isLeftColumn)
                    {
                        LeftMemoryDumpCurrentlyDisplayed += ("\u000D" + "\u000A");
                    }
                    else
                    {
                        RightMemoryDumpCurrentlyDisplayed += ("\u000D" + "\u000A");
                    }
                }
            }
        }

        async private Task InitializeInputColumn()
        {
            await Task.Delay(DELAY_TIME);
            InputColumnCurrentlyDisplayed += ">";
            Notify("InputColumnCurrentlyDisplayed");
        }
    }
}
