using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util.Xml
{
    class WhereLessThen : IOperator
    {
        public bool IsCompare(string key, string value)
        {
            double outKeyDouble = 0;
            double outValueDouble = 0;
            if (double.TryParse(key, out outKeyDouble) && double.TryParse(value, out outValueDouble))
            {
                if (outKeyDouble < outValueDouble)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
