using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LK.Util.Utilities
{
    public static class LkObjectUtil
    {
        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            Type type = obj.GetType();
            T result = default(T);
            try
            {
                result = (T)obj.GetType().InvokeMember(propertyName, System.Reflection.BindingFlags.GetProperty, null, obj, new object[] { });
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        private static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);

            return null;
        }

    }
}
