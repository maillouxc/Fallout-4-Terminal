using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Fallout_Terminal.Sound
{
    class SoundManager
    {
        private SoundPlayer SoundPlayer;

        /// <summary>
        /// Basic constructor that initializes the MediaPlayer.
        /// </summary>
        public SoundManager()
        {
            SoundPlayer = new SoundPlayer();
        }

        /// <summary>
        /// Plays the sound at the given path. Only accepts .wav files.
        /// </summary>
        /// <param name="FilePath">A string representing the path to the file.</param>
        public void PlaySound(string filePath)
        {
            Uri uri = new Uri(filePath, UriKind.Relative);
            SoundPlayer.SoundLocation = filePath;
            SoundPlayer.LoadAsync();
            SoundPlayer.Play();
        }
    }
}
