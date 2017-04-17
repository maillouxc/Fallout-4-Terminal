using Fallout_Terminal.Utilities;
using System.Collections.Generic;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// The list of hexadecimal numbers to be used for the hex columns.
    /// </summary>
    public class HexList
    {
        public List<string> Values { get; private set; }

        private const int NUMBER_OF_VALUES_NEEDED = TerminalModel.NUMBER_OF_LINES * 2; // 2 columns
        private const int INCREMENT_VALUE = TerminalModel.NUMBER_OF_COLUMNS;
        private const int MIN_START_VALUE = 0;
        private const int MAX_END_VALUE = 65535 - (NUMBER_OF_VALUES_NEEDED * INCREMENT_VALUE);

        /// <summary>
        /// Initializes the HexList by choosing a start number to begin generating the list of hex values at, and then generating the list.
        /// </summary>
        internal HexList()
        {
            Values = new List<string>();
            int startNumber = GetStartNumber();
            GenerateList(startNumber, NUMBER_OF_VALUES_NEEDED);
        }

        /// <summary>
        /// Returns a randomly generated starting number, based on the amount of numbers needed, which is a constant defined 
        /// in the class. Doing this ensures that the list will never overflow the maximum value. (I think...)
        /// </summary>
        private int GetStartNumber()
        {
            int startNumber = RandomProvider.Next(MIN_START_VALUE, MAX_END_VALUE);
            return startNumber;
        }

        /// <summary>
        /// Generates the list of hex values, beginning at the provided starting number. Begins at the starting value (an int), 
        /// and populates the list of hex values by adding the INCREMENT_VALUE constant to the int (the way that Fallout actually does it),
        /// and then using ToString() to convert it back to hex form.
        /// </summary> 
        private void GenerateList(int startNumber, int numberOfValuesNeeded)
        {
            int currentValue = startNumber;
            Values.Add("0x" + currentValue.ToString("x"));
            for(int i = 1; i < numberOfValuesNeeded; i++)
            {
                currentValue += INCREMENT_VALUE;
                Values.Add("0x" + currentValue.ToString("X"));
            }
        }
    }
}
