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

        public TerminalModel()
        {
            HexList = new HexList();
            PasswordManager = new PasswordManager();
            MemoryDump = new MemoryDump(PasswordManager.PotentialPasswords);
        }
    }
}
