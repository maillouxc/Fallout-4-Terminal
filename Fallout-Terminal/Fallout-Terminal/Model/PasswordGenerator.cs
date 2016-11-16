using System;
using System.Collections.Generic;
using System.Linq;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// This class reads a list of potential passwords from a file, and 
    /// supplies a list of passwords potential passwords to use for the game.
    /// </summary>
    public class PasswordGenerator
    {
        private List<string> wordsFromFile;
        const string DEFAULT_WORDLIST_PATH = @"..\..\Resources\Misc\words.txt";
        Random random = RandomProvider.GetThreadRandom();

        /// <summary>
        /// Does nothing other than instantiate the generator and read the wordlist in from a file.
        /// </summary>
        public PasswordGenerator()
        {
            this.wordsFromFile = ReadAllWordsFromFile();
        }

        /// <summary>
        /// Gets all of the words from the words.txt file by default, or from another file 
        /// if a path to the file is passed as a parameter.
        /// </summary>
        /// <returns>
        /// A list of strings representing each word in the file at the passed (or default) 
        /// path.
        /// </returns>
        private List<string> ReadAllWordsFromFile(string path = DEFAULT_WORDLIST_PATH)
        {
            List<string> allWordsFromFile;
            allWordsFromFile = System.IO.File.ReadAllLines(path).ToList();
            return allWordsFromFile;
        }

        /// <summary>
        /// Generates the list of potential passwords that will be used in the game 
        /// from the list of words read in from the words file.
        /// </summary>
        /// <param name="numberToGenerate">The number of potential passwords to generate.</param>
        /// <param name="desiredLength">The desired length of each password.</param>
        /// <returns>The list of potential passwords that will be used in the game.</returns>
        public List<string> GeneratePasswords(int numberToGenerate, int desiredLength)
        {
            // TODO: Optimize memory usage of dictionary.
            List<string> potentialPasswords = new List<string>();
            List<string> wordsOfCorrectLength = GetWordsOfLength(desiredLength);
            for(int i = 0; i < numberToGenerate; i++)
            {
                // TODO: Modify method to generate passwords with more letters in common to make the game easier.
                potentialPasswords.Add(
                    wordsOfCorrectLength.ElementAt(
                        random.Next(0, wordsOfCorrectLength.Count)
                        )
                    );
                // TODO: Fix potential (although extremely unlikely) possiblilty of two of the same words being pulled.
            }
            return potentialPasswords;
        }

        /// <summary>
        /// Returns a list of all words of the provided length contained within the 
        /// list of words read in from the words file.
        /// </summary>
        /// <param name="length">The length of words to return.</param>
        private List<string> GetWordsOfLength(int length)
        {
            List<string> wordsOfCorrectLength = new List<string>();
            for(int i = 0; i < this.wordsFromFile.Count; i++)
            {
                if(this.wordsFromFile.ElementAt(i).Length == length)
                {
                    wordsOfCorrectLength.Add(this.wordsFromFile.ElementAt(i));
                }
            }
            return wordsOfCorrectLength;
        }
    }
}
