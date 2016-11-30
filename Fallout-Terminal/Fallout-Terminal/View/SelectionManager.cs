using Fallout_Terminal.Model;
using Fallout_Terminal.Sound;
using Fallout_Terminal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

namespace Fallout_Terminal.View
{
    /// <summary>
    /// Manages the selection of text on the screen that the player does with the arrow keys.
    /// </summary>
    public class SelectionManager
    {
        private MainWindow MainWindow;
        private TextPointer LeftStart;
        private TextPointer LeftEnd;
        private TextPointer RightStart;
        private TextPointer RightEnd;
        private int Y;
        private int X;
        private int LeftJumpOffset;
        private int RightJumpOffset;
        private enum Side
        {
            Left,
            Right
        }
        private Side ActiveSide;
        private bool IsWordCurrentlySelected = false;
        private bool IsBracketTrickSelected = false;
        private List<string> UsedBracketTricks = new List<string>();

        /// <summary>
        /// Creates a new instance of SelectionManager.
        /// </summary>
        public SelectionManager(MainWindow window)
        {
            MainWindow = window;
            MainWindow.ViewModel.MemoryDumpContentsChanged += OnMemoryDumpContentsChanged;
            MainWindow.ViewModel.TerminalReady += OnTerminalReady;
            MainWindow.ViewModel.OnPowerOff += OnPowerOff;
        }

        /// <summary>
        /// Handler for keyPress Events while the program is running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal void OnKeyDown(object sender, KeyEventArgs args)
        {
            switch (args.Key)
            {
                case (Key.Left):
                    MoveSelectionLeft();
                    break;
                case (Key.Right):
                    MoveSelectionRight();
                    break;
                case (Key.Up):
                    MoveSelectionUp();
                    break;
                case (Key.Down):
                    MoveSelectionDown();
                    break;
                case (Key.Enter):
                    SubmitSelection(GetSelection());
                    SoundPlayer.PlayEnterKeySound();
                    break;
            }
        }

        /// <summary>
        /// Called when initialization is complete to enable selection to begin. 
        /// This method will ensure that no weird stuff happens when the program is initializing.
        /// It also ensures that when initialization is complete, the first character in the first
        /// column starts out selected and focused. Finally, it also resets instance variables 
        /// as needed to ensure no data is accidentally carried over from one play to the next.
        /// This sounds like too many responsibilities for one method to have, I know, 
        /// but if you read the method you'll see it's really fine.
        /// </summary>
        private void OnTerminalReady(object sender, EventArgs args)
        {
            X = 0;
            Y = 0;
            MainWindow.LeftPasswordColumn.Focusable = true;
            MainWindow.RightPasswordColumn.Focusable = true;
            ActiveSide = Side.Left;
            MainWindow.LeftPasswordColumn.Focus();
            UsedBracketTricks.Clear();
            MoveSelection(); // This call will highlight the first character.
        }

        /// <summary>
        /// Called when the program has notified that power is being turned off, allowing
        /// us to take appropriate action.
        /// 
        /// In this case, appropriate action means that we need to reset the password columns
        /// to be non-focusable. Otherwise, the bug we fixed where users could do weird stuff
        /// while the program was initializing would still be present once the player power
        /// cycled the terminal.
        /// </summary>
        private void OnPowerOff(object sender, EventArgs args)
        {
            MainWindow.LeftPasswordColumn.Focusable = false;
            MainWindow.LeftPasswordColumn.Focusable = false;
        }

        /// <summary>
        /// Moves the selection cursor left.
        /// </summary>
        private void MoveSelectionLeft()
        {
            if (IsWordCurrentlySelected)
            {
                JumpToLeftEndOfSelectedWord();
            }
            if (X > 0)
            {
                X--;
                MoveSelection();
            }
        }

        /// <summary>
        /// Moves the selection cursor right.
        /// </summary>
        private void MoveSelectionRight()
        {
            if (IsWordCurrentlySelected)
            {
                JumpToRightEndOfSelectedWord();
            }
            if (X < (TerminalModel.NUMBER_OF_COLUMNS * 2) - 1)
            {
                X++;
                MoveSelection();
            }
        }

