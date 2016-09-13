using System;

namespace Fallout_Terminal.Source.Logic
{
    /// <summary>
    /// This class is used to get random numbers. By using this single class for all random numbers needed by the program, 
    /// we are able to generate better random numbers in rapid sucession. (Remember that Random seeds with the System clock 
    /// by default.)
    /// </summary>
    public static class RNG
    {
        private static Random random;

        /// <summary>
        /// Ensures that only a single instance of Random can be generated.
        /// </summary>
        private static void Initialize()
        {
            if (random == null)
            {
                random = new Random();
            }
        }

        /// <summary>
        /// Returns a random int between min and max.
        /// This method is more random than just creating a new RNG object, as it uses only one instance of random.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomInt(int min, int max)
        {
            Initialize();
            int randomInt = random.Next(min, max);
            return randomInt;
        }
    }
}
