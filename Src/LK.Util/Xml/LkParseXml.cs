using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace LK.Util
{
    public class LkParseXml
    {
        #region BaseParse
        public static int? GetIntValue(string elementName, XElement element)
        {
            int? result = null;
            int intValue = 0;
            if (int.TryParse(GetStringValue(elementName, element), out intValue))
            {
                result = intValue;
            }
            return result;
        }

        public static double? GetDoubleValue(string elementName, XElement element)
        {
            double? result = null;
            double intValue = 0;
            if (double.TryParse(GetStringValue(elementName, element), out intValue))
            {
                result = intValue;
            }
            return result;
        }

        public static string GetStringValue(string elementName, XElement element)
        {
            string result = string.Empty;
            try
            {
                result = LkCommonUtil.TrimText(element.Element(elementName).Value);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public static bool GetBoolValue(string elementName, XElement element)
        {
            bool result = false;
            string boolValue = GetStringValue(elementName, element).ToLower();
            if (boolValue == "1" || string.Compare(boolValue, "true", true) == 0 || string.Compare(boolValue, "on", true) == 0)
            {
                result = true;
            }
            else if (boolValue == "0" || string.Compare(boolValue, "false", true) == 0 || string.Compare(boolValue, "off", true) == 0)
            {
                result = false;
            }
            return (result);
        }

        public static DateTime? GetDateTimeValue(string elementName, XElement element)
        {
            DateTime? result = null;
            DateTime datetimeValue = DateTime.MinValue;
            if (DateTime.TryParse(GetStringValue(elementName, element), out datetimeValue))
            {
                result = datetimeValue;
            }
            return result;
        }

        public static object GetEnumValue(Type enumType, string elementName, XElement element)
        {
            object result = null;
            string profileValue = GetStringValue(elementName, element);
            try
            {
                result = Enum.Parse(enumType, profileValue, true);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public static object GetEnumValue(Type enumType, string elementName, string defaultValue, XElement element, bool toUpper = false)
        {
            object result = null;
            string profileValue = GetStringValue(elementName, element);
            if (toUpper)
            {
                profileValue = profileValue.ToUpper();
                defaultValue = defaultValue.ToUpper();
            }
            try
            {
                result = Enum.Parse(enumType, profileValue, true);
            }
            catch
            {
                result = Enum.Parse(enumType, defaultValue);
            }
            return result;
        }

        //public static ModbusFunctionCode GetFunctionCode(string elementName, XElement element)
        //{
        //    ModbusFunctionCode functionCode;
        //    Enum.TryParse(GetStringValue(elementName, element), out functionCode);
        //    if (!Enum.IsDefined(typeof(ModbusFunctionCode), functionCode))
        //    {
        //        functionCode = ModbusFunctionCode.Code3;
        //    }
        //    return functionCode;
        //}


        #endregion

    }
}
