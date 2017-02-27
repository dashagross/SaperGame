using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaperLibrary
{
    public class CellChangedEventArgs : EventArgs
    {
        public int x;
        public int y;

        public CellChangedEventArgs(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
