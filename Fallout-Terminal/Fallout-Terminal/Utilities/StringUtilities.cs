using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Utilities
{
    /// <summary>
    /// Contains several methods that will do various things to string that are not already supported
    /// by the built in C# methods.
    /// </summary>
    public static class StringUtilities
    {
        /// <summary>
        /// Removes all '\n' newline characters from a string.
        /// </summary>
        /// <param name="s">The string to strip the newline characters from.</param>
        /// <returns>The input string without the newline characters.</returns>
        public static string RemoveNewlines(string s)
        {
            string result = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != '\n')
                {
                    result += s[i];
                }
            }
            return result;
        }
    }
}
