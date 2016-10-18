using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// Abstract superclass for char grids, containing methods common to all CharGrids.
    /// A CharGrid stores a series of characters in a 2d array to be manipulated 
    /// and later accessed and then displayed on the screen.
    /// </summary>
    public abstract class CharGrid
    {
        private char[,] charGrid;

        /// <summary>
        /// The horizontal size of the CharGrid.
        /// </summary>
        internal int XSize
        {
            get;
        }

        /// <summary>
        /// The vertical size of the CharGrid.
        /// </summary>
        internal int YSize
        {
            get;
        }

        /// <summary>
        /// Creates the object and populates the CharGrid with spaces.
        /// </summary>
        internal CharGrid(int width, int height)
        {
            this.XSize = width;
            this.YSize = height;
            this.charGrid = new char[width, height];
            PopulateWithSpaces();
        }

        /// <summary>
        /// FOR TESTING PURPOSES ONLY
        /// DO NOT CALL IN PRODUCTION CODE.
        /// Prints charArray to console.
        /// </summary>
        internal void PrintCharArray()
        {
            for (int j = 0; j < this.YSize; j++)
            {
                for (int i = 0; i < this.XSize; i++)
                {
                    Console.Write(charGrid[i, j]);
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Fills every spot in the grid with spaces.
        /// Only called when first creating a CharGrid.
        /// </summary>
        private void PopulateWithSpaces()
        {
            for (int i = 0; i < XSize; i++)
            {
                for (int j = 0; j < YSize; j++)
                {
                    // TODO: Change X to space.
                    // I'm only using an X here to make the ouput visible on the console while testing.
                    charGrid[i, j] = 'X';
                }
            }
        }

        /// <summary>
        /// Gets the line of text in the CharGrid at the given y position.
        /// </summary>
        /// <param name="row">The row to get text from. (0 is the first row)</param>
        /// <returns>A string with the text from the line.</returns>
        internal string GetLine(int y)
        {
            string line = "";
            for(int x = 0; x < XSize; x++)
            {
                line += charGrid[x, y];
            }
            return line;
        }
        
        /// <summary>
        /// This method gets a character at the desired position in the CharGrid.
        /// </summary>
        /// <param name="i">The x position from the left.</param>
        /// <param name="j">The y position from the top.</param>
        /// <returns>The charcacter at the given position in the CharGrid.</returns>
        internal char GetCharAt(int i, int j)
        {
            return charGrid[i, j];
        }

        /// <summary>
        /// Sets a character within the CharGrid
        /// </summary>
        /// <param name="i">The x position of the character to set, from left to right.</param>
        /// <param name="j">The y position of the chraccter to set, from the top down.</param>
        /// <param name="character">The character to set at the position.</param>
        internal void SetCharAt(int i, int j, char character)
        {
            charGrid[i, j] = character;
        }

        /// <summary>
        /// (Abstract) Inserts a string into a CharGrid subclass object at the provided start point.
        /// </summary>
        /// <param name="stringToInsert">The string to insert.</param>
        /// <param name="i">The x position to start insertion at.</param>
        /// <param name="j">The y position to start at, down from the top.</param>
        abstract internal void InsertString(string stringToInsert, int xStart, int yStart);

        /// <summary>
        /// Checks whether there is enough room to insert a given string into the CharGrid at the point given.
        /// </summary>
        /// <param name="xStart">The x position to start checking from.</param>
        /// <param name="yStart">The y position (with y=0 being at the top) to start checking from.</param>
        /// <param name="charsToInsert">The array of characters composing the string to check.</param>
        /// <returns>True if there is enough room to insert the string, false if not.</returns>
        protected bool IsEnoughRoom(int xStart, int yStart, char[] charsToInsert)
        {
            int spacesAvailable = 0;
            int currentX = xStart;
            int currentY = yStart;

            for (int i = 0; i < charsToInsert.Length; i++)
            {
                if(currentX < (XSize - 1))
                {
                    // If there is room for this char in the current row, increment the counter.
                    currentX++;
                    spacesAvailable++;
                }
                else if (currentY < (YSize - 1))
                {
                    // If no room in the current row, but room in the next row, increment the counter and return to x = 0.
                    currentX = 0;
                    currentY++;
                    spacesAvailable++;
                }
            }

            if(spacesAvailable >= charsToInsert.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
