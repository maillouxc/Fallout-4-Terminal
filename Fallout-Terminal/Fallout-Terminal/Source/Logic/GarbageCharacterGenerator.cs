using System;

namespace Fallout_Terminal.Source.Logic
{
    /// <summary>
    /// This static class can be used to return one of the garbage characters from the Fallout hacking mini-game.
    /// </summary>
    class GarbageCharacterGenerator
    {
        Random random = new Random();

        private static char[] garbageCharacters =
            {'!', '(', ')', '{', '}', '<', '>', '[', ']', '/', '\\', '|', '$',
            '@', ',', '\'', ';', ':', '?', '*', '^', '=', '.', '-', '+', '&', '_', '%', '#'};

        /// <summary>
        /// Returns a random garbage character to populate the spaces in the terminal.
        /// </summary>
        /// <returns>A char which is one of the allowed garbage characters.</returns>
        internal char GetGarbageCharacter()
        {
            return garbageCharacters[random.Next(0, garbageCharacters.Length)];
        }
    }
}
