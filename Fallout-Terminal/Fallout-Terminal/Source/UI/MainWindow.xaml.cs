using Fallout_Terminal.Source.Logic;
using Fallout_Terminal.Source.Logic.CharGrid;
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
            charArray.InsertString("Welcome to ROBCO Industries (TM) Termlink         ", 0, 0);
            charArray.InsertString("                                                  ", 0, 1);
            charArray.InsertString("Password Required                                 ", 0, 2);
            charArray.InsertString("                                                  ", 0, 3);
            charArray.InsertString("Attempts Remaining: # # # #                       ", 0, 4);
            charArray.InsertString("                                                  ", 0, 5);
            charArray.PrintCharArray();
            Console.WriteLine("");

            // Test password generation...
            List<string> testPasswords = new List<string>();
            PasswordsManager manager = new PasswordsManager();
            testPasswords = manager.GetPasswordsWithEnoughLettersInCommon(9, 4, 50);
            for(int i = 0; i < testPasswords.Count; i++)
            {
                Console.Write(testPasswords.ElementAt(i).ToString() + ", ");
            }

            // Test InputCharArray
            InputCharGrid inputCharGrid = new InputCharGrid();
            for(int i = 0; i < 20; i++)
            {
                inputCharGrid.InsertString("Entry denied.", 0, 0);
            }
            inputCharGrid.InsertString("Testing.", 0, 0);
            inputCharGrid.InsertString("Success!", 0, 0);
            inputCharGrid.InsertString("Error.", 0, 0);
            inputCharGrid.InsertString("$", 0, 0);
            inputCharGrid.InsertString("Chris.", 0, 0);
            inputCharGrid.InsertString("Testing.", 0, 0);
            inputCharGrid.InsertString("Init Lockout.", 0, 0);
            inputCharGrid.PrintCharArray();

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
