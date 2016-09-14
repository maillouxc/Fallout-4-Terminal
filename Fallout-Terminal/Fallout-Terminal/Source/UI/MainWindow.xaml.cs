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


            // TESTING: DO NOT USE IN PRODUCTION CODE
            // Test garbage chars.
            Console.WriteLine("\n");
            GarbageCharacterGenerator garbageGenerator = new GarbageCharacterGenerator();
            for(int i=0; i<1000; i++)
            {
                char temp = garbageGenerator.GetGarbageCharacter();
                Console.Write(temp);
            }
            // Test charArray.
            MainCharArray charArray = new MainCharArray();
            Console.WriteLine("\n");
            charArray.PrintCharArray();
            Console.WriteLine("\n");
           
            // Add Header text to the charArray.
            charArray.InsertString("ROBCO (TM) TERMLINK PROTOCOL                      ", 0, 0);
            charArray.InsertString("                                                  ", 0, 1);
            charArray.InsertString("Enter password now:                               ", 0, 2);
            charArray.InsertString("                                                  ", 0, 3);
            charArray.InsertString("Attempts remaining: # # # #                       ", 0, 4);
            charArray.InsertString("                                                  ", 0, 5);

            charArray.PrintCharArray();
            // Add blank line after test results to make it easier to find things.
            Console.WriteLine("\n");
            // END TESTING




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
