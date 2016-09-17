using System;
using System.Collections.Generic;
using System.Linq;

namespace Fallout_Terminal.Source.Logic
{
    /// <summary>
    /// This class will read a list of potential passwords from a file, and 
    /// will handle supplying the game with a list of passwords to use in gameplay as choices.
    /// </summary>
    class PasswordGenerator
    {
        private List<string> wordsFromFile;
        const string DEFAULT_WORDLIST_PATH = @"..\..\Resources\Misc\words.txt";
        Random random = RandomProvider.GetThreadRandom();

        public PasswordGenerator()
        {
            this.wordsFromFile = ReadAllWordsFromFile();
        }

        /// <summary>
        /// Gets all of the words from the words.txt file by default, or from another file 
        /// if a path to the file is passed as a parameter.
        /// </summary>
        /// <returns>A list of strings representing each word in the file at the passed (or default) 
        /// path.</returns>
        private List<string> ReadAllWordsFromFile(string path = DEFAULT_WORDLIST_PATH)
        {
            List<string> allWordsFromFile;
            allWordsFromFile = System.IO.File.ReadAllLines(DEFAULT_WORDLIST_PATH).ToList();
            return allWordsFromFile;
        }

        /// <summary>
        /// Generates the list of potential passwords that will be used in the game.
        /// </summary>
        /// <param name="numberToGenerate">The number of potential passwords to generate.</param>
        /// <param name="desiredLength">The desired length of each password.</param>
        /// <returns>The list of potential passwords that will be used in the game.</returns>
        public List<string> GeneratePotentialPasswords(int numberToGenerate, int desiredLength)
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
        /// Returns a list of words of the passed length contained within the 
        /// list of words read in from the words file.
        /// </summary>
        /// <param name="length">The length of words to return.</param>
        /// <returns>A list of words of the desired length.</returns>
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
