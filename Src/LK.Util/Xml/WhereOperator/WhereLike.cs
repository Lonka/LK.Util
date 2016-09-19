using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util.Xml
{
    class WhereLike : IOperator
    {
        public bool IsCompare(string key, string value)
        {
            if (key.IndexOf(value) > -1)
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
