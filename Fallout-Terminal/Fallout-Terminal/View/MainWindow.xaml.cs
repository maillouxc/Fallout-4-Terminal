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

namespace Fallout_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TerminalViewModel ViewModel;

        internal SoundManager SoundManager;
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
            SoundManager = new SoundManager();
            SelectionManager = new View.SelectionManager(this);
            LeftPasswordColumn.PreviewKeyDown += new KeyEventHandler(SelectionManager.OnKeyDown);
            RightPasswordColumn.PreviewKeyDown += new KeyEventHandler(SelectionManager.OnKeyDown);
        }

        private void powerButton_Click(object sender, RoutedEventArgs e)
        {
            if(ViewModel.PowerIsOn)
            {
                SoundManager.PlaySound(@"..\..\Resources\Sounds\powerOff.wav");
                ViewModel.PowerOff();
            }
            else
            {
                ViewModel.PowerOn();
                SoundManager.PlaySound(@"..\..\Resources\Sounds\powerOn.wav");
                //TODO: Think about this some more. Is this the right way to do this?
            }

        }

        private void TerminalScreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO Determine what logic, if any, needs to go here.
            // Possibly sound logic?
        }
    }
}
