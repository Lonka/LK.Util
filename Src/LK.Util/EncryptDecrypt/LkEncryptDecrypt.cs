using System;

namespace LK.Util
{
    /// <summary>
    /// IV(or IVStr) 和 Key(or KeyStr)一定要設定
    /// </summary>
    public class LkEncryptDecrypt
    {
        private static string ms_encStart = "*";


        private static byte[] _iv = null;
        public static byte[] IV
        {
            get
            {
                if (_iv == null && !string.IsNullOrEmpty(IVStr) && IVStr.Length >= 8)
                {
                    byte[] temp = System.Text.Encoding.ASCII.GetBytes(IVStr);
                    _iv = new byte[8];
                    Array.Copy(temp, _iv, 8);
                }
                return _iv;
            }
            set
            {
                _iv = value;
            }
        }

        private static byte[] _key = null;
        public static byte[] Key
        {
            get
            {
                if (_key == null && !string.IsNullOrEmpty(KeyStr) && KeyStr.Length >= 8)
                {
                    byte[] temp = System.Text.Encoding.ASCII.GetBytes(KeyStr);
                    _key = new byte[8];
                    Array.Copy(temp, _key, 8);
                }
                return _key;
            }
            set
            {
                _key = value;
            }
        }

        private static string _ivStr = "12345678";
        public static string IVStr
        {
            get
            {
                return _ivStr;
            }
            set
            {
                _ivStr = value;
            }
        }
        private static string _keyStr = "abcdefgh";
        public static string KeyStr
        {
            get
            {
                return _keyStr;
            }
            set
            {
                _keyStr = value;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Decrypt(string value, out string result)
        {
            bool bResult = false;
            result = string.Empty;
            if (IV == null || Key == null)
            {
                return bResult;
            }

            try
            {
                //4是因為*TovzlNN3FHebHgB9J84vmQ==3163中的3163
                int encLength = ms_encStart.Length + 4;

                if (value.StartsWith(ms_encStart) && value.Length > encLength)
                {
                    string inputStr = value.Substring(ms_encStart.Length, value.Length - encLength);
                    result = DoDecrypt(inputStr);
                    bResult = true;
                }
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return bResult;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Encrypt(string value, out string result)
        {
            bool bResult = false;
            result = string.Empty;
            if (IV == null || Key == null)
            {
                return bResult;
            }
            try
            {
                string encEnd = new Random().Next(1000, 9999).ToString();
                result = ms_encStart + DoEncrypt(value) + encEnd;
                bResult = true;
            }
            catch (Exception e)
            {
                LkLog.WriteLog(LogLevel.Error, e);
            }
            return bResult;
        }

        /// <summary>
        /// 是否有加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsEncrypt(string source)
        {
            bool result = false;
            int encEnd = -1;
            if (source.StartsWith(ms_encStart) && int.TryParse(source.Substring(source.Length - 4, 4), out encEnd))
            {
                result = true;
            }
            return result;
        }

        private static string DoDecrypt(string source)
        {
            string result;
            byte[] iV = new Byte[8];
            Array.Copy(IV, iV, 8);
            System.Security.Cryptography.DES des = System.Security.Cryptography.DESCryptoServiceProvider.Create();
            System.Security.Cryptography.ICryptoTransform decryptor = des.CreateDecryptor(Key, iV);
            byte[] decodeSrc = System.Convert.FromBase64String(source);
            byte[] decode = decryptor.TransformFinalBlock(decodeSrc, 0, decodeSrc.Length);
            result = System.Text.Encoding.Default.GetString(decode, 0, decode.Length);
            return result;
        }

        private static string DoEncrypt(string source)
        {
            string result;
            byte[] iV = new Byte[8];
            Array.Copy(IV, iV, 8);
            System.Security.Cryptography.DES des = System.Security.Cryptography.DESCryptoServiceProvider.Create();
            byte[] encodeStr = System.Text.Encoding.Default.GetBytes(source);
            System.Security.Cryptography.ICryptoTransform cryptor = des.CreateEncryptor(Key, iV);
            byte[] encode = cryptor.TransformFinalBlock(encodeStr, 0, encodeStr.Length);
            result = System.Convert.ToBase64String(encode);
            return result;
        }
    }
}