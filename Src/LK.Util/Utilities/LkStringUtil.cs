using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace LK.Util
{
    /// <summary>
    /// String operation(Static)
    /// </summary>
    public static class LkStringUtil
    {
        /// <summary>
        /// 傳回字串陣列,利用字串來切割
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator">切割器(字串)</param>
        /// <returns></returns>
        public static string[] Split(this string str, string separator)
        {
            string[] newLine = new string[] { separator };
            return str.Split(newLine, StringSplitOptions.RemoveEmptyEntries);
        }


        /// <summary>
        ///  斷行
        /// </summary>
        /// <param name="sourceString">原始字串</param>
        /// <param name="byteLength">所需"字元"長度(BYTES數)</param>
        /// <param name="mark">每行分隔符號</param>
        /// <returns></returns>
        public static string BreakLine(this string sourceString, int byteLength, string mark)
        {
            string result = string.Empty;

            List<int> characterBytes = new List<int>();

            char[] characters = sourceString.Trim().ToCharArray();

            for (int i = 0; i < characters.Length; i++)
            {
                characterBytes.Add(System.Text.Encoding.Default.GetByteCount(characters[i].ToString()));
            }

            int tempByteLength = 0;
            int end = 0;
            int start = 0;

            for (int i = 0; i < characterBytes.Count; i++)
            {
                if (i + 1 != characterBytes.Count)
                {
                    if ((tempByteLength + characterBytes[i]) <= byteLength)
                    {
                        tempByteLength += characterBytes[i];
                        end = i;
                    }
                    else
                    {
                        result += sourceString.Substring(start, end - start + 1) + mark;
                        start = i;
                        i--;
                        tempByteLength = 0;
                    }
                }
                else
                {
                    tempByteLength += characterBytes[i];

                    if (tempByteLength > byteLength)
                    {
                        result += sourceString.Substring(start, characterBytes.Count - start - 1) + mark;
                        result += sourceString.Substring(i, 1);
                    }
                    else
                    {
                        result += sourceString.Substring(start, characterBytes.Count - start);
                    }
                }
            }
            return result;
        }


        public static string Substring(this string str, int startIndex, int length, bool useByte)
        {
            string result = string.Empty;
            if (useByte)
            {
                byte[] byteSource = System.Text.Encoding.Unicode.GetBytes(str);
                int byteLength = byteSource.Length - startIndex * 2;
                IEnumerable<int> byteRange = Enumerable.Range(startIndex * 2,
                     (
                         length * 2 > byteLength
                             ? byteLength
                             : length * 2
                     )
                     );
                //要補的byte數
                double alphanumeric = (byteRange.Count(i => byteSource[i].Equals(0)) * 0.5);

                double tempAlphanumeric = 0;
                int tempAlphanumericCount = 0;
                //往後找總共要補的個數
                for (int i = (startIndex * 2 + length * 2); i < byteSource.Length; i++)
                {
                    if (tempAlphanumeric.Equals(alphanumeric))
                    {
                        if (tempAlphanumericCount % 2 == 0)
                        {
                            alphanumeric = tempAlphanumericCount;
                        }
                        else
                        {
                            alphanumeric = tempAlphanumericCount + 1;
                        }
                        break;
                    }
                    if (byteSource[i] > 0)
                    {
                        tempAlphanumeric += 0.5;
                    }
                    tempAlphanumericCount++;
                }

                int totalCount = (length * 2) + (int)
                    (
                        alphanumeric % 2 == 0
                            ? alphanumeric
                            : alphanumeric + 1
                    );

                int realCount =
                    (
                        totalCount >= byteLength
                            ? byteLength
                            : totalCount
                    );
                result = System.Text.Encoding.Unicode.GetString(byteSource, startIndex * 2, realCount);
            }
            else
            {
                result = str.Substring(startIndex, length);
            }
            return result;
        }

        public static string ReplaceText(this string str, Dictionary<string, string> value)
        {
            string result = str;
            int startIndex = result.IndexOf("{");
            if (value != null && value.Count > 0 && startIndex > -1)
            {
                string keyStr = result.Substring(startIndex + 1);
                int endIndex = keyStr.IndexOf("}");
                if (endIndex > -1)
                {
                    keyStr = keyStr.Substring(0, endIndex);
                    string headStr = str.Substring(0, startIndex + endIndex + 2);
                    string endStr = str.Substring(startIndex + endIndex + 2);
                    if (value.ContainsKey(keyStr.ToUpper()))
                    {
                        headStr = headStr.Replace("{" + keyStr + "}", value[keyStr.ToUpper()]);
                    }
                    result = headStr + endStr.ReplaceText(value);
                }

            }
            return result;
        }

        public static bool EqualString(this string A, string B)
        {
            if (string.IsNullOrEmpty(A) && string.IsNullOrEmpty(B))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(A) && !string.IsNullOrEmpty(B))
            {
                return A.Equals(B);
            }

            if ((string.IsNullOrEmpty(A) && !string.IsNullOrEmpty(B))
                || (!string.IsNullOrEmpty(A) && string.IsNullOrEmpty(B))
                )
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// 依不同db及不同type來轉成query的field(to_date(...))
        /// </summary>
        /// <param name="source">欄位</param>
        /// <param name="value">值</param>
        /// <param name="type">型態</param>
        /// <param name="dbType">db</param>
        /// <returns></returns>
        public static string ParseDbField(this string source, string value, Type type, CommonType dbType)
        {
            string result = string.Empty;
            if (type == typeof(string))
            {
                if (value.ToUpper().Equals("NULL"))
                {
                    result = value + " as " + source;
                }
                else
                {
                    result = "'" + value + "' as " + source;
                }
            }
            else if (type == typeof(double))
            {
                result = value + " as " + source;
            }
            else if (type == typeof(DateTime))
            {
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                {
                    switch (dbType)
                    {
                        case CommonType.SqlServer:
                            result = "convert(datetime, '" + dt.ToString("yyyy/MM/dd HH:mm:ss") + "',20) as " + source;
                            break;
                        case CommonType.Oracle:
                            result = "TO_DATE('" + dt.ToString("yyyy/MM/dd HH:mm:ss") + "', 'YYYY/MM/DD HH24:MI:SS') as " + source;
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 依不同db加上query的參數格式(@param)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static string ParseDbParam(this string source, CommonType dbType)
        {
            string result = string.Empty;
            switch (dbType)
            {
                case CommonType.Sqlite:
                case CommonType.SqlServer:
                    result = "@" + source;
                    break;
                case CommonType.Oracle:
                    result = ":" + source;
                    break;
            }
            return result;
        }


        /// <summary>
        /// 轉換insert字串
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="insertCmds">insert list ex:[into T (A) values (1)]</param>
        /// <returns></returns>
        public static string ConvertInsertString(CommonType dbType, List<string> insertCmds)
        {
            string result = string.Empty;
            switch (dbType)
            {
                case CommonType.SqlServer:
                    result = " insert " + string.Join(" ; insert ", insertCmds.ToArray()) + ";";
                    break;
                case CommonType.Oracle:
                    result = " insert all " + string.Join(string.Empty, insertCmds.ToArray()) + " select * from dual ";
                    break;
            }
            return result;
        }

        /// <summary>
        /// 確認是否有寫上副檔名
        /// </summary>
        /// <param name="file">檔案</param>
        /// <param name="extansion">副檔名(ex:dll)</param>
        /// <returns></returns>
        public static string CheckExtansion(this string file, string extansion)
        {
            string result = string.Empty;
            if (file.ToUpper().LastIndexOf("." + extansion.ToUpper()) == 3)
            {
                result = file;
            }
            else if (file.ToUpper().LastIndexOf("." + extansion.ToUpper()) < 0)
            {
                result = file + "." + extansion;
            }
            return result;
        }



    }
}
