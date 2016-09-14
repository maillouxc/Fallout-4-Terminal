using Fallout_Terminal.Source.Logic;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Fallout_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // TESTING:
            GarbageCharacterGenerator garbageGenerator = new GarbageCharacterGenerator();
            for(int i=0; i<1000; i++)
            {
                char temp = garbageGenerator.GetGarbageCharacter();
                Console.Write(temp);
            }        
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
