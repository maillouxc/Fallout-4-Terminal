using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// The CharGrid that holds the input column on the right side of the screen.
    /// </summary>
    internal class InputCharGrid : CharGrid
    {
        private const int WIDTH = 14;
        private const int HEIGHT = 20;

        /// <summary>
        /// Subclass constructor, which only calls the base class constructor.
        /// </summary>
        internal InputCharGrid() : base(WIDTH, HEIGHT)
        {
            // Intentionally blank.
        }

        /// <summary>
        /// Inserts a string of characters into the grid. The int parameters are IGNORED
        /// because all insertions into this grid should take place at the start of the last line.
        /// 
        /// Moves everything up a row and inserts the String on the last line.
        /// </summary>
        /// <param name="stringToInsert">The string of characters to insert.</param>
        /// <param name="xStart">Has no effect.</param>
        /// <param name="yStart">Has no effect.</param>
        override internal void InsertString(string stringToInsert, int xStart, int yStart)
        {
            // Add the '>' symbol which belongs on the start of each line in this column.
            stringToInsert = '>' + stringToInsert;
            char[] charsToInsert = stringToInsert.ToCharArray();
            int currentXPosition = 0;
            int currentYPosition = HEIGHT - 1;
            MoveEveryLineUpARow();

            for (int i = 0; i < charsToInsert.Length; i++)
            {
                SetCharAt(currentXPosition, currentYPosition, charsToInsert[i]);
                if (currentXPosition < (base.XSize - 1))
                {
                    currentXPosition++;
                }
                else
                {
                    currentXPosition = 0;
                    currentYPosition++;
                }
            }
        }

        /// <summary>
        /// Moves every line in the InputCharGrid up by 1 to prepare for the insertion of a new row at the bottom.
        /// </summary>
        private void MoveEveryLineUpARow()
        {
            // The top line will be erased, so gather every line that needs to be moved up...
            List<string> linesToKeep = new List<string>();
            for (int y = 1; y < HEIGHT; y++)
            {
                linesToKeep.Add(base.GetLine(y));
            }
            // Reinsert each line one row higher:
            // For each line in linesToKeep...
            for (int y = 0; y < linesToKeep.Count; y++)
            {
                // Fille the line with spaces to make room:
                for(int x = 0; x < WIDTH; x++)
                {
                    SetCharAt(x, (HEIGHT - 1), ' ');
                }
                // For each character in the given line...
                for (int x = 0; x < linesToKeep[y].Length; x++)
                {
                    SetCharAt(x, y, linesToKeep[y].ElementAt(x));
                }
            }
        }
    }
}
