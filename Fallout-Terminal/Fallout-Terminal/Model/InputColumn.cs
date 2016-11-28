using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class InputColumn
    {
        /// <summary>
        /// The contents of the input column.
        /// </summary>
        public string[] Contents;

        public delegate void InputColumnChangedEventHandler(object sender, EventArgs args);
        public event InputColumnChangedEventHandler InputColumnChanged;

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
        public void AddLine(string text)
        {
            MoveAllLinesUpByOne();
            Contents[Contents.Length - 1] = text;
            NotifyInputColumnChanged(new EventArgs());
        }

        /// <summary>
        /// Overwrites the last line in the contents with the new string.
        /// </summary>
        public void OverwriteLastLine(string text)
        {
            Contents[Contents.Length - 1] = text;
            NotifyInputColumnChanged(new EventArgs());
        }

        /// <summary>
        /// Appends the provided text to the last line of the input column.
        /// </summary>
        /// <param name="text"></param>
        public void AppendToLastLine(string text)
        {
            Contents[Contents.Length - 1] += text;
            NotifyInputColumnChanged(new EventArgs());
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
            }
            Contents = temp;
        }

        /// <summary>
        /// Override of the default ToString() method.
        /// Returns the contents array as a string with newline characters seperating elements.
        /// </summary>
        /// <returns></returns>
        override public string ToString()
        {
            StringBuilder b = new StringBuilder();
            foreach (string s in Contents)
            {
                b.AppendLine(s);
            }
            return b.ToString();
        }

        /// <summary>
        /// Fired whenever the contents of the input column change.
        /// </summary>
        private void NotifyInputColumnChanged(EventArgs args)
        {
            InputColumnChanged?.Invoke(this, args);
        }
    }
}
