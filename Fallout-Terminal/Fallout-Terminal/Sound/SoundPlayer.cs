using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Fallout_Terminal.Utilities;

namespace Fallout_Terminal.Sound
{
    /// <summary>
    /// Manages all sound aspects related to the game, 
    /// which basically entails just playing various sounds on command.
    /// </summary>
    /// <remarks>
    /// You might hate me for making this static because it somewhat clashes with MVVM in it's implied usage,
    /// but I'm not writing a half-dozen events to notify the viewModel about the various sounds. 
    /// The world will not end if I play a sound directly from the model. 
    /// I know it's wrong, let's just leave it at that, okay?
    /// </remarks>
    public class SoundPlayer
    {
        /// <summary>
        /// General purpose method to play the sound file at the given path. Accepts .wav files.
        /// </summary>
        /// <remarks>
        /// Most uses of this method will be internal to this class, via the other sound playing methods
        /// which are used to play a specific sound. We could've just forced the user to pass in a string of the 
        /// file path for every sound, or even use enums to hold filepaths for the various sounds and pass those in
        /// as arguments here, but individual method way is probably better here since it results in cleaner code,
        /// and I have a little more control over how individual sounds are handled.
        /// 
        /// I'm pretty sure this method can accept .mp3 files as well, but it hasn't been tested for that, so try that at your own risk.
        /// </remarks>
        /// <param name="FilePath">A string representing the path to the file.</param>
        public static void PlaySound(string filePath)
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
        public static void PlayTypingSound()
        {
            int randomInt = RandomProvider.Next(2, 11);
            string filePath = "";
            switch (randomInt)
            {
                /*
                case 1:
                    // This sound has multiple keypress sounds, which is weird for only a single press.
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
        public static void PlayCharacterDisplaySound()
        {
            string filePath = @"..\..\Resources\Sounds\typing6.wav";
            PlaySound(filePath);
        }

        /// <summary>
        /// Plays the sound for the Enter key press.
        /// </summary>
        public static void PlayEnterKeySound()
        {
            string filePath = @"..\..\Resources\Sounds\enterKey.wav";
            PlaySound(filePath);
        }

        /// <summary>
        /// Plays the sound for incorrect password input.
        /// </summary>
        public static void PlayIncorrectPasswordSound()
        {
            string filePath = @"..\..\Resources\Sounds\wrongPassword.wav";
            PlaySound(filePath);
        }

        /// <summary>
        /// Plays the appropriate sound for when a user enters the correct password.
        /// </summary>
        public static void PlayCorrectPasswordSound()
        {
            string filePath = @"..\..\Resources\Sounds\correctPassword.wav";
            PlaySound(filePath);
        }
    }
}
