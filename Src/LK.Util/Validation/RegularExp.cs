using System;
using System.Collections.Generic;
using System.Text;

namespace LK.Util
{
    class RegularExp
    {
        public const string Chinese = @"^[\u4E00-\u9FA5\uF900-\uFA2D]+$";
        public const string Color = "^#[a-fA-F0-9]{6}";
        public const string Date = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
        public const string DateTime = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d$";
        public const string Email = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
        public const string Float = @"^(-?\d+)(\.\d+)?$";
        public const string ImageFormat = @"\.(?i:jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$";
        public const string Integer = @"^-?\d+$";
        public const string IP = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
        public const string Letter = "^[A-Za-z]+$";
        public const string LowerLetter = "^[a-z]+$";
        public const string MinusFloat = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
        public const string MinusInteger = "^-[0-9]*[1-9][0-9]*$";
        public const string Mobile = "^0{0,1}13[0-9]{9}$";
        public const string NumbericOrLetterOrChinese = @"^[A-Za-z0-9\u4E00-\u9FA5\uF900-\uFA2D]+$";
        public const string Numeric = "^[0-9]+$";
        public const string NumericOrLetter = "^[A-Za-z0-9]+$";
        public const string NumericOrLetterOrUnderline = @"^\w+$";
        public const string PlusFloat = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
        public const string PlusInteger = "^[0-9]*[1-9][0-9]*$";
        public const string Telephone = @"(\d+-)?(\d{4}-?\d{7}|\d{3}-?\d{8}|^\d{7,8})(-\d+)?";
        public const string UnMinusFloat = @"^\d+(\.\d+)?$";
        public const string UnMinusInteger = @"\d+$";
        public const string UnPlusFloat = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
        public const string UnPlusInteger = @"^((-\d+)|(0+))$";
        public const string UpperLetter = "^[A-Z]+$";
        public const string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
    }
}
