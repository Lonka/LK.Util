using System;
using System.Data;
using System.Reflection;

namespace LK.Util
{
    public class LkMapper
    {
        public static T CreateClassFromRow<T>(DataRow dr) where T : new()
        {
            T item = new T();
            SetItemFromRow(item, dr);
            return item;
        }

        public static void SetItemFromRow<T>(T item, DataRow dr) where T : new()
        {
            foreach (DataColumn col in dr.Table.Columns)
            {
                PropertyInfo propertyInfo = item.GetType().GetProperty(col.ColumnName);
                if (propertyInfo != null && dr[col] != DBNull.Value)
                {

                    if( propertyInfo.PropertyType == typeof(int))
                    {
                        int intValue;
                        if(!int.TryParse(dr[col].ToString(),out intValue))
                        {
                            intValue = int.MaxValue;
                        }
                        propertyInfo.SetValue(item, intValue, null);
                    }
                    else if (propertyInfo.PropertyType == typeof(double))
                    {
                        double doubleValue;
                        if (!double.TryParse(dr[col].ToString(), out doubleValue))
                        {
                            doubleValue = int.MaxValue;
                        }
                        propertyInfo.SetValue(item, doubleValue, null);

                    }
                    else
                    {
                        propertyInfo.SetValue(item, dr[col], null);
                    }
                }
            }
        }
    }
}