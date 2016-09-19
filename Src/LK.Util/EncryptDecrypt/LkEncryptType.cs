using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util
{
    public enum LkEncryptType
    {
        MD5,
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        /// <summary>
        /// 要nuget BCrypt.Net，暫不加入
        /// </summary>
        BCrypt
    }
}