        /// <summary>
        /// Moves the selection cursor up.
        /// </summary>
        private void MoveSelectionUp()
        {
            if (Y > 0)
            {
                Y--;
                MoveSelection();
            }
        }

        /// <summary>
        /// Moves the selection cursor down.
        /// </summary>
        private void MoveSelectionDown()
        {
            if (Y < TerminalModel.NUMBER_OF_LINES - 1)
            {
                Y++;
                MoveSelection();
            }
        }

        /// <summary>
        /// Actually moves the selection. Handles changing of focus as appropriate,
        /// and adjustment of the textPointer positions. Also handles special cases,
        /// such as words and bracket tricks.
        /// </summary>
        private void MoveSelection()
        {
            UpdateActiveSide();
            int offset = CalculateOffset(X, Y, ActiveSide);
            if(ActiveSide == Side.Left)
            {
                MainWindow.LeftPasswordColumn.Focus();
                LeftStart = MainWindow.LeftPasswordColumn.Document.ContentStart;
                LeftEnd = LeftStart.GetPositionAtOffset(1);
                LeftStart = LeftStart.GetPositionAtOffset(offset);
                LeftEnd = LeftStart.GetPositionAtOffset(1);
                MainWindow.LeftPasswordColumn.Selection.Select(LeftStart, LeftEnd);
            }
            else if (ActiveSide == Side.Right)
            {
                MainWindow.RightPasswordColumn.Focus();
                RightStart = MainWindow.RightPasswordColumn.Document.ContentStart;
                RightEnd = RightStart.GetPositionAtOffset(1);
                RightStart = RightStart.GetPositionAtOffset(offset);
                RightEnd = RightStart.GetPositionAtOffset(1);
                MainWindow.RightPasswordColumn.Selection.Select(RightStart, RightEnd);
            }
            ResizeSelectionForWords();
            HandleBracketTricks();
            NotifySelectionChanged();
        }

        /// <summary>
        /// Checks for bracket tricks at the current user selection point.
        /// If any are found, they are selected.
        /// </summary>
        private void HandleBracketTricks()
        {
            IsBracketTrickSelected = false;
            // Can't use a bracket trick more than once!
            if (IsBracketTrickUsed(X, Y))
            {
                return;
            }
            string selection = GetSelection();
            char startChar = selection[0];
            char endChar; // Not the actual endChar, but the needed one to complete the bracket pair.
            // Bracket tricks are only selectable from the left side, which simplifies things.
            switch (startChar)
            {
                case '(':
                    endChar = ')';
                    break;
                case '[':
                    endChar = ']';
                    break;
                case '<':
                    endChar = '>';
                    break;
                case '{':
                    endChar = '}';
                    break;
                default:
                    // Meaning there are no valid bracket tricks on this line...
                    return;
            }
            string textAfter;
            if (ActiveSide == Side.Left)
            {
                textAfter = LeftEnd.GetTextInRun(LogicalDirection.Forward);
            }
            else // ActiveSide == Side.Right
            {
                textAfter = RightEnd.GetTextInRun(LogicalDirection.Forward);
            }
            int offset = 0;
            // Only check the currently selected line
            while (offset < textAfter.Length && textAfter[offset] != '\n')
            {
                // No words are allowed between the bracket pairs.
                if (char.IsLetter(textAfter[offset]))
                {
                    return;
                }
                if (textAfter[offset] == endChar)
                {
                    IsBracketTrickSelected = true;
                    if (ActiveSide == Side.Left)
                    {
                        LeftEnd = LeftEnd.GetPositionAtOffset(offset + 1);
                        MainWindow.LeftPasswordColumn.Selection.Select(LeftStart, LeftEnd);
                        break;
                    }
                    else // ActiveSide == Side.Right
                    {
                        RightEnd = RightEnd.GetPositionAtOffset(offset + 1);
                        MainWindow.RightPasswordColumn.Selection.Select(RightStart, RightEnd);
                        break;
                    }
                }
                offset++;
            } 
        }

