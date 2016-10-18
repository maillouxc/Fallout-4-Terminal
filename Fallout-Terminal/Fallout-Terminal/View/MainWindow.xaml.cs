using Fallout_Terminal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Fallout_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewModel.TerminalViewModel viewModel;

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
        }


        /// <summary>
        /// A method with various things to execute once the window is fully loaded. Fixes that pesky focus bug.
        /// This method should NOT be left like this in the final program. Most of the stuff in here is for testing purposes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // TESTING: DO NOT USE IN PRODUCTION CODE

            // Test UI stuff.
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


        }

        private void powerButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO Add Logic for Power Button.
        }

        private void TerminalScreen_TextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO Determine what logic, if any, needs to go here.
        }
    }
}
