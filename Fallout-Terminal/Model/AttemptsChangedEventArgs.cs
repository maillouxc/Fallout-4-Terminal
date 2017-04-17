using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fallout_Terminal.Model
{
    public class AttemptsChangedEventArgs : EventArgs
    {
        public int AttemptsRemaining { get; private set; }

        public AttemptsChangedEventArgs(int attemptsRemaining)
        {
            AttemptsRemaining = attemptsRemaining;
        }
    }
}
