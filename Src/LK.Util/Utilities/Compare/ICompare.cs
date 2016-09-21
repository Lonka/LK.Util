using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util
{
    internal interface ICompare
    {
        bool Equal();
        bool GreaterThen();
        bool GreaterThenAndEqual();
        bool LessThen();
        bool LessThenAndEqual();
    }
}
