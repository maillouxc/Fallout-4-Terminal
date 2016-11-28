using Fallout_Terminal.Utilities;
using System;
using System.Collections.Generic;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// Handles everything to do with how the rest of the game interacts with passwords.
    /// This includes things like storing the list of potential passwords, storing the correct password,
    /// checking if a given password is correct, etc.
    /// </summary>
    public class PasswordManager
    {
        // TODO: Fix literally everything in this bullshit class. I don't think I ever finished it in the first place, I just moved on...
        // TODO: Determine programmatically, removing these constants.
        private const int DEFAULT_NUMBER_OF_PASSWORDS = 8;
        private const int DEFAULT_PASSWORD_LENGTH = 11;
        private const int DEFAULT_LETTERS_IN_COMMON = 30;

        private const int MAX_PASSWORD_LENGTH = 11;
        private const int MIN_PASSWORD_LENGTH = 4;
        private const int MINIMUM_NUMBER_OF_PASSWORDS = 6;
        private const int MAXIMUM_NUMBER_OF_PASSWORDS = 12;

        public List<string> PotentialPasswords;
        public int PasswordLength;

        private string CorrectPassword;
        private int NumberOfPasswordsToGenerate;
        private PasswordGenerator PasswordGenerator;

        /// <summary>
        /// Creates a new PasswordManager object. Determines the length and number of passwords to generate, 
        /// and generates the passwords via initializing a PasswordGenerator object, 
        /// </summary>
        public PasswordManager()
        {
            NumberOfPasswordsToGenerate = DEFAULT_NUMBER_OF_PASSWORDS;
            PasswordLength = DEFAULT_PASSWORD_LENGTH;
            PasswordGenerator = new PasswordGenerator();
            PotentialPasswords = PasswordGenerator.GeneratePasswords(NumberOfPasswordsToGenerate, PasswordLength);
            ChooseACorrectPassword();
            // Convert passwords to uppercase:
            for (int password = 0; password < PotentialPasswords.Count; password++)
            {
                PotentialPasswords[password] = PotentialPasswords[password].ToUpper();
            }
            Console.WriteLine("Correct Password: " + CorrectPassword); // TESTING
        }

        /// <summary>
        /// Checks the input password to see if it is correct. If the password is correct, returns -1,
        /// else, returns the number of characters in common with the passwordToCheck argument provided.
        /// </summary>
        public int CheckPassword(string passwordToCheck)
        {
            if(passwordToCheck == CorrectPassword)
            {
                return -1;
            }
            else
            {
                int numberOfCorrectChars;
                numberOfCorrectChars = GetNumberOfCorrectChars(passwordToCheck);
                return numberOfCorrectChars;
            }
        }

        /// <summary>
        /// Returns the number of characters in common with the correct password. Should only be called by the CheckPassword method.
        /// </summary>
        private int GetNumberOfCorrectChars(string passwordToCheck)
        {
            passwordToCheck = passwordToCheck.ToLower();
            int numberCorrect = 0;
            for(int i = 0; i < CorrectPassword.Length; i++)
            {
                if(passwordToCheck[i] == CorrectPassword[i])
                {
                    numberCorrect++;
                }
            }
            return numberCorrect;
        }

        /// <summary>
        /// Called by the class constructor to pick a correct password out of the list of potential passwords stored in the class field.
        /// </summary>
        private void ChooseACorrectPassword()
        {
            CorrectPassword = PotentialPasswords[RandomProvider.Next(0, (PotentialPasswords.Count - 1))];
        }

        /// Keeps generating lists until finding one with enough characters in common.
        /// 'Enough characters' is determined by the passed argument, as are the number of passwords to generate,
        /// and the length of the passwords to generate.
        /// Does it's magic using an instance of the PasswordGenerator class possessed by this class.
        /// This is O(scary), especially if the RNG is unkind, but we will see if it is problem or not through testing.
        /// Working programs come first, after all! Optimization second!
        /// There is almost certainly a better way to do this, but that is a job for later.
        private List<string> GetPasswordsWithEnoughLettersInCommon(int howManyPasswords, int passwordLength, int howManyInCommon)
        {
            // TODO: Actually Use this Method
            bool enoughLettersInCommon = false;
            List<string> passwords = new List<string>();
            while (enoughLettersInCommon == false)
            {
                passwords = PasswordGenerator.GeneratePasswords(howManyPasswords, passwordLength);
                enoughLettersInCommon = EnoughLettersInCommon(passwords, howManyInCommon);
            }
            return passwords;
        }

        /// <summary>
        /// Determines whether there are enough letters in common between the passwords in the list.
        /// This is needed to decrese the difficulty of the game, since if I just chose random passwords
        /// from the dictionary file, we would likely end up with words that have little to no letters exactly in common,
        /// making guessing the correct password from the number of letters in common with the player's incorrect
        /// guesses much more difficult, if not impossible.
        /// </summary>
        /// <param name="passwords">The list of passwords to check.</param>
        /// <param name="howManyLetters">The number of letters that must be in common among the passwords.</param>
        /// <returns></returns>
        private bool EnoughLettersInCommon(List<string> passwords, int howManyLetters)
        {
            // TODO: Can we make this less O(Bad)?
            int lettersInCommon = 0;
            // For each password in the list of passwords...
            for (int passwordToCompareTo = 0; passwordToCompareTo < passwords.Count; passwordToCompareTo++)
            {
                // We want to compare each character in this password...
                for (int characterToCompareTo = 0; characterToCompareTo < passwords[0].Length; characterToCompareTo++)
                {
                    // Against every other password...
                    for (int password = passwordToCompareTo; password < passwords.Count; password++)
                    {
                        // At the corresponding character.
                        for (int character = 0; character < passwords[0].Length; character++)
                        {
                            // If the password to look at is not the same is the password we are comparing against...
                            if ((password == passwordToCompareTo) == false)
                            {
                                // If the characters are equal...
                                if (passwords[passwordToCompareTo][characterToCompareTo]
                                    == passwords[password][character])
                                {
                                    lettersInCommon++;
                                }
                            }
                        }
                    }
                }
            }
            return (lettersInCommon >= howManyLetters) ? true : false;
        }
    }
}