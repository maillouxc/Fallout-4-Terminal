using Fallout_Terminal.Sound;
using Fallout_Terminal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class TerminalModel
    {
        public HexList HexList { get; private set; }
        public PasswordManager PasswordManager { get; private set; }
        public MemoryDump MemoryDump { get; private set; }
        public InputColumn InputColumn { get; private set; }

        private int _attemptsRemaining;

        /// <summary>
        /// The number of attempts the player has remaining to guess the password. 
        /// Fires an event to notify the program of the change when set is called.
        /// 
        /// <Remarks>Isn't it annoying how C# forces us to declare a seperate backing field in this case? C'mon, Microsoft, step it up.</Remarks>
        /// </summary>
        private int AttemptsRemaining
        {
            get
            {
                return _attemptsRemaining;
            }
            set
            {
                _attemptsRemaining = value;
                NotifyAttemptsChanged(new AttemptsChangedEventArgs(_attemptsRemaining));
            }
        }

        public delegate void AttemptsChangedEventHandler(object sender, AttemptsChangedEventArgs args);
        public event AttemptsChangedEventHandler AttemptsChanged;

        public delegate void IncorrectPasswordEventHandler(object sender, EventArgs args);
        public event IncorrectPasswordEventHandler IncorrectPassword;

        public const int NUMBER_OF_LINES = 16;
        public const int NUMBER_OF_COLUMNS = 12;

        private const int STARTING_ATTEMPTS = 4;

        /// <summary>
        /// Creates an instance of TerminalModel.
        /// </summary>
        public TerminalModel()
        {
            HexList = new HexList();
            PasswordManager = new PasswordManager();
            MemoryDump = new MemoryDump(PasswordManager.PotentialPasswords);
            InputColumn = new InputColumn();
            AttemptsRemaining = STARTING_ATTEMPTS;
        }

        /// <summary>
        /// Dispatches the AttemptsChangedEvent with information about how many attempts remain.
        /// </summary>
        /// <param name="args"></param>
        private void NotifyAttemptsChanged(AttemptsChangedEventArgs args)
        {
            AttemptsChanged?.Invoke(this, args);
        }

        /// <summary>
        /// Takes a string of input text from the user, and determines if it is
        /// the correct password, or an Incorrect Password.
        /// Triggers the appropriate game logic following this information.
        /// </summary>
        /// <param name="input">The input string to process.</param>
        public void ProcessInput(String input)
        {
            // We can check for bracket tricks by their length and first character.
            if(input.Length > 1 && input.Length != PasswordManager.PasswordLength && !char.IsLetter(input[0]))
            {
                OnBracketTrickEntered(input);
            }
            else if(input.Length == PasswordManager.PasswordLength)
            {
                int charsInCommon;
                charsInCommon = PasswordManager.CheckPassword(input);
                if (charsInCommon == PasswordManager.PasswordLength)
                {
                    OnCorrectPasswordEntered(input);
                }
                else
                {
                    OnIncorrectPasswordEntered(charsInCommon, input);
                }
            }
        }

        /// <summary>
        /// Called whenever a correct password is entered.
        /// </summary>
        private void OnCorrectPasswordEntered(string password)
        {
            // Come at me bro
            SoundPlayer.PlayCorrectPasswordSound();
        }


        /// <summary>
        /// Called whenever an incorrect password is entered.
        /// </summary>
        private void OnIncorrectPasswordEntered(int charsInCommon, string password)
        {
            InputColumn.OverwriteLastLine(">" + password);
            InputColumn.AddLine(">Entry denied.");
            InputColumn.AddLine(">Likeness=" + charsInCommon);
            InputColumn.AddLine(">");
            AttemptsRemaining--;
            // This violates MVVC? Fight me. I'm not spending my life writing events
            // for a toy program just to conform with design patterns.
            SoundPlayer.PlayIncorrectPasswordSound();
        }

        /// <summary>
        /// Fired to notify listeners that an incorrect password has been entered.
        /// </summary>
        private void NotifyIncorrectPasswordEntered(TerminalModel terminalModel, EventArgs args)
        {
            IncorrectPassword?.Invoke(this, args); // Okay, I gotta admit. C# syntax can be so great sometimes.
        }

        /// <summary>
        /// Called whenever a bracket trick is entered by the user.
        /// 
        /// Either removes a dud password, or replenishes the attempts remaining.
        /// Replenishing attempts is only a 1 in 3 chance, while dud removal is 2 in 3.
        /// </summary>
        private void OnBracketTrickEntered(string text)
        {
            // Replenishing attempts should be less common than removing a dud.
            int rand = RandomProvider.Next(0,2);
            InputColumn.OverwriteLastLine(text);
            if(rand == 0)
            {
                ReplenishAttempts();
            }
            else
            {
                RemoveDud();
            }
        }

        /// <summary>
        /// Removes a dud password from the memory dump. Dud passwords are replaced with dots.
        /// </summary>
        private void RemoveDud()
        {
            // TODO: Do not remove dud if no duds remaining.
            InputColumn.AddLine(">Dud Removed.");
            InputColumn.AddLine(">");
            // TODO: Implement.
        }

        /// <summary>
        /// Replenishes the attempts remaining for the player to guess the correct password
        /// back to to their starting value.
        /// </summary>
        private void ReplenishAttempts()
        {
            AttemptsRemaining = STARTING_ATTEMPTS;
            InputColumn.AddLine(">Tries Reset.");
            InputColumn.AddLine(">");
        }
    }
}