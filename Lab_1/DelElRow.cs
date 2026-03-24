using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    internal class DelElRow
    {
        double mod;
        char XY;
        int index;
        public DelElRow(double mod, char XY, int index)
        {
            this.mod = mod;
            this.XY = XY;
            this.index = index;
        }

        public double GetMod()
        {
            return mod;
        }
        public char GetXY()
        {
            return XY;
        }
        public int GetIndex()
        {
            return index;
        }
    }
}
