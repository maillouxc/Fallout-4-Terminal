using Fallout_Terminal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class MemoryDump
    {
        public const int LINE_LENGTH = 12;
        public const int NUMBER_OF_LINES = 16;

        private const int LENGTH = LINE_LENGTH * NUMBER_OF_LINES * 2; // 2 columns

        /// <summary>
        /// The contents of the memory dump.
        /// </summary>
        public string Contents { get; private set; }

        public delegate void ContentsChangedHandler(object sender, EventArgs args);
        public event ContentsChangedHandler OnContentsChanged;

        private GarbageCharacterGenerator GarbageCharacterGenerator;
        private List<string> passwords;

        /// <summary>
        /// Creates an instance of MemoryDump, full of potential passwords and ready to go.
        /// </summary>
        /// <param name="passwords"></param>
        public MemoryDump(List<string> passwords)
        {
            Contents = "";
            GarbageCharacterGenerator = new GarbageCharacterGenerator();
            this.passwords = passwords;
            PopulateContentsWithGarbageCharacters();
            PopulateContentsWithPasswords();
        }

        /// <summary>
        /// Removes the provided password from the memory dump, and replaces it with dots.
        /// Assumes there is never more than one instance of the given password.
        /// 
        /// Note: This method can actually be used to replace any string within the memory dump with dots,
        /// but there is really no reason to do this.
        /// </summary>
        public void Remove(string password)
        {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < password.Length; i++)
            {
                b.Append('.');
            }
            string dots = b.ToString();
            Contents = Contents.Replace(password, dots);
            NotifyContentsChanged(new EventArgs());
        }

        /// <summary>
        /// Call to notify program that contents of memory dump have changed.
        /// </summary>
        /// <param name="args"></param>
        private void NotifyContentsChanged(EventArgs args)
        {
            OnContentsChanged?.Invoke(this, args);
        }
        
        /// <summary>
        /// Fills the memory dump contents with 'garbage-characters' like the ones used in Fallout.
        /// Fetches random garbage characters from the GarbageCharacterGenerator class instance within this class instance.
        /// Important: This method APPENDS the characters to the memory dump, so it is critical that the dump be empty when called.
        /// This method should be called when initializing the memory dump.
        /// </summary>
        private void PopulateContentsWithGarbageCharacters()
        {
            for(int index = 0; index < LENGTH; index++)
            {
                Contents += GarbageCharacterGenerator.GetGarbageCharacter();
            }
        }

        /// <summary>
        /// Fills the contents of the memory dump with generated game passwords. 
        /// Ensures that there is room to insert each password.
        /// 
        /// Uses random numbers to determine where to attempt insertion of each password.
        /// Note: If the total length of characters in all of the passwords becomes too long, this method can
        /// cause serious performance problems due to the difficulty in finding room for each of the passwords
        /// by using random attempts. If the total length of all of the passwords exceeds the length of the memory dump,
        /// you will have a problem.
        /// </summary>
        private void PopulateContentsWithPasswords()
        {
            // TODO: Fix to ensure that each position is only attempted once.
            for (int index = 0; index < passwords.Count; index++)
            {
                int position = 0;
                bool success = false;
                while(!success)
                {
                    position = RandomProvider.Next(0, (LENGTH - 1));
                    if (IsRoomForPassword(position))
                    {
                        for(int j = 0; j < passwords[index].Length; j++)
                        {
                            int offsetPosition = position + j;
                            char[] temp;
                            temp = Contents.ToCharArray();
                            temp[offsetPosition] = passwords[index].ToCharArray()[j];
                            Contents = new String(temp);
                        }
                        success = true;
                    }
                }
            }
        }

        /// <summary>
        /// Returns true when there is room to insert a game password into the memory dump at the given location, false if not.
        /// Assumes all passwords are the same length, and gets this length from the list from fields within the class.
        /// </summary>
        /// <param name="position">The proposed insertion position of the password.</param>
        private bool IsRoomForPassword(int position)
        {
            for (int i = 0; i < passwords[0].Length; i++)
            {
                // Will the end of the password would run over the length of the string?
                if ((position + i) >= LENGTH)
                {
                    return false;
                }
            }
            // Is there another password immediately to the right?
            if ((position + (passwords[0].Length) + 1 <= LENGTH))
            {
                if (Char.IsLetter(Contents[position + passwords[0].Length + 1]))
                {
                    return false;
                }
            }
            // Is there another password immediately to the left?
            if ((position != 0) && (Char.IsLetter(Contents[position - 1])))
            {
                return false;
            }
            // If we got here, there is room.
            return true;
        }
    }
}