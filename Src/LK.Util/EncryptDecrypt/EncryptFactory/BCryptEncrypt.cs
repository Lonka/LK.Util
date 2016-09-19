using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCrypt.Net;

namespace Security.Encryption
{
    internal class BCryptEncrypt : IEncrypt
    {
        private int mWorkFactor = 1;
        public BCryptEncrypt(int workFactor)
        {
            mWorkFactor = workFactor;
        }
        public string GetSecurePassword(string password)
        {
            string result = string.Empty;
            result = BCrypt.Net.BCrypt.HashPassword(password, mWorkFactor);
            return result;
        }

        public string GetSecurePassword(string password, string salt)
        {
            return GetSecurePassword(password + salt);
        }
    }
}
