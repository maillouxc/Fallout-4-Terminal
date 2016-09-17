using System;

namespace Fallout_Terminal.Source.Logic
{
    /// <summary>
    /// This class contains the main character array that models the text on the screen at any given time.
    /// </summary>
    class MainCharArray
    {
        // TODO: Consider extracting some functionality to superclass.

        private const int MAX_HORIZONTAL_CHARACTERS = 50;
        private const int MAX_VERTICAL_CHARACTERS = 28;
        
        // The array of all characters positions on the screen.
        // [# right from start at top left, # down from start at top left]
        private char[,] charArray = new char[MAX_HORIZONTAL_CHARACTERS, MAX_VERTICAL_CHARACTERS];

        public MainCharArray()
        {
            PopulateWithSpaces();
        }

        /// <summary>
        /// Fills every spot in the array with spaces.
        /// </summary>
        private void PopulateWithSpaces()
        {
            for (int i = 0; i < MAX_HORIZONTAL_CHARACTERS; i++)
            {
                for (int j = 0; j < MAX_VERTICAL_CHARACTERS; j++)
                {
                    charArray[i, j] = 'X';
                }
            }
        }

        /// <summary>
        /// FOR TESTING PURPOSES ONLY
        /// DO NOT USE IN PRODUCTION CODE.
        /// Prints charArray to console.
        /// </summary>
        internal void PrintCharArray()
        {
            for (int j = 0; j < MAX_VERTICAL_CHARACTERS; j++)
            {
                for (int i = 0; i < MAX_HORIZONTAL_CHARACTERS; i++)
                {
                    Console.Write(charArray[i,j]);
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Sets a character within the main char array.
        /// </summary>
        /// <param name="i">The x position of the character to set, from left to right.</param>
        /// <param name="j">The y position of the chraccter to set, from the top down.</param>
        /// <param name="character">The character to set at the position.</param>
        internal void SetCharAt(int i, int j, char character)
        {
            charArray[i, j] = character;
        }

        /// <summary>
        /// This method gets a character at the desired position from the mainCharArray.
        /// </summary>
        /// <param name="i">The x position from the left.</param>
        /// <param name="j">The y position from the top.</param>
        /// <returns>The charcacter at the given position in the array.</returns>
        internal char GetCharAt(int i, int j)
        {
            return charArray[i, j];
        }

        /// <summary>
        /// This method inserts a string into the charArray starting at the given start position,
        /// from left to right, top to bottom.
        /// </summary>
        /// <param name="stringToInsert"></param>
        /// <param name="i">The x position to start insertion at.</param>
        /// <param name="j">The y position to start at, down from the top.</param>
        /// 
        internal void InsertString(string stringToInsert, int xStart, int yStart)
        {
            char[] charsToInsert = stringToInsert.ToCharArray();

            int currentXPosition;
            int currentYPosition;

            currentXPosition = xStart;
            currentYPosition = yStart;

            for (int index=0; index < charsToInsert.Length; index++)
            {
                // For each character in the array of charsToInsert...

                SetCharAt(currentXPosition, currentYPosition, charsToInsert[index]);

                if (currentXPosition < MAX_HORIZONTAL_CHARACTERS - 1) 
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
    }
}
