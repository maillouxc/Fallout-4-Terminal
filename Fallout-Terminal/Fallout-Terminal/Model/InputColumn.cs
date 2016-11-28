using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class InputColumn
    {
        public string[] Contents;

        /// <summary>
        /// Creates an InputColumn object.
        /// </summary>
        public InputColumn()
        {
            Contents = new string[TerminalModel.NUMBER_OF_LINES];
        }

        /// <summary>
        /// Adds a new line of text to the bottom line of the input column, moving all other lines up 1,
        /// and erasing the topmost line, if the column is at max height.
        /// </summary>
        public void AddLine(string line)
        {
            MoveAllLinesUpByOne();
            Contents[TerminalModel.NUMBER_OF_LINES - 1] = line;
            // TODO: Notify the program with an event.
        }

        /// <summary>
        /// Moves all lines within the input column array up by one, erasing the topmost line
        /// if the input column is full.
        /// </summary>
        public void MoveAllLinesUpByOne()
        {
            string[] temp = new string[TerminalModel.NUMBER_OF_LINES];
            for(int i = 1; i < Contents.Length; i++)
            {
                temp[i - 1] = Contents[i];
                Console.WriteLine(temp[i-1]);
            }
            Contents = temp;
        }
    }
}
