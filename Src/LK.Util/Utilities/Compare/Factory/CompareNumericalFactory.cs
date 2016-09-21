using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util
{
    internal class CompareNumericalFactory : ICompare
    {
        private double value1;
        private double value2;
        public CompareNumericalFactory(object obj1,object obj2)
        {
            double temp;
            if (double.TryParse(obj1.ToString(), out temp))
            {
                value1 = temp;
            }
            if (double.TryParse(obj2.ToString(), out temp))
            {
                value2 = temp;
            }
        }

        public bool Equal()
        {
            return value1.Equals(value2);
        }


        public bool GreaterThen()
        {
            return value1 > value2;
        }

        public bool GreaterThenAndEqual()
        {
            return value1 >= value2;
        }

        public bool LessThen()
        {
            return value1 < value2;
        }

        public bool LessThenAndEqual()
        {
            return value1 <= value2;
        }
    }
}
