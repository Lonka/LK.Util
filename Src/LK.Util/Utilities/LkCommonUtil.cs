using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace LK.Util
{
    public static class LkCommonUtil
    {
        public static string GetFilePath(string fileName)
        {
            string path = fileName;
            if (fileName.StartsWith(@".\"))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName.Replace(@".\", string.Empty));
            }
            else if (fileName.StartsWith(@"\") || !fileName.Contains(@":\"))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            }
            return path;
        }

        public static string TrimText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            return text.Trim().Replace("\r\n", string.Empty);
        }

        public static double ReverseValue(double originalValue, double? offset, double? multiplier, double? exponent)
        {
            double result = originalValue;
            if (exponent != null)
            {
                result /= Math.Pow(10, exponent.Value);
            }
            if (multiplier != null)
            {
                result /= multiplier.Value;
            }
            if (offset != null)
            {
                result -= offset.Value;
            }
            return result;
        }

        public static double ProcessValue(double originalValue, double? offset, double? multiplier, double? exponent)
        {
            double result = originalValue;
            if (offset != null)
            {
                result += offset.Value;
            }
            if (multiplier != null)
            {
                result *= multiplier.Value;
            }

            if (exponent != null)
            {
                result *= Math.Pow(10, exponent.Value);
            }
            return result;
        }


        private const string m_nullPartten = "<NULL>";

        /// <summary>
        /// 將Hashtable中的項目經過關鍵字轉換後重組為query string
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static string ConvertQueryStringFromHash(Hashtable hash)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in hash.Keys)
            {
                if (builder.Length > 0)
                    builder.Append("&");
                if (hash[key] == null)
                    builder.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(m_nullPartten));
                else
                    builder.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(hash[key].ToString()));
            }
            return (builder.ToString());
        }


        /// <summary>
        /// 將 query string 轉為　Hashtable　的格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Hashtable ConvertQueryStringToHash(string input)
        {
            Hashtable hash = new Hashtable();
            CombineQueryStringToHash(ref hash, input);
            return (hash);
        }

        /// <summary>
        /// 傳入URL 中的 Query String，解析並以後蓋前的方式加入原有的Hashtable
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="input"></param>
        public static void CombineQueryStringToHash(ref Hashtable hash, string input)
        {
            string[] items = input.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < items.Length; i++)
            {
                string[] nameValue = items[i].Split("=".ToCharArray(), 2);
                string name = "";
                string value = "";

                if (nameValue.Length > 0)
                {
                    name = HttpUtility.UrlDecode(nameValue[0]);
                    if (nameValue.Length > 1)
                    {
                        value = HttpUtility.UrlDecode(nameValue[1]);
                    }
                    if (hash.ContainsKey(name))
                    {
                        if (value == m_nullPartten)
                            hash[name] = null;
                        else
                            hash[name] = value;
                    }
                    else
                    {
                        if (value == m_nullPartten)
                            hash.Add(name, null);
                        else
                            hash.Add(name, value);
                    }
                }
            }
        }
    }
}
