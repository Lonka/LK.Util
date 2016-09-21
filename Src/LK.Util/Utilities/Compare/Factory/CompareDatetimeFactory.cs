using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util
{
    internal class CompareDatetimeFactory : ICompare
    {
        private DateTime value1;
        private DateTime value2;

        public CompareDatetimeFactory(object obj1, object obj2)
        {
            value1 = (DateTime)obj1;
            value2 = (DateTime)obj2;
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
