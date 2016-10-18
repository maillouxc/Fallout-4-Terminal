using System;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// This static class can be used to return the random garbage characters from the Fallout hacking mini-game.
    /// </summary>
    class GarbageCharacterGenerator
    {
        // We need to check later whether this is sufficiently random, or whether we need to use
        // the wrapper class that Jon Skeet suggested.
        Random random = new Random();

        private static char[] garbageCharacters =
            {'!', '(', ')', '{', '}', '<', '>', '[', ']', '/', '\\', '|', '$',
            '@', ',', '\'', ';', ':', '?', '*', '^', '=', '.', '-', '+', '&', '_', '%', '#'};

        /// <summary>
        /// Returns a random garbage character.
        /// </summary>
        /// <returns>A random char which is one of the allowed garbage characters.</returns>
        internal char GetGarbageCharacter()
        {
            return garbageCharacters[random.Next(0, garbageCharacters.Length)];
        }
    }
}
