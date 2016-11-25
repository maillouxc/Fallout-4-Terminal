using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class InputColumn
    {
        public string[] Contents { get; set; } 

        /// <summary>
        /// Creates an InputColumn object.
        /// </summary>
        public InputColumn()
        {
            Contents = new string[TerminalModel.NUMBER_OF_LINES];
        }
    }
}
