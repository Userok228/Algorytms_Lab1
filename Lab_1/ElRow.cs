namespace Lab_1
{
    internal class ElRow
    {
        double mod;
        char XY;
        int index;
        public ElRow(double mod, char XY, int index)
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
