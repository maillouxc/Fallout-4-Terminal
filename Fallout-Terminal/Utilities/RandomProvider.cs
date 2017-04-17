using System;
using System.Threading;

namespace Fallout_Terminal.Utilities
{
    /// <summary>
    /// This class provides a better, "more random" way of getting random numbers,
    /// since creating a new instance of random each time we need a random number is
    /// far from random, as testing has revealed.
    /// 
    /// This idea comes directly from the legendary Jon Skeet, god of C#.
    /// Thanks, Jon.
    /// </summary>
    public static class RandomProvider
    {
        private static Random RNG = new Random();

        /// <summary>
        /// Gets a random number between given lower and upper bounds.
        /// This is just a wrapper for the built in C# Random class, but using this
        /// gives more random randoms.
        /// </summary>
        public static int Next(int min, int max)
        {
            return RNG.Next(min, max);
        }
    }
}