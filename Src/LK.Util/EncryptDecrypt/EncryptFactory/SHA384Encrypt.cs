using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace LK.Util
{
    internal class SHA384Encrypt : IEncrypt
    {
        public string GetSecurePassword(string password)
        {
            string result = string.Empty;
            SHA384 sha384 = new SHA384CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(password);
            byte[] hash = sha384.ComputeHash(source);
            result = Convert.ToBase64String(hash);
            return result;
        }

        public string GetSecurePassword(string password, string salt)
        {
            return GetSecurePassword(password + salt);
        }
    }
}
