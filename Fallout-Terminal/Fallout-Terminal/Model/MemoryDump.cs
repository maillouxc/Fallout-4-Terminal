using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class MemoryDump
    {
        Random random = new Random();

        // Number of Columns * Width of Columns * Number of Rows.
        private const int LENGTH = 384;

        private GarbageCharacterGenerator GarbageCharacterGenerator;
        //private PasswordManager PasswordManager;
        private List<string> passwords;

        //private string contents = "";
        public string Contents { get; private set; }

        public MemoryDump(List<string> passwords)
        {
            Contents = "";
            GarbageCharacterGenerator = new GarbageCharacterGenerator();
            //PasswordManager PasswordManager = new PasswordManager();
            this.passwords = passwords;
            PopulateContentsWithGarbageCharacters();
            Console.WriteLine(Contents);    // TESTING
            PopulateContentsWithPasswords();

            // TESTING ONLY:
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Contents:");
            Console.WriteLine(Contents);
            Console.WriteLine("");
        }

        private void PopulateContentsWithGarbageCharacters()
        {
            for(int index = 0; index < LENGTH; index++)
            {
                Contents += GarbageCharacterGenerator.GetGarbageCharacter();
            }
        }

        private void PopulateContentsWithPasswords()
        {
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

        private bool IsRoomForPassword(int position)
        {
            for (int i = 0; i < passwords[0].Length; i++)
            {
                if ((position + i) >= LENGTH)
                {
                    // If true, the end of the password would run over the length of the string. No room!
                    return false;
                }
            }
            if ((position + (passwords[0].Length) + 1 <= LENGTH))
            {
                if (IsLetter(Contents[position + passwords[0].Length + 1]))
                {
                    return false;
                }
            }
            if ((position != 0) && (IsLetter(Contents[position - 1])))
            {
                return false;
            }
            // If we got here, there is room.
            return true;
        }

        private bool IsLetter(char character)
        {
            character = Char.ToLower(character);
            char[] letters = {'a','b','c','d','e','f','g','h','i','j','k','l','m'
                    ,'n','o','p','q','r','s','t','u','v','w','x','y','z'};
            if(letters.Contains(character))
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