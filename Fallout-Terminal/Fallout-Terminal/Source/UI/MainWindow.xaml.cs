using Fallout_Terminal.Source.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
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
            for(int i = 0; i < 1000; i++)
            {
                char temp = garbageGenerator.GetGarbageCharacter();
                Console.Write(temp);
            }
            // Test charArray.
            MainCharGrid charArray = new MainCharGrid();
            Console.WriteLine("");
            charArray.PrintCharArray();
            Console.WriteLine("");
           
            // Add Header text to the charArray.
            charArray.InsertString("ROBCO (TM) TERMLINK PROTOCOL                      ", 0, 0);
            charArray.InsertString("                                                  ", 0, 1);
            charArray.InsertString("Enter password now:                               ", 0, 2);
            charArray.InsertString("                                                  ", 0, 3);
            charArray.InsertString("Attempts remaining: # # # #                       ", 0, 4);
            charArray.InsertString("                                                  ", 0, 5);
            charArray.PrintCharArray();
            Console.WriteLine("");

            // Test password generator.
            PasswordGenerator passwordGenerator = new PasswordGenerator();
            List<string> testPasswords = new List<string>();
            testPasswords = passwordGenerator.GeneratePotentialPasswords(9, 7);
            for(int i = 0; i < testPasswords.Count; i++)
            {
                Console.Write(testPasswords.ElementAt(i).ToString() + ", ");
            }

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
