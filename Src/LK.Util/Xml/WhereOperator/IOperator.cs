using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util.Xml
{
    interface IOperator
    {
        /// <summary>
        /// 比較兩數
        /// </summary>
        /// <param name="key">來源</param>
        /// <param name="value">目標</param>
        /// <returns></returns>
        bool IsCompare(string key, string value);
    }
}
