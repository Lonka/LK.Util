using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util
{
    public class LkCompareUtil
    {
        /// <summary>
        /// 兩個object比較
        /// </summary>
        /// <param name="compareType">比較的type</param>
        /// <param name="type">型態</param>
        /// <param name="obj1">value 1</param>
        /// <param name="obj2">value 2</param>
        /// <returns></returns>
        public static bool Compare(LkCompareType compareType, Type type, object obj1, object obj2)
        {
            ICompare compare = null;
            bool result = false;
            if (type == typeof(double) || type == typeof(long) || type == typeof(int) || type == typeof(float))
            {
                compare = new CompareNumericalFactory(obj1, obj2);
            }
            else if (type == typeof(DateTime))
            {
                compare = new CompareDatetimeFactory(obj1, obj2);
            }

            if (compare != null)
            {
                switch (compareType)
                {
                    case LkCompareType.Equal:
                        result = compare.Equal();
                        break;
                    case LkCompareType.IsGreaterThen:
                        result = compare.GreaterThen();
                        break;
                    case LkCompareType.IsGreaterThenAndEqual:
                        result = compare.GreaterThenAndEqual();
                        break;
                    case LkCompareType.IsLessThen:
                        result = compare.LessThen();
                        break;
                    case LkCompareType.IsLessThenAndEqual:
                        result = compare.LessThenAndEqual();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
