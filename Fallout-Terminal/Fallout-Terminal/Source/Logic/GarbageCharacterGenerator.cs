using System;

namespace Fallout_Terminal.Source.Logic
{
    /// <summary>
    /// This static class can be used to return one of the garbage characters from the Fallout hacking mini-game.
    /// </summary>
    class GarbageCharacterGenerator
    {
        Random random = new Random();

        private char GetGarbageCharacter()
        {
            // TEMPORARY
            return 'x';
        }
    }
}
