using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Utilities
{
    public class IsLetterChecker
    {
        /// <summary>
        /// Returns true if the given character is a letter a-z, false if not. Case insensitive.
        /// </summary>
        /// <param name="character">The character to test.</param>
        public static bool IsLetter(char character)
        {
            character = Char.ToLower(character);
            char[] letters = {'a','b','c','d','e','f','g','h','i','j','k','l','m'
                    ,'n','o','p','q','r','s','t','u','v','w','x','y','z'};
            return letters.Contains(character) ? true : false;
        }
    }
}
