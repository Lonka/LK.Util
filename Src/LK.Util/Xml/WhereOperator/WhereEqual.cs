using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util.Xml
{
    class WhereEqual :IOperator
    {
        public bool IsCompare(string key, string value)
        {
            if (key.Equals(value))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
