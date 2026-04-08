namespace Lab_1
{
    internal class Row
    {
        int ind;
        ElRow[] formula;
        double value;

        public Row(int index, ElRow[] formula)
        {
            this.ind = index;
            this.formula = formula;
            this.value = double.MinValue;
        }

        public int GetIndex()
        {
            return ind;
        }
        public ElRow[] GetFormula()
        {
            return formula;
        }
        public void ChangeValue(double val)
        {
            value = val;
        }
        public double GetValue()
        {
            return value;
        }
    }
}