        /// <summary>
        /// Expands the text selection to fit entire words whenever a letter is selected.
        /// 
        /// <Remarks>
        /// This method is probably longer than it should be, but only because it handles two cases.
        /// It would be weird to have a seperate method for the left side and for the right side.
        /// </Remarks>
        /// </summary>
        private void ResizeSelectionForWords()
        {
            LeftJumpOffset = 0;
            RightJumpOffset = 0;
            bool finished = false;
            while (!finished)
            {
                if (ActiveSide == Side.Left)
                {
                    // We only need to check the adjacent characters if the current character is a letter.
                    if (Char.IsLetter(MainWindow.LeftPasswordColumn.Selection.Text[0]) || MainWindow.LeftPasswordColumn.Selection.Text[0] == '\n')
                    {
                        char charBefore = LeftStart.GetTextInRun(LogicalDirection.Backward).LastOrDefault();
                        char charAfter = LeftEnd.GetTextInRun(LogicalDirection.Forward).FirstOrDefault();
                        if (Char.IsLetter(charBefore) || charBefore == '\n')
                        {
                            LeftStart = LeftStart.GetPositionAtOffset(-1);
                            LeftJumpOffset++;
                        }
                        else if (Char.IsLetter(charAfter) || charAfter == '\n')
                        {
                            LeftEnd = LeftEnd.GetPositionAtOffset(1);
                            RightJumpOffset++;
                        }
                        else
                        {
                            finished = true;
                            IsWordCurrentlySelected = true;
                        }
                        MainWindow.LeftPasswordColumn.Selection.Select(LeftStart, LeftEnd);
                    }
                    else
                    {
                        finished = true;
                        IsWordCurrentlySelected = false;
                    }
                }
                else if (ActiveSide == Side.Right)
                {
                    if (Char.IsLetter(MainWindow.RightPasswordColumn.Selection.Text[0]) || MainWindow.RightPasswordColumn.Selection.Text[0] == '\n')
                    {
                        char charBefore = RightStart.GetTextInRun(LogicalDirection.Backward).LastOrDefault();
                        char charAfter = RightEnd.GetTextInRun(LogicalDirection.Forward).FirstOrDefault();
                        if (Char.IsLetter(charBefore) || charBefore == '\n')
                        {
                            RightStart = RightStart.GetPositionAtOffset(-1);
                            LeftJumpOffset++;
                        }
                        else if (Char.IsLetter(charAfter) || charAfter == '\n')
                        {
                            RightEnd = RightEnd.GetPositionAtOffset(1);
                            RightJumpOffset++;
                        }
                        else
                        {
                            finished = true;
                            IsWordCurrentlySelected = true;
                        }
                        MainWindow.RightPasswordColumn.Selection.Select(RightStart, RightEnd);
                    }
                    else
                    {
                        finished = true;
                        IsWordCurrentlySelected = false;
                    }
                }
            }
        }

        /// <summary>
        /// Adjusts selection cursor to just past the left end of the selected word.
        /// 
        /// If the selected word is broken across more than one line of input, than the cursor
        /// is moved to the appropriate position on the line above or below. This method will not
        /// allow the cursor to exit the bounds of the screen.
        /// </summary>
        private void JumpToLeftEndOfSelectedWord()
        {
            X -= LeftJumpOffset;
            int leftEndOfColumn = 0;
            if (ActiveSide == Side.Right)
            {
                leftEndOfColumn = TerminalModel.NUMBER_OF_COLUMNS;
            }
            if (X < leftEndOfColumn)
            {
                if (Y > 0)
                {
                    Y--;
                    X += TerminalModel.NUMBER_OF_COLUMNS + 1; // The +1 compensates for the X--; in MoveSelectionLeft()
                }
                else
                {
                    Y = 0;
                    X = 0;
                }
            }
            UpdateActiveSide();
        }

        /// <summary>
        /// Adjusts the selection cursor to just past the right end of the selected word.
        /// 
        /// If the selected word is broken across more than one line of input, than the cursor
        /// is moved to the appropriate position on the line above or below. This method will not
        /// allow the cursor to exit the bounds of the screen.
        /// </summary>
        private void JumpToRightEndOfSelectedWord()
        {
            X += RightJumpOffset;
            int rightEndOfColumn = TerminalModel.NUMBER_OF_COLUMNS - 1;
            if (ActiveSide == Side.Right)
            {
                rightEndOfColumn = (TerminalModel.NUMBER_OF_COLUMNS * 2) - 1;
            }
            if (X > rightEndOfColumn)
            {
                if (Y < TerminalModel.NUMBER_OF_LINES - 1)
                {
                    Y++;
                    X -= TerminalModel.NUMBER_OF_COLUMNS + 1; // The +1 compensates for the X++ in MoveSelectionRight()
                }
                else
                {
                    Y = TerminalModel.NUMBER_OF_LINES - 1;
                    X = (TerminalModel.NUMBER_OF_COLUMNS * 2) - 1;
                }
            }
            UpdateActiveSide();
        }

