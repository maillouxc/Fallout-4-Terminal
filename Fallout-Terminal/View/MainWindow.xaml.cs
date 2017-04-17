using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Fallout_Terminal.ViewModel;
using Fallout_Terminal.Sound;
using Fallout_Terminal.View;
using System.Threading.Tasks;

namespace Fallout_Terminal
{
    /// <summary>
    /// Code-behind for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TerminalViewModel ViewModel;

        private SelectionManager SelectionManager;

        /// <summary>
        /// Initializes an instance of the main window for the application.
        /// </summary>
        public MainWindow()
        {
            // This is just something that WPF apps MUST do.
            InitializeComponent();
            ViewModel = FindResource("ViewModel") as TerminalViewModel;
            SelectionManager = new View.SelectionManager(this);
            // Using the "preview" events here allows us to detect the arrow key presses, which we otherwise can't.
            LeftPasswordColumn.PreviewKeyDown += new KeyEventHandler(SelectionManager.OnKeyDown);
            RightPasswordColumn.PreviewKeyDown += new KeyEventHandler(SelectionManager.OnKeyDown);
        }

        /// <summary>
        /// Fired whenever the powerButton is clicked. Plays the appropriate sound depending on current power state, 
        /// and tells the ViewModel to toggle the power.
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
                SoundPlayer.PlaySound(@"..\..\Resources\Sounds\powerOn.wav");
                ViewModel.PowerOn();
            }
        }

        /// <summary>
        /// Fired whenever the text in the LeftPasswordColumn changes. Plays the appropriate sound.
        /// </summary>
        private void LeftPasswordColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            SoundPlayer.PlayCharacterDisplaySound();
        }

        /// <summary>
        /// Fired whenever the text in the RightPasswordColumn changes. Plays the appropriate sound.
        /// </summary>
        private void RightPasswordColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            SoundPlayer.PlayCharacterDisplaySound();
        }

        /// <summary>
        /// Fired whenever the text in the left hex column is changed. Plays the appropriate sound.
        /// </summary>
        private void LeftHexColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            SoundPlayer.PlayCharacterDisplaySound();
        }

        /// <summary>
        /// Fired whenever the text in the right hex column has changed. Plays the appropriate sound.
        /// </summary>
        private void RightHexColumn_TextChanged(object sender, TextChangedEventArgs e)
        {
            SoundPlayer.PlayCharacterDisplaySound();
        }

        /// <summary>
        /// Fired whenever the headertext changes. Plays the appropriate sound.
        /// </summary>
        private void HeaderText_TextChanged(object sender, TextChangedEventArgs e)
        {
            SoundPlayer.PlayCharacterDisplaySound();
        }

        /// <summary>
        /// Fired whenever the attempts remaining text is changed. Plays the appropriate sound.
        /// </summary>
        private void AttemptsText_TextChanged(object sender, TextChangedEventArgs e)
        {
            SoundPlayer.PlayCharacterDisplaySound();
        }
    }
}
