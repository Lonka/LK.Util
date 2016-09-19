using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util
{
    public class LkParamInfo : IEquatable<LkParamInfo>
    {
        public string Name { get; set; }
        public string StrValue { get; set; }

        public double? DoubleValue { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as LkParamInfo;
            if (other == null)
            {
                return false;
            }
            return Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }


        public bool Equals(LkParamInfo other)
        {
            if (other == null)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            bool result = true;
            if (!this.Name.EqualString(other.Name)
                || !this.StrValue.EqualString(other.StrValue)
                )
            {
                result = false;
            }
            return result;

        }
    }
}
