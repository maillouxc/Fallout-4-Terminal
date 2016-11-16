﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class MemoryDump
    {
        // This is the Number of Columns * Width of Columns * Number of Rows.
        private const int LENGTH = 384;

        // TODO Should replace this with a RandomProvider based rng.
        Random random = new Random();

        private GarbageCharacterGenerator GarbageCharacterGenerator;
        private List<string> passwords;

        public string Contents { get; private set; }

        public MemoryDump(List<string> passwords)
        {
            Contents = "";
            GarbageCharacterGenerator = new GarbageCharacterGenerator();
            this.passwords = passwords;
            PopulateContentsWithGarbageCharacters();
            PopulateContentsWithPasswords();
            Console.WriteLine(Contents); // TESTING ONLY
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
                    position = random.Next(0, (LENGTH - 1));
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
                if (IsLetter(Contents[position + passwords[0].Length + 1]))
                {
                    return false;
                }
            }
            // Is there another password immediately to the left?
            if ((position != 0) && (IsLetter(Contents[position - 1])))
            {
                return false;
            }
            // If we got here, there is room.
            return true;
        }

        /// <summary>
        /// Returns true if the given character is a letter a-z, false if not. Case insensitive.
        /// </summary>
        /// <param name="character">The character to test.</param>
        private bool IsLetter(char character)
        {
            character = Char.ToLower(character);
            char[] letters = {'a','b','c','d','e','f','g','h','i','j','k','l','m'
                    ,'n','o','p','q','r','s','t','u','v','w','x','y','z'};
            return letters.Contains(character) ? true : false;
        }
    }
}