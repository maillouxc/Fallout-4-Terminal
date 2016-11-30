using Fallout_Terminal.Utilities;
using System;
using System.Collections.Generic;

namespace Fallout_Terminal.Model
{
    /// <summary>
    /// Handles everything to do with how the rest of the game interacts with passwords.
    /// This includes things like storing the list of potential passwords,
    /// checking if a given password is correct, etc.
    /// </summary>
    public class PasswordManager
    {
        private const int MAX_PASSWORD_LENGTH = 11;
        private const int MIN_PASSWORD_LENGTH = 4;
        private const int MINIMUM_NUMBER_OF_PASSWORDS = 6;
        private const int MAXIMUM_NUMBER_OF_PASSWORDS = 12;
        private const double PERCENTAGE_NEEDED = 0.3;

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
            NumberOfPasswordsToGenerate = RandomProvider.Next(MINIMUM_NUMBER_OF_PASSWORDS, MAXIMUM_NUMBER_OF_PASSWORDS);
            PasswordLength = RandomProvider.Next(MIN_PASSWORD_LENGTH, MAX_PASSWORD_LENGTH);
            PasswordGenerator = new PasswordGenerator();
            int lettersInCommonNeeded = DetermineHowManyLettersNeededInCommon(NumberOfPasswordsToGenerate, PasswordLength, PERCENTAGE_NEEDED);
            PotentialPasswords = PasswordGenerator.GeneratePasswords(NumberOfPasswordsToGenerate, PasswordLength, lettersInCommonNeeded);
            ChooseACorrectPassword();
            // Convert passwords to uppercase:
            for (int password = 0; password < PotentialPasswords.Count; password++)
            {
                PotentialPasswords[password] = PotentialPasswords[password].ToUpper();
            }
        }

        /// <summary>
        /// Returns the number of chars that the passed string has in common with the correct password.
        /// </summary>
        public int GetNumberOfCorrectChars(string passwordToCheck)
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

        /// <summary>
        /// Returns an int representing how many letters would need to be in common among the list of passwords,
        /// given the number of passwords that will be generated and the length of passwords to be generated,
        /// as well as a given desired minimum percentage match among the words.
        /// </summary>
        private int DetermineHowManyLettersNeededInCommon(int numberOfPasswords, int passwordLength, double percentageDesired)
        {
            double result;
            int totalLetters = passwordLength * numberOfPasswords;
            result = totalLetters * percentageDesired;
            return (int)result;
        }
    }
}