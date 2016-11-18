using System;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// This static class can be used to return the random garbage characters from the Fallout hacking mini-game.
    /// </summary>
    class GarbageCharacterGenerator
    {
        private Random random = RandomProvider.GetThreadRandom();

        private static char[] garbageCharacters =
            {'!', '(', ')', '{', '}', '<', '>', '[', ']', '/', '\\', '|', '$',
            '@', ',', '\'', ';', ':', '?', '*', '^', '=', '.', '-', '+', '&', '_', '%', '#'};

        /// <summary>
        /// Returns a random garbage character.
        /// </summary>
        /// <returns>A random char which is one of the allowed garbage characters.</returns>
        public char GetGarbageCharacter()
        {
            return garbageCharacters[random.Next(0, garbageCharacters.Length)];
        }
    }
}
