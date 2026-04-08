using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_1
{
    public class Variable
    {
        public enum VariableType
        {
            Zero = 0,
            X,
            Y,
            S,
            V, 
            U
        }

        public int index;
        public VariableType type;

        public Variable(int index, char type)
        {
            this.index = index;
            switch (type)
            {
                case 'X':
                    this.type = VariableType.X;
                    break;
                case 'Y':
                    this.type = VariableType.Y;
                    break;
                case 'S':
                    this.type = VariableType.S;
                    break;
                case 'V':
                    this.type = VariableType.V;
                    break;
                case 'U':
                    this.type = VariableType.U;
                    break;
                case ('0'):
                    this.type = VariableType.Zero;
                    break;
                default:
                    throw new Exception("Wrong variable type");
            }
        }

    }
}