        /// <summary>
        /// Calculates the offset for positioning the textPointers, from a given x and y position.
        /// </summary>
        /// <param name="column">The column the current selection is in.</param>
        /// <returns></returns>
        private int CalculateOffset(int x, int y, Side column)
        {
            if (column == Side.Left)
            {
                return (2 + x) + (y * (TerminalModel.NUMBER_OF_COLUMNS + 1));
            }
            else if (column == Side.Right)
            {
                return (2 + x - TerminalModel.NUMBER_OF_COLUMNS) + (y * (TerminalModel.NUMBER_OF_COLUMNS + 1));
            }
            // I don't see how this would ever happen, but I suppose in this case returing 0 is logical.
            return 0;
        }

        /// <summary>
        /// Submits the current selection to the ViewModel to be processed by the game logic.
        /// 
        /// If the current selection happens to be a bracket trick, the X and Y coordinates of the
        /// trick are added to the list of used bracket tricks, to ensure the players can't use the 
        /// same bracket trick multiple times.
        /// </summary>
        private void SubmitSelection(string selection)
        {
            if (IsBracketTrickSelected)
            {
                UsedBracketTricks.Add(X + "," + Y);
                MoveSelection();
            }
            MainWindow.ViewModel.Submit(selection);
        }

        /// <summary>
        /// Returns the current user selection, as a parsed string in which the newline characters have been removed.
        /// </summary>
        private string GetSelection()
        {
            string selection = "";
            if (ActiveSide == Side.Left)
            {
                selection = MainWindow.LeftPasswordColumn.Selection.Text;
            }
            else if (ActiveSide == Side.Right)
            {
                selection = MainWindow.RightPasswordColumn.Selection.Text;
            }
            selection = StringUtilities.RemoveNewlines(selection);
            return selection;
        }

        /// <summary>
        /// Notifies the viewModel that the current user selection is changed, so that the view model can deal with it as it wishes.
        /// 
        /// Also, we might as well play the delightful clacky keyboard sounds while here.
        /// (The sounds are played on a Task.Delay() timer, which is why this method is async.)
        /// </summary>
        async private void NotifySelectionChanged()
        {
            string newSelection = GetSelection();
            for (int i = 0; i < newSelection.Length; i++)
            {
                await Task.Delay(30);
                SoundPlayer.PlayTypingSound();
            }
            MainWindow.ViewModel.SelectionChanged(newSelection);
        }

        /// <summary>
        /// Updates the class field reprsenting the side of the input we are on.
        /// E.g., which of the 2 columns of memory dump is currently selected.
        /// 
        /// The word column as used here should not be confused with the way it is used in 
        /// TerminalModel.NUMBER_OF_COLUMNS. In that case, we are reffering to the columns within each of
        /// the two memory dump columns.
        /// </summary>
        private void UpdateActiveSide()
        {
            ActiveSide = (X < TerminalModel.NUMBER_OF_COLUMNS) ? Side.Left : Side.Right;
        }

        /// <summary>
        /// Returns true if a bracket trick at the provided coordinate has already been used.
        /// </summary>
        /// <param name="x">The x coordinate of the bracket trick.</param>
        /// <param name="y">The y coordinate of the bracket trick.</param>
        private bool IsBracketTrickUsed(int x, int y)
        {
            if (UsedBracketTricks.Contains(x + "," + y))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Called when memory dump contents are changed.
        /// This method is only needed to fix that nasty selection bug which causes the entire 
        /// section to be highlighted upon any changes. It normally would be bad practice to place it here,
        /// and some refactoring could probably find a better way to do this, but I haven't the time.
        /// </summary>
        private void OnMemoryDumpContentsChanged(object sender, EventArgs args)
        {
            MoveSelection();
        }
    }
}
