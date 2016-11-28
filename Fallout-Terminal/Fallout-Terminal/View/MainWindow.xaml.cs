using Fallout_Terminal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Fallout_Terminal.ViewModel;
using Fallout_Terminal.Sound;
using Fallout_Terminal.View;
using System.Windows.Media;
using System.Threading.Tasks;

namespace Fallout_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TerminalViewModel ViewModel;

        internal SoundPlayer SoundManager;

        private SelectionManager SelectionManager;

        /// <summary>
        /// Creates an instance of the main window for the application, 
        /// which contains all other UI elements.
        /// </summary>
        public MainWindow()
        {
            // This method is just something that WPF apps MUST do. Don't touch it!
            InitializeComponent();
            // It's best to wait until stuff is fully loaded before manipulating it.
            // Not doing so can cause some obscure bugs.
            ViewModel = FindResource("ViewModel") as TerminalViewModel;
            SoundManager = new SoundPlayer();
            SelectionManager = new View.SelectionManager(this);
            // Using the "preview" events here allows us to detect the arrow key presses, which we otherwise can't.
            LeftPasswordColumn.PreviewKeyDown += new KeyEventHandler(SelectionManager.OnKeyDown);
            RightPasswordColumn.PreviewKeyDown += new KeyEventHandler(SelectionManager.OnKeyDown);
        }

        /// <summary>
        /// Fired whenever the powerButton is clicked.
        /// 
        /// Plays a sound and tells the viewModel to enable the power.
        /// </summary>
        private void powerButton_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel.PowerIsOn)
            {
                SoundPlayer.PlaySound(@"..\..\Resources\Sounds\powerOff.wav");
                ViewModel.PowerOff();
            }
            else
            {
                ViewModel.PowerOn();
                SoundPlayer.PlaySound(@"..\..\Resources\Sounds\powerOn.wav");
                //TODO: Think about this some more. Is this the right way to do this?
            }
        }

        /// <summary>
        /// Fired whenever the text in the LeftPasswordColumn changes.
        /// </summary>
        private async void LeftPasswordColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SoundManager != null)
            {
                await Task.Delay(TerminalViewModel.DELAY_TIME);
                SoundPlayer.PlayCharacterDisplaySound();
            }
        }

        /// <summary>
        /// Fired whenever the text in the RightPasswordColumn changes.
        /// </summary>
        private async void RightPasswordColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SoundManager != null)
            {
                await Task.Delay(TerminalViewModel.DELAY_TIME);
                SoundPlayer.PlayCharacterDisplaySound();
            }
        }

        /// <summary>
        /// Fired whenever the text in the left hex column is changed.
        /// </summary>
        private async void LeftHexColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SoundManager != null)
            {
                await Task.Delay(TerminalViewModel.DELAY_TIME);
                SoundPlayer.PlayCharacterDisplaySound();
            }
        }

        /// <summary>
        /// Fired whenever the text in the right hex column has changed.
        /// </summary>
        private async void RightHexColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SoundManager != null)
            {
                await Task.Delay(TerminalViewModel.DELAY_TIME);
                SoundPlayer.PlayCharacterDisplaySound();
            }
        }

        /// <summary>
        /// Fired whenever the headertext changes.
        /// </summary>
        private async void HeaderText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SoundManager != null)
            {
                await Task.Delay(TerminalViewModel.DELAY_TIME);
                SoundPlayer.PlayCharacterDisplaySound();
            }
        }

        /// <summary>
        /// Fired whenever the attempts remaining text is changed.
        /// </summary>
        private async void AttemptsText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SoundManager != null)
            {
                await Task.Delay(TerminalViewModel.DELAY_TIME);
                SoundPlayer.PlayCharacterDisplaySound();
            }
        }
    }
}
