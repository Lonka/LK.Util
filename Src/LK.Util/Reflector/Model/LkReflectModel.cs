using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LK.Util
{
    /// <summary>
    /// Reflection的Instance
    /// </summary>
    public class LkReflectModel
    {
        public Assembly Assembly { get; set; }
        public Type Class { get; set; }

        public object ClassInstance { get; set; }
    }
}
