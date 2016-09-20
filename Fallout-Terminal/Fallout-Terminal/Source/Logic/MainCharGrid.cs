using Fallout_Terminal.Source.Logic;
using System;
using System.Windows;

namespace Fallout_Terminal.Source.Logic
{
    /// <summary>
    /// This class contains the main character grid that models the text on the screen at any given time.
    /// The MainCharGrid has a fixed size and contains all of the other sub-grids.
    /// By doing things this way, I can later iterate through each individual char in the grid and print them 
    /// to the screen, one at a time, like in the game.
    /// </summary>
    public class MainCharGrid : CharGrid
    {
        private const int WIDTH = 50;
        private const int HEIGHT = 28;

        /// <summary>
        /// Does nothing but call the base constructor to initialize class fields.
        /// </summary>
        public MainCharGrid() : base(WIDTH, HEIGHT)
        {
            // Intentionally blank.
        }

        /// <summary>
        /// Concrete implementation of the abstract method. Handles when IsEnoughRoom == false.
        /// When false, crashes the program.
        /// </summary>
        /// <param name="stringToInsert">The string to insert.</param>
        /// <param name="xStart">The x position to start at.</param>
        /// <param name="yStart">The y position to start at. (y=0 is at the top)</param>
        override internal void InsertString(string stringToInsert, int xStart, int yStart)
        {
            char[] charsToInsert = stringToInsert.ToCharArray();
            int currentXPosition = xStart;
            int currentYPosition = yStart;

            for (int i = 0; i < charsToInsert.Length; i++)
            {
                if (IsEnoughRoom(xStart, yStart, charsToInsert))
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
                else
                {
                    Console.WriteLine("Error: Attempted to add a string to MainCharGrid in a location"
                        + " where there is not enough space, which is an undefined behavior."
                        + " Crashing program...");
                    Application.Current.Shutdown(1);
                }
            }
        }

        /// <summary>
        /// Inserts a sub-grid of characters into the MainCharGrid at the given position.
        /// </summary>
        /// <param name="subGrid">The subgrid of characters to insert.</param>
        /// <param name="xPosition">The x position to insertion at.</param>
        /// <param name="yPosition">The y position to start insertion at. (y = 0 at the top)</param>
        internal void InsertSubGrid(CharGrid subGrid, int xPosition, int yPosition)
        {
            for (int row = 0; row < subGrid.YSize; row++)
            {
                // TODO: Finish method.
            }
        }
    }
}
