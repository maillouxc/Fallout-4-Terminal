using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Fallout_Terminal.Utilities;

namespace Fallout_Terminal.Sound
{
    class SoundManager
    {
     //   private MediaPlayer MediaPlayer;

        /// <summary>
        /// Basic constructor that initializes the MediaPlayer.
        /// </summary>
        public SoundManager()
        {
        //    MediaPlayer = new MediaPlayer();
        }

        /// <summary>
        /// Plays the sound at the given path. Only accepts .wav files.
        /// </summary>
        /// <param name="FilePath">A string representing the path to the file.</param>
        public void PlaySound(string filePath)
        {
            MediaPlayer MediaPlayer = new MediaPlayer();
            Uri uri = new Uri(filePath, UriKind.Relative);
            MediaPlayer.Open(uri);
            MediaPlayer.Volume = 100;
            MediaPlayer.Play();
        }

        /// <summary>
        /// Plays a random typing sound.
        /// </summary>
        public void PlayTypingSound()
        {
            int randomInt = RandomProvider.Next(2, 11);
            string filePath = "";
            switch (randomInt)
            {
                /*
                case 1:
                    filePath = @"..\..\Resources\Sounds\typing1.wav";
                    break;
                */
                case 2:
                    filePath = @"..\..\Resources\Sounds\typing2.wav";
                    break;
                case 3:
                    filePath = @"..\..\Resources\Sounds\typing3.wav";
                    break;
                case 4:
                    filePath = @"..\..\Resources\Sounds\typing4.wav";
                    break;
                case 5:
                    filePath = @"..\..\Resources\Sounds\typing5.wav";
                    break;
                case 6:
                    filePath = @"..\..\Resources\Sounds\typing6.wav";
                    break;
                case 7:
                    filePath = @"..\..\Resources\Sounds\typing7.wav";
                    break;
                case 8:
                    filePath = @"..\..\Resources\Sounds\typing8.wav";
                    break;
                case 9:
                    filePath = @"..\..\Resources\Sounds\typing9.wav";
                    break;
                case 10:
                    filePath = @"..\..\Resources\Sounds\typing10.wav";
                    break;
                case 11:
                    filePath = @"..\..\Resources\Sounds\typing11.wav";
                    break;
            }
            PlaySound(filePath);
        }

        /// <summary>
        /// Plays that dull "click" sound that is heard while each character on the screen is printed
        /// for the first time.
        /// </summary>
        public void PlayCharacterDisplaySound()
        {
            string filePath = @"..\..\Resources\Sounds\typing6.wav";
            PlaySound(filePath);
        }
    }
}
