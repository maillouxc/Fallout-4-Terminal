using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class TerminalModel
    {
        public const string HEADER_TEXT_ROW_1 = "Welcome to ROBCO Industries (TM) Termlink";
        public const string HEADER_TEXT_ROW_2 = "Password Required";

        public HexList Hexlist { get; private set; }
        public PasswordManager PasswordsManager { get; private set; }
        public MemoryDump MemoryDump { get; private set; }

        public TerminalModel()
        {
            Hexlist = new HexList();
            PasswordsManager = new PasswordManager();
            MemoryDump = new MemoryDump(PasswordsManager.PotentialPasswords);
        }
    }
}
