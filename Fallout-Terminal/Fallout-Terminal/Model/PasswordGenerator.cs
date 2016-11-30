using Fallout_Terminal.Utilities;
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
        private const string DEFAULT_WORDLIST_PATH = @"..\..\Resources\Misc\words.txt";

        /// <summary>
        /// Creates an instance of PasswordGenerator.
        /// </summary>
        public PasswordGenerator()
        {
        }

        /// <summary>
        /// Generates the list of potential passwords that will be used in the game 
        /// from the list of words read in from the words file.
        /// </summary>
        /// <param name="numberToGenerate">The number of potential passwords to generate.</param>
        /// <param name="desiredLength">The desired length of each password.</param>
        /// <returns>The list of potential passwords that will be used in the game.</returns>
        public List<string> GeneratePasswords(int numberToGenerate, int desiredLength, int lettersInCommonNeeded)
        {
            List<string> words = ReadAllWordsFromFile();
            words = GetWordsOfLength(desiredLength, words); // Filter to words of correct length.
            List<string> potentialPasswords = new List<string>();
            int lettersInCommon = 0;
            for (int i = 0; i < numberToGenerate; i++)
            {
                int rand = RandomProvider.Next(0, words.Count - 1);
                string word = words.ElementAt(rand);
                // The same word could be chosen twice, and shouldn't be a problem.
                potentialPasswords.Add(word);
            }
            while (lettersInCommon < lettersInCommonNeeded)
            {
                int rand1 = RandomProvider.Next(1, potentialPasswords.Count - 1);
                int rand2 = RandomProvider.Next(0, words.Count - 1);
                potentialPasswords.RemoveAt(rand1);
                potentialPasswords.Add(words[rand2]);
                lettersInCommon = GetLettersInCommon(potentialPasswords[0], potentialPasswords);
            }
            return potentialPasswords;
        }

        /// <summary>
        /// Gets all of the words from the words.txt file by default, or from another file 
        /// if a path to the file is passed as a parameter.
        /// </summary>
        private List<string> ReadAllWordsFromFile(string path = DEFAULT_WORDLIST_PATH)
        {
            List<string> allWordsFromFile;
            allWordsFromFile = System.IO.File.ReadAllLines(path).ToList();
            return allWordsFromFile;
        }

        /// <summary>
        /// Returns a list of all words of the provided length contained within the 
        /// list of words read in from the words file.
        /// </summary>
        /// <param name="length">The length of words to return.</param>
        private List<string> GetWordsOfLength(int length, List<string> wordsFromFile)
        {
            List<string> wordsOfCorrectLength = new List<string>();
            for(int i = 0; i < wordsFromFile.Count; i++)
            {
                if(wordsFromFile.ElementAt(i).Length == length)
                {
                    wordsOfCorrectLength.Add(wordsFromFile.ElementAt(i));
                }
            }
            return wordsOfCorrectLength;
        }

        /// <summary>
        /// Returns the number of letters in common among the list of words provided.
        /// This is needed to decrese the difficulty of the game, since if I just chose random passwords
        /// from the dictionary file, we would likely end up with words that have little to no letters exactly in common,
        /// making guessing the correct password from the number of letters in common with the player's incorrect
        /// guesses much more difficult, if not impossible.
        /// </summary>
        /// <param name="word">A word to compare against.</param>
        /// <param name="passwords">The list of passwords to check.</param>
        private int GetLettersInCommon(string word, List<string> passwords)
        {
            int result = 0;
            foreach (string password in passwords)
            {
                result += GetLettersInCommon(password, word);
            }
            return result;
        }

        /// <summary>
        /// Overload of same named method, except this version only takes two words and compares them.
        /// </summary>
        private int GetLettersInCommon(string word1, string word2)
        {
            int result = 0;
            for (int i = 0; i < word1.Length; i++)
            {
                if (word1[i] == word2[i])
                {
                    result++;
                }
            }
            return result;
        }
    }
}
