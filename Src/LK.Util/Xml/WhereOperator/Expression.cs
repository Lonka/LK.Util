using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util.Xml
{
    class Expression
    {
        /// <summary>
        /// 建立運數子
        /// </summary>
        /// <param name="whereMode">運數子種類</param>
        /// <returns></returns>
        public static IOperator ConstructOperator(WhereMode whereMode)
        {
            switch (whereMode)
            {
                case WhereMode.Equal:
                    return new WhereEqual();
                case WhereMode.Like:
                    return new WhereLike();
                case WhereMode.IsGreaterThen:
                    return new WhereGreaterThen();
                case WhereMode.IsLessThen:
                    return new WhereLessThen();
                case WhereMode.IsGreaterThenAndEqual:
                    return new WhereGreaterThenEqual();
                case WhereMode.IsLessThenAndEqual:
                    return new WhereLessThenEqual();
            }
            return null;
        }
    }
}
