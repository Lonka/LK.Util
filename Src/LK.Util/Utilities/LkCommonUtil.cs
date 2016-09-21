using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
    }
}
