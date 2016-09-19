using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util
{
    public class LkEncrypt
    {
        public static string GetSecurePassword(LkEncryptType type,string value)
        {
            IEncrypt encrypt = GetEncryptFactory(type);
            if (encrypt == null)
            {
                return string.Empty;
            }
            else
            {
                return encrypt.GetSecurePassword(value);
            }
        }

        public static string GetSecurePassword(LkEncryptType type, string value, string salt)
        {
            IEncrypt encrypt = GetEncryptFactory(type);
            return encrypt.GetSecurePassword(value, salt);
        }


        private static IEncrypt GetEncryptFactory(LkEncryptType type)
        {
            IEncrypt encrypt = null;
            switch (type)
            {
                case LkEncryptType.MD5:
                    encrypt = new MD5Encrypt();
                    break;
                case LkEncryptType.SHA1:
                    encrypt = new SHA1Encrypt();
                    break;
                case LkEncryptType.SHA256:
                    encrypt = new SHA256Encrypt();
                    break;
                case LkEncryptType.SHA384:
                    encrypt = new SHA384Encrypt();
                    break;
                case LkEncryptType.SHA512:
                    encrypt = new SHA512Encrypt();
                    break;
                case LkEncryptType.BCrypt:
                    break;
                default:
                    break;
            }
            return encrypt;
        }


    }
}
