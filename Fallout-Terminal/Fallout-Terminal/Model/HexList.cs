using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// The list of hex numbers to be used for the hex columns.
    /// </summary>
    public class HexList
    {
        private const int NUMBER_OF_VALUES_NEEDED = 44;
        private const int INCREMENT_VALUE = 12;
        private const int MIN_START_VALUE = 0;
        private const int MAX_END_VALUE = 65535 - (NUMBER_OF_VALUES_NEEDED * INCREMENT_VALUE);

        private Random rng = RandomProvider.GetThreadRandom();

        public List<string> Values { get; private set; }

        /// <summary>
        /// When this constructor is called, it initializes the class by choosing a start number
        /// to begin generating the list of hex values at, and then generates the list.
        /// The list can then be accessed via the methods in the class.
        /// </summary>
        internal HexList()
        {
            Values = new List<string>();
            int startNumber = GetStartNumber();
            GenerateList(startNumber, NUMBER_OF_VALUES_NEEDED);
        }

        /// <summary>
        /// Generates a starting number, based on the amount of numbers needed,
        /// which is a constant defined in the class. Ensures that the list 
        /// will never overflow the maximum value. Most of the behavior of this method
        /// can be configured by changing constants in the class, but that should never
        /// need to be done unless modifying how the game is played, such as adding more
        /// rows to the screen.
        /// </summary>
        /// <returns>A randomly generated int that represents what number to start generating the hex values at.</returns>
        private int GetStartNumber()
        {
            int startNumber = rng.Next(MIN_START_VALUE, MAX_END_VALUE);
            return startNumber;
        }

        /// <summary>
        /// Generates the list of hex values based off the starting number.
        /// Begins at the starting value (an int), and populates the list of hex values
        /// by adding 12 to the int (the way that Fallout actually does it), and the using
        /// an argument for ToString() to convert it back to hex form.
        /// </summary>
        /// <param name="startNumber">The random int value to begin generating the list at.
        /// Initialized in the class constructor.</param>
        /// <param name="NUMBER_OF_VALUES_NEEDED">A constant value within the class 
        /// that represents how long the list should be.</param>
        private void GenerateList(int startNumber, int NUMBER_OF_VALUES_NEEDED)
        {
            int currentValue = startNumber;
            for(int i = 0; i < NUMBER_OF_VALUES_NEEDED; i++)
            {
                if(currentValue == startNumber)
                {
                    Values.Add("0x" + currentValue.ToString("X"));
                }
                currentValue += INCREMENT_VALUE;
                Values.Add("0x" + currentValue.ToString("X"));
            }
        }
    }
}
