using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace LK.Util
{
   internal  class MD5Encrypt :IEncrypt
    {
        public string GetSecurePassword(string password)
        {
            string result = string.Empty;
            MD5 md5 = MD5.Create();
            byte[] source = Encoding.Default.GetBytes(password);
            byte[] hash = md5.ComputeHash(source);
            result = Convert.ToBase64String(hash);
            return result;
        }


        public string GetSecurePassword(string password, string salt)
        {
            return GetSecurePassword(password + salt);
        }
    }
}
