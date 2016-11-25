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

        private int AttemptsRemaining;

        public delegate void AttemptsChangedEventHandler(object sender, AttemptsChangedEventArgs args);
        public event AttemptsChangedEventHandler AttemptsChanged;

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
        public void NotifyAttemptsChanged(AttemptsChangedEventArgs args)
        {
            if (AttemptsChanged != null)
            {
                AttemptsChanged(this, args);
            }
        }

        public void ProcessInput(String input)
        {

        }
    }
}
