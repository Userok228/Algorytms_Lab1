namespace Lab_1
{
    internal class DelRow
    {
        int ind;
        DelElRow[] formula;
        double value;

        public DelRow(int index, DelElRow[] formula)
        {
            this.ind = index;
            this.formula = formula;
            this.value = double.MinValue;
        }

        public int GetIndex()
        {
            return ind;
        }
        public DelElRow[] GetFormula()
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
