using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LK.Util
{
    /// <summary>
    /// 使用Regex驗證(TryParse的performance比Regex好)
    /// </summary>
    public class LkValidation
    {
        /// <summary>
        /// 判斷字串是否與指定正則運算式匹配
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <param name="regularExp">正則運算式</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsMatch(string input, string regularExp)
        {
            return Regex.IsMatch(input, regularExp);
        }

        /// <summary>
        /// 驗證非負整數（正整數 + 0）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUnMinusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnMinusInteger);
        }

        /// <summary>
        /// 驗證正整數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsPlusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.PlusInteger);
        }

        /// <summary>
        /// 驗證非正整數（負整數 + 0） 
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUnPlusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnPlusInteger);
        }

        /// <summary>
        /// 驗證負整數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsMinusInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.MinusInteger);
        }

        /// <summary>
        /// 驗證整數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsInt(string input)
        {
            return Regex.IsMatch(input, RegularExp.Integer);
        }

        /// <summary>
        /// 驗證非負浮點數（正浮點數 + 0）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUnMinusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnMinusFloat);
        }

        /// <summary>
        /// 驗證正浮點數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsPlusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.PlusFloat);
        }

        /// <summary>
        /// 驗證非正浮點數（負浮點數 + 0）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUnPlusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.UnPlusFloat);
        }

        /// <summary>
        /// 驗證負浮點數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsMinusFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.MinusFloat);
        }

        /// <summary>
        /// 驗證浮點數
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsFloat(string input)
        {
            return Regex.IsMatch(input, RegularExp.Float);
        }

        /// <summary>
        /// 驗證由26個英文字母組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.Letter);
        }

        /// <summary>
        /// 驗證由中文組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsChinese(string input)
        {
            return Regex.IsMatch(input, RegularExp.Chinese);
        }

        /// <summary>
        /// 驗證由26個英文字母的大寫組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUpperLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.UpperLetter);
        }

        /// <summary>
        /// 驗證由26個英文字母的小寫組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsLowerLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.LowerLetter);
        }

        /// <summary>
        /// 驗證由數位和26個英文字母組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumericOrLetter(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumericOrLetter);
        }

        /// <summary>
        /// 驗證由數位組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumeric(string input)
        {
            return Regex.IsMatch(input, RegularExp.Numeric);
        }
        /// <summary>
        /// 驗證由數位和26個英文字母或中文組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumericOrLetterOrChinese(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumbericOrLetterOrChinese);
        }

        /// <summary>
        /// 驗證由數位、26個英文字母或者下劃線組成的字串
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsNumericOrLetterOrUnderline(string input)
        {
            return Regex.IsMatch(input, RegularExp.NumericOrLetterOrUnderline);
        }

        /// <summary>
        /// 驗證email地址
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsEmail(string input)
        {
            return Regex.IsMatch(input, RegularExp.Email);
        }

        /// <summary>
        /// 驗證URL
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsUrl(string input)
        {
            return Regex.IsMatch(input, RegularExp.Url);
        }

        /// <summary>
        /// 驗證電話號碼
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsTelephone(string input)
        {
            return Regex.IsMatch(input, RegularExp.Telephone);
        }

        /// <summary>
        /// 驗證手機號碼
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsMobile(string input)
        {
            return Regex.IsMatch(input, RegularExp.Mobile);
        }

        /// <summary>
        /// 通過檔副檔名驗證圖像格式
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsImageFormat(string input)
        {
            return Regex.IsMatch(input, RegularExp.ImageFormat);
        }

        /// <summary>
        /// 驗證IP
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsIP(string input)
        {
            return Regex.IsMatch(input, RegularExp.IP);
        }

        /// <summary>
        /// 驗證日期（YYYY-MM-DD）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsDate(string input)
        {
            return Regex.IsMatch(input, RegularExp.Date);
        }

        /// <summary>
        /// 驗證日期和時間（YYYY-MM-DD HH:MM:SS）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsDateTime(string input)
        {
            return Regex.IsMatch(input, RegularExp.DateTime);
        }

        /// <summary>
        /// 驗證顏色（#ff0000）
        /// </summary>
        /// <param name="input">要驗證的字串</param>
        /// <returns>驗證通過返回true</returns>
        public static bool IsColor(string input)
        {
            return Regex.IsMatch(input, RegularExp.Color);
        }
    }
}
