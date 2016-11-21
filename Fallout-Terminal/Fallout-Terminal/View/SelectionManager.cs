using Fallout_Terminal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Fallout_Terminal.Utilities;

namespace Fallout_Terminal.View
{
    public class SelectionManager
    {
        private MainWindow MainWindow;
        private TextPointer LeftStart;
        private TextPointer LeftEnd;
        private TextPointer RightStart;
        private TextPointer RightEnd;
        private int Y;
        private int X;
        private int Offset;
        private int LeftJumpOffset;
        private int RightJumpOffset;
        private enum Side
        {
            Left,
            Right
        }
        private bool IsWordCurrentlySelected = false;

        /// <summary>
        /// Creates a standard instance of the SelectionManager class.
        /// </summary>
        public SelectionManager(MainWindow window)
        {
            MainWindow = window;
            Y = 0;
            X = 0;
        }

        /// <summary>
        /// Handler for keyPress Events while the program is running.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        internal void OnKeyDown(object sender, KeyEventArgs args)
        {
            Console.WriteLine(args.Key.ToString());
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
                    SubmitSelection();
                    break;
            }
        }

        private void MoveSelectionLeft()
        {
            if(X > 0)
            {
                if (IsWordCurrentlySelected)
                {
                    JumpToLeftEndOfSelectedWord();
                }
                X--;
                MoveSelection();
            }
        }

        private void MoveSelectionRight()
        {
            if(X < (TerminalViewModel.NUMBER_OF_COLUMNS * 2) - 1)
            {
                if (IsWordCurrentlySelected)
                {
                    JumpToRightEndOfSelectedWord();
                }
                X++;
                MoveSelection();
            }
        }

        private void MoveSelectionUp()
        {
            if (Y > 0)
            {
                Y--;
                MoveSelection();
            }
        }

        private void MoveSelectionDown()
        {
            if (Y < TerminalViewModel.NUMBER_OF_LINES - 1)
            {
                Y++;
                MoveSelection();
            }
        }

        private void MoveSelection()
        {
            if(X < TerminalViewModel.NUMBER_OF_COLUMNS)
            {
                MainWindow.LeftPasswordColumn.Focus();
                LeftStart = MainWindow.LeftPasswordColumn.Document.ContentStart;
                LeftEnd = LeftStart.GetPositionAtOffset(1);
                Offset = CalculateOffset(Side.Left);
                LeftStart = LeftStart.GetPositionAtOffset(Offset);
                LeftEnd = LeftStart.GetPositionAtOffset(1);
                MainWindow.LeftPasswordColumn.Selection.Select(LeftStart, LeftEnd);
                ResizeSelectionForWords(Side.Left);
            }
            else if (X >= TerminalViewModel.NUMBER_OF_COLUMNS)
            {
                MainWindow.RightPasswordColumn.Focus();
                RightStart = MainWindow.RightPasswordColumn.Document.ContentStart;
                RightEnd = RightStart.GetPositionAtOffset(1);
                Offset = CalculateOffset(Side.Right);
                RightStart = RightStart.GetPositionAtOffset(Offset);
                RightEnd = RightStart.GetPositionAtOffset(1);
                MainWindow.RightPasswordColumn.Selection.Select(RightStart, RightEnd);
                ResizeSelectionForWords(Side.Right);
            }
        }

        private void ResizeSelectionForWords(Side column)
        {
            bool finished = false;
            LeftJumpOffset = 0;
            RightJumpOffset = 0;
            while (!finished)
            {
                if (column == Side.Left)
                {
                    // We only need to check the adjacent characters if the current character is a letter.
                    if (IsLetterChecker.IsLetter(MainWindow.LeftPasswordColumn.Selection.Text[0])
                            || MainWindow.LeftPasswordColumn.Selection.Text[0] == '\u000D'
                            || MainWindow.LeftPasswordColumn.Selection.Text[0] == '\u000A')
                    {
                        char charBefore = LeftStart.GetTextInRun(LogicalDirection.Backward).LastOrDefault();
                        char charAfter = LeftEnd.GetTextInRun(LogicalDirection.Forward).FirstOrDefault();
                        Console.WriteLine("charBefore == " + charBefore + "... charAfter == " + charAfter);
                        if (IsLetterChecker.IsLetter(charBefore) || charBefore == '\u000D' || charBefore == '\u000A')
                        {
                            LeftStart = LeftStart.GetPositionAtOffset(-1);
                            LeftJumpOffset++;
                        }
                        else if (IsLetterChecker.IsLetter(charAfter) || charAfter == '\u000D' || charAfter == '\u000A')
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
                else if (column == Side.Right)
                {
                    if (IsLetterChecker.IsLetter(MainWindow.RightPasswordColumn.Selection.Text[0])
                            || MainWindow.RightPasswordColumn.Selection.Text[0] == '\u000D'
                            || MainWindow.RightPasswordColumn.Selection.Text[0] == '\u000A')
                    {
                        char charBefore = RightStart.GetTextInRun(LogicalDirection.Backward).LastOrDefault();
                        char charAfter = RightEnd.GetTextInRun(LogicalDirection.Forward).FirstOrDefault();
                        Console.WriteLine("charBefore == " + charBefore + "... charAfter == " + charAfter);
                        if (IsLetterChecker.IsLetter(charBefore) || charBefore == '\u000D' || charBefore == '\u000A')
                        {
                            RightStart = RightStart.GetPositionAtOffset(-1);
                            LeftJumpOffset++;
                        }
                        else if (IsLetterChecker.IsLetter(charAfter) || charAfter == '\u000D' || charAfter == '\u000A')
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

        private void JumpToLeftEndOfSelectedWord()
        {
            X -= LeftJumpOffset;
        }

        private void JumpToRightEndOfSelectedWord()
        {
            X += RightJumpOffset;
        }

        private int CalculateOffset(Side column)
        {
            if (column == Side.Left)
            {
                return (2 + X) + (Y * (TerminalViewModel.NUMBER_OF_COLUMNS + 1));
            }
            else if (column == Side.Right)
            {
                return (2 + X - TerminalViewModel.NUMBER_OF_COLUMNS) + (Y * (TerminalViewModel.NUMBER_OF_COLUMNS + 1));
            }
            else
            {
                return 0;
            }
        }

        private void SubmitSelection()
        {

        }
    }
}
