using Fallout_Terminal.ViewModel;
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
    public class SelectionManager
    {
        private MainWindow MainWindow;
        private int y;
        private int x;
        private TextPointer LeftStart;
        private TextPointer LeftEnd;
        private TextPointer RightStart;
        private TextPointer RightEnd;
        private enum Side
        {
            Left,
            Right
        }

        /// <summary>
        /// Creates a standard instance of the SelectionManager class.
        /// </summary>
        public SelectionManager(MainWindow window)
        {
            MainWindow = window;
            y = 0;
            x = 0;
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
                    // TODO: Implement selection submission.
                    break;
                default:
                    break;
            }
            Console.WriteLine(y + " " + x);
        }

        private void MoveSelectionLeft()
        {
            if(x > 0)
            {
                x--;
                MoveSelection();
            }
        }

        private void MoveSelectionRight()
        {
            if(x < (TerminalViewModel.NUMBER_OF_COLUMNS * 2) - 1)
            {
                x++;
                MoveSelection();
            }
        }

        private void MoveSelectionUp()
        {
            if (y > 0)
            {
                y--;
                MoveSelection();
            }
        }

        private void MoveSelectionDown()
        {
            if (y < TerminalViewModel.NUMBER_OF_LINES - 1)
            {
                y++;
                MoveSelection();
            }
        }

        private void MoveSelection()
        {
            if(x < TerminalViewModel.NUMBER_OF_COLUMNS)
            {
                MainWindow.LeftPasswordColumn.Focus();
                LeftStart = MainWindow.LeftPasswordColumn.Document.ContentStart;
                LeftEnd = LeftStart.GetPositionAtOffset(1);
                int offset;
                offset = 2 + x + (y * (TerminalViewModel.NUMBER_OF_COLUMNS + 2));
                LeftStart = LeftStart.GetPositionAtOffset(offset);
                LeftEnd = LeftStart.GetPositionAtOffset(1);
                MainWindow.LeftPasswordColumn.Selection.Select(LeftStart, LeftEnd);
            }
            else if (x >= TerminalViewModel.NUMBER_OF_COLUMNS)
            {
                MainWindow.RightPasswordColumn.Focus();
                // Reset the position on the TextPointers to the beginning. It's much easier this way.
                RightStart = MainWindow.RightPasswordColumn.Document.ContentStart;
                RightEnd = RightStart.GetPositionAtOffset(1);
                int offset;
                offset = (x - TerminalViewModel.NUMBER_OF_COLUMNS + 2) + (y * (TerminalViewModel.NUMBER_OF_COLUMNS + 2));
                RightStart = RightStart.GetPositionAtOffset(offset);
                RightEnd = RightStart.GetPositionAtOffset(1);
                MainWindow.RightPasswordColumn.Selection.Select(RightStart, RightEnd);
            }
        }

        private void ResizeSelectionForWords(Side column)
        {
            
        }
    }
}
