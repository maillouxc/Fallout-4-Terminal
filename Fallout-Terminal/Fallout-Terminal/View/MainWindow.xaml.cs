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

namespace Fallout_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TerminalViewModel viewModel;
        SoundManager SoundManager;

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
            Loaded += Window_Loaded;
            viewModel = FindResource("viewModel") as ViewModel.TerminalViewModel;
            SoundManager = new SoundManager();       
        }

        /// <summary>
        /// A method filled with various things to execute once the window is fully loaded. Fixes that pesky focus bug.
        /// This method should NOT be left like this in the final program. Most of the stuff in here is for testing purposes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Test some stuff todo with character selection.
            /*
            LeftPasswordColumn.Focus();
            LeftPasswordColumn.Document.Blocks.Clear();
            LeftPasswordColumn.Document.Blocks.Add(new Paragraph(new Run("GJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYL")));
            TextPointer start = LeftPasswordColumn.Document.ContentStart;
            start = start.GetPositionAtOffset(3);
            TextPointer end = LeftPasswordColumn.Document.ContentEnd;
            end = end.GetPositionAtOffset(-3);
            LeftPasswordColumn.Selection.Select(start, end);
            LeftPasswordColumn.IsInactiveSelectionHighlightEnabled = true;

            RightPasswordColumn.Focus();
            RightPasswordColumn.Document.Blocks.Clear();
            RightPasswordColumn.Document.Blocks.Add(new Paragraph(new Run("GJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYLGJFJGJHUTIYL")));
            TextPointer start2 = RightPasswordColumn.Document.ContentStart;
            start2 = start2.GetPositionAtOffset(3);
            TextPointer end2 = RightPasswordColumn.Document.ContentEnd;
            end2 = end2.GetPositionAtOffset(-3);
            RightPasswordColumn.Selection.Select(start2, end2);
            RightPasswordColumn.IsInactiveSelectionHighlightEnabled = true;
            */
        }

        private void powerButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: different sound on power on than power off.
            SoundManager.PlaySound(@"..\..\Resources\Sounds\powerOn.wav");
            //TODO: Think about this some more. Is this the right way to do this?
            viewModel.InitializeCharacters();
        }

        private void TerminalScreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO Determine what logic, if any, needs to go here.
            // Possibly sound logic?
        }
    }
}
