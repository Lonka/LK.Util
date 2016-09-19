using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util
{
    interface IEncrypt
    {
        string GetSecurePassword(string password);

        string GetSecurePassword(string password, string salt);
    }
}
