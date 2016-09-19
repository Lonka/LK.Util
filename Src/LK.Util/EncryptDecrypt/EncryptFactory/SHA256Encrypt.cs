using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LK.Util
{
    internal class SHA256Encrypt : IEncrypt
    {
        public string GetSecurePassword(string password)
        {
            string result = string.Empty;
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] source = Encoding.Default.GetBytes(password);
            byte[] hash = sha256.ComputeHash(source);
            result = Convert.ToBase64String(hash);
            return result;
        }

        public string GetSecurePassword(string password, string salt)
        {
            return GetSecurePassword(password + salt);
        }
    }
}
