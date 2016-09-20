namespace Fallout_Terminal.Source.Logic
{
    /// <summary>
    /// Handles everything to do with how the rest of the game interacts with passwords.
    /// This includes things like storing the list of potential passwords, storing the correct password,
    /// checking if a given password is correct, etc.
    /// </summary>
    public class PasswordsManager
    {
        private string correctPassword;
        private int numberOfPasswordsToGenerate;

        private PasswordGenerator passwordGenerator;
        // TODO: Use this ^

        public PasswordsManager()
        {
            this.passwordGenerator = new PasswordGenerator();
            // TODO: Finish constructor.
        }

        /// <summary>
        /// Property storing the list of potential password choices that
        /// the player can pick from.
        /// </summary>
        internal string[] potentialPasswords
        {
            get;
        }

        /// <summary>
        /// Checks the input password to see if it is correct. If
        /// </summary>
        /// <returns>-1 if the password is correct,
        /// otherwise the number of characters in common with the correct password.</returns>
        internal int CheckPassword(string passwordToCheck)
        {
            if(passwordToCheck == correctPassword)
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

            for(int i = 0; i < correctPassword.Length; i++)
            {
                if(passwordToCheck[i] == correctPassword[i])
                {
                    numberCorrect++;
                }
            }

            return numberCorrect;
        }
    }
}