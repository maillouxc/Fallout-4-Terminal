using Fallout_Terminal.Model;
using Fallout_Terminal.Utilities;
using Fallout_Terminal.ViewModel;
using Fallout_Terminal.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
        /// Creates a standard instance of the SelectionManager class.
        /// </summary>
        public SelectionManager(MainWindow window)
        {
            MainWindow = window;
            Y = 0;
            X = 0;
            ActiveSide = Side.Left;
            MainWindow.ViewModel.MemoryDumpContentsChanged += OnMemoryDumpContentsChanged;
            // TODO: Use event to fix so that first character starts out visibly selected upon each start.
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
        /// and adjustment of the textPointer positions.
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
                    return;
            }
            string textAfter;
            if (ActiveSide == Side.Left)
            {
                textAfter = LeftEnd.GetTextInRun(LogicalDirection.Forward);
            }
            else // Assuming it's not null...
            {
                textAfter = RightEnd.GetTextInRun(LogicalDirection.Forward);
            }
            int offset = 0;
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
                    else if (ActiveSide == Side.Right)
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
        /// </summary>
        /// <param name="column">The column (left or right) that the selection is in.</param>
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
                    X -= TerminalModel.NUMBER_OF_COLUMNS + 1; // The -1 compensates for the X++ in MoveSelectionRight()
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
            return 0;
        }

        /// <summary>
        /// Submits the current selection to the viewModel to be processed by the game logic.
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
        /// <returns></returns>
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
        /// Also, since we are already in the View, we might as well play the delightful clacky keyboard sounds while here.
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
        /// </summary>
        private void UpdateActiveSide()
        {
            ActiveSide = (X < TerminalModel.NUMBER_OF_COLUMNS) ? Side.Left : Side.Right;
        }

        /// <summary>
        /// Returns true if the bracket trick has already been used.
        /// </summary>
        /// <param name="x">The x coordinate of the proposed bracket trick.</param>
        /// <param name="y">The y coordinate of the proposed bracket trick.</param>
        /// <returns></returns>
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
        /// section to be highlighted upon any changes.
        /// </summary>
        private void OnMemoryDumpContentsChanged(object sender, EventArgs args)
        {
            MoveSelection();
        }
    }
}
