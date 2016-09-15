namespace Fallout_Terminal.Source.Logic
{
    // TODO: Documentation.
    public class PasswordsManager
    {
        private string correctPassword;
        private int numberOfPasswordsToGenerate;
        private int passwordLength;

        // TODO: Documentation.
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

        private int GetNumberOfCorrectChars(string passwordToCheck)
        {
            // TODO: Write Method.
        }
    }
}
