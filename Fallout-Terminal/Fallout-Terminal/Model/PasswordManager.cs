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
        // TODO: Determine programmatically.
        private const int DEFAULT_NUMBER_OF_PASSWORDS = 8;
        private const int DEFAULT_PASSWORD_LENGTH = 6;

        public List<string> PotentialPasswords;

        private string CorrectPassword;
        private int NumberOfPasswordsToGenerate;
        public int PasswordLength;
        private PasswordGenerator PasswordGenerator;

        public PasswordManager()
        {
            NumberOfPasswordsToGenerate = DEFAULT_NUMBER_OF_PASSWORDS;
            PasswordLength = DEFAULT_PASSWORD_LENGTH;
            PasswordGenerator = new PasswordGenerator();
            PotentialPasswords = PasswordGenerator.GeneratePasswords(NumberOfPasswordsToGenerate, PasswordLength);
            ChooseACorrectPassword();

            // TESTING:
            Console.WriteLine(CorrectPassword);
        }

        /// <summary>
        /// Checks the input password to see if it is correct. If
        /// </summary>
        /// <returns>-1 if the password is correct,
        /// otherwise the number of characters in common with the correct password.</returns>
        internal int CheckPassword(string passwordToCheck)
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
        /// Returns the number of characters in common with the correct password.
        /// </summary>
        /// <param name="passwordToCheck"></param>
        /// <returns>The number of characters in common with the correct password.</returns>
        private int GetNumberOfCorrectChars(string passwordToCheck)
        {
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

        private void ChooseACorrectPassword()
        {
            Random random = new Random();
            CorrectPassword = PotentialPasswords[random.Next(0, (PotentialPasswords.Count - 1))];
        }

        internal List<string> GetPasswordsWithEnoughLettersInCommon(int howManyPasswords, int passwordLength, int howManyInCommon)
        {
            bool enoughLettersInCommon = false;
            List<string> passwords = new List<string>();
            // Keep generating lists until we get one with enough in common.
            // This could be O(scary), especially if the RNG is unkind, but we will see.
            // There is almost certainly a better way to do this, but that is a job for later.
            while (enoughLettersInCommon == false)
            {
                passwords = PasswordGenerator.GeneratePasswords(howManyPasswords, passwordLength);
                enoughLettersInCommon = EnoughLettersInCommon(passwords, howManyInCommon);
            }
            return passwords;
        }

        private bool EnoughLettersInCommon(List<string> passwords, int howManyLetters)
        {
            int lettersInCommon = 0;
            // HERE BE DRAGONS!!!
            // For each password in the list of passwords...
            for(int passwordToCompareTo = 0; passwordToCompareTo < passwords.Count; passwordToCompareTo++)
            {
                // We want to compare each character in this password...
                for(int characterToCompareTo = 0; characterToCompareTo < passwords[0].Length; characterToCompareTo++)
                {
                    // Against every other password...
                    for (int password = passwordToCompareTo; password < passwords.Count; password++)
                    {
                        // At the corresponding character.
                        for (int character = 0; character < passwords[0].Length; character++)
                        {
                            // If the password to look at is not the same is the password we are comparing against...
                            if((password == passwordToCompareTo) == false)
                            {
                                // If the characters are equal...
                                if(passwords[passwordToCompareTo][characterToCompareTo] 
                                    == passwords[password][character])
                                {
                                    lettersInCommon++;
                                }
                            }
                        }
                    }
                }
            }
            if(lettersInCommon >= howManyLetters)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}