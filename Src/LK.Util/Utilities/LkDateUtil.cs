using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace LK.Util
{
    /// <summary>
    /// Data operation(Static)
    /// </summary>
    public static class LkDateUtil
    {
        /// <summary>
        /// 取得民國年
        /// </summary>
        public static int GetTaiwanYear(this DateTime dt)
        {
            TaiwanCalendar tc = new TaiwanCalendar();
            return tc.GetYear(dt);
        }

        /// <summary>
        /// 取得民國年月日 (100/10/10)
        /// </summary>
        public static string GetTaiwanDate(this DateTime dt)
        {
            TaiwanCalendar tc = new TaiwanCalendar();
            return tc.GetYear(dt) + "/" + tc.GetMonth(dt) + "/" + tc.GetDayOfMonth(dt);
        }

        /// <summary>
        /// 取得民國年月日 (100年10月10日)
        /// </summary>
        public static string GetTaiwanChiDate(this DateTime dt)
        {
            TaiwanCalendar tc = new TaiwanCalendar();
            return tc.GetYear(dt) + "年" + tc.GetMonth(dt) + "月" + tc.GetDayOfMonth(dt) + "日";
        }

        /// <summary>
        /// 取得農曆年
        /// </summary>
        public static int GetTaiwanLnsYear(this DateTime dt)
        {
            TaiwanLunisolarCalendar tlc = new TaiwanLunisolarCalendar();
            return tlc.GetYear(dt);
        }

        /// <summary>
        /// 取得農曆月
        /// </summary>
        public static int GetTaiwanLnsMonth(this DateTime dt)
        {
            TaiwanLunisolarCalendar tlc = new TaiwanLunisolarCalendar();
            return tlc.GetMonth(dt);
        }

        /// <summary>
        /// 取得農曆日
        /// </summary>
        public static int GetTaiwanLnsDay(this DateTime dt)
        {
            TaiwanLunisolarCalendar tlc = new TaiwanLunisolarCalendar();
            return tlc.GetDayOfMonth(dt);
        }

        /// <summary>
        /// 取得農曆年月日 (100/10/10)
        /// </summary>
        public static string GetTaiwanLnsDate(this DateTime dt)
        {
            TaiwanLunisolarCalendar tlc = new TaiwanLunisolarCalendar();
            return tlc.GetYear(dt) + "/" + tlc.GetMonth(dt) + "/" + tlc.GetDayOfMonth(dt);
        }

        /// <summary>
        /// 取得農曆年月日 (100年10月10日)
        /// </summary>
        public static string GetTaiwanLnsChiDate(this DateTime dt)
        {
            TaiwanLunisolarCalendar tlc = new TaiwanLunisolarCalendar();
            return tlc.GetYear(dt) + "年" + tlc.GetMonth(dt) + "月" + tlc.GetDayOfMonth(dt) + "日";
        }

        /// <summary>
        /// 取得天干地支年月日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetTaiwanHeaEarDate(this DateTime dt)
        {
            string teanGean = "甲乙丙丁戊己庚辛壬癸";
            string deGe = "子丑寅卯辰巳午未申酉戌亥";
            TaiwanLunisolarCalendar tlc = new TaiwanLunisolarCalendar();
            int lun60Year = tlc.GetSexagenaryYear(DateTime.Now.AddYears(0));
            int TeanGeanYear = tlc.GetCelestialStem(lun60Year) - 1;
            int DeGeYear = tlc.GetTerrestrialBranch(lun60Year) - 1;
            int lunMonth = tlc.GetMonth(DateTime.Now.AddYears(0));
            int leapMonth = tlc.GetLeapMonth(tlc.GetYear(DateTime.Now.AddYears(0)));
            if (leapMonth > 0 && lunMonth >= leapMonth)
            {
                lunMonth -= 1;
            }
            int lunDay = tlc.GetDayOfMonth(DateTime.Now.AddYears(0));
            return String.Format("{0}年{1}月{2}日", teanGean[TeanGeanYear].ToString() + deGe[DeGeYear].ToString(), lunMonth, lunDay);
        }

        /// <summary>
        /// 取得生肖
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GetChiAnimal(this DateTime dt)
        {
            string animal = "鼠牛虎兔龍蛇馬羊猴雞狗豬";
            TaiwanLunisolarCalendar tlc = new TaiwanLunisolarCalendar();
            int lun60Year = tlc.GetSexagenaryYear(DateTime.Now.AddYears(0));
            int DeGeYear = tlc.GetTerrestrialBranch(lun60Year) - 1;
            return animal[DeGeYear].ToString();
        }

        /// <summary>
        /// 計算日期間距
        /// </summary>
        /// <param name="sDate">起始日期</param>
        /// <param name="eDate">結束日期</param>
        /// <returns></returns>
        public static DateInterval CountDateInterval(this DateTime sDate, DateTime eDate)
        {
            DateInterval dateInterval = new DateInterval();
            TimeSpan ts = eDate - sDate;
            if (ts.Days + 1 < 0)
            {
                return dateInterval;
            }

            if (sDate == eDate)
            {
                return dateInterval;
            }
            dateInterval.TotalYearInterval = (int)(ts.TotalDays / 365);
            dateInterval.TotalMonthInterval = (int)(ts.TotalDays / (365 / 12));
            dateInterval.TotalDayInterval = (int)ts.Days;

            DateTime sTempDate = sDate;
            while (sTempDate <= eDate)
            {
                if (sTempDate.DayOfWeek != DayOfWeek.Saturday && sTempDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    dateInterval.TotalWorkDay++;
                }
                sTempDate = sTempDate.AddDays(1);
            }

            #region 算完整間隔日
            int sDateYear = sDate.Year;
            int sDateMonth = sDate.Month;
            int sDateDay = sDate.Day;
            int sDateMonthDay = DateTime.DaysInMonth(sDateYear, sDateMonth);

            int eDateYear = eDate.Year;
            int eDateMonth = eDate.Month;
            int eDateDay = eDate.Day;
            int eDateMonthDay = DateTime.DaysInMonth(eDateYear, eDateMonth);


            //計算起月數離當月 - 相隔天數
            int sMonthInterval = sDateMonthDay - (sDateDay - 1);
            //計算迄月數離當月 - 相隔天數
            int eMonthInterval = eDateMonthDay - (eDateMonthDay - eDateDay);

            int monthExtra = 0;
            if (sMonthInterval == sDateMonthDay)
            {
                monthExtra++;
                sMonthInterval = 0;
            }
            if (eMonthInterval == eDateMonthDay)
            {
                monthExtra++;
                eMonthInterval = 0;
            }


            int monthInterval = 0;
            if (sDateYear == eDateYear)
            {
                if (sDateMonth == eDateMonth)
                {
                    dateInterval.DayIntervalOfSameYearMonth = (ts.Days + 1);
                    return dateInterval;
                }
                else
                {
                    //計算起迄月數 - 中間相隔月數
                    monthInterval = (eDateMonth - (sDateMonth + 1)) + monthExtra;
                    dateInterval.FullMonthInterval = monthInterval;
                    dateInterval.StartMonthToEndDayInterval = sMonthInterval;
                    dateInterval.EndMonthToCurrentDayInterval = eMonthInterval;
                    return dateInterval;
                }
            }
            else
            {
                int yearInterval = eDateYear - (sDateYear + 1);
                if (yearInterval == 0)
                {
                    //計算起迄月數 - 中間相隔月數
                    monthInterval = (eDateMonth - (sDateMonth + 1) + 12) + monthExtra;
                    dateInterval.FullMonthInterval = monthInterval;
                    dateInterval.StartMonthToEndDayInterval = sMonthInterval;
                    dateInterval.EndMonthToCurrentDayInterval = eMonthInterval;
                    return dateInterval;
                }
                else
                {
                    //計算起迄年數 - 中間相隔年數
                    int monthsOfTotalYear = yearInterval * 12;
                    //計算起迄月數 - 中間相隔月數
                    monthInterval = (eDateMonth - (sDateMonth + 1) + 12);
                    //總計月數
                    int totalMonthInterval = monthsOfTotalYear + monthInterval + monthExtra;
                    dateInterval.FullMonthInterval = totalMonthInterval;
                    dateInterval.StartMonthToEndDayInterval = sMonthInterval;
                    dateInterval.EndMonthToCurrentDayInterval = eMonthInterval;
                    return dateInterval;
                }

            }
            #endregion


        }

        /// <summary>
        /// 獲取某一日期是該年中的第幾週
        /// </summary>
        /// <param name="d​​t">日期</param>
        /// <returns> 該日期在該年中的周數</returns>
        public static int GetWeekOfYear(this DateTime dt)
        {
            TaiwanCalendar tc = new TaiwanCalendar();
            return tc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday) + (dt.DayOfWeek == DayOfWeek.Sunday ? 1 : 0);
        }

        /// <summary>
        /// 獲取某一日期是該月中的第幾週
        /// </summary>
        /// <param name="d​​t">日期</param>
        /// <returns> 該日期在該月中的周數</returns>
        public static int GetWeekOfMonth(this DateTime dt)
        {
            DateTime first = new DateTime(dt.Year, dt.Month, 1);
            return GetWeekOfYear(dt) - GetWeekOfYear(first) + 1;
        }

        /// <summary>
        /// 獲取一年中有多少週
        /// </summary>
        /// <param name="year">年</param>
        /// <returns> 獲取一年中有多少週</returns>
        public static int GetWeekAmount(int year)
        {
            DateTime end = new DateTime(year, 12, 31);
            TaiwanCalendar tc = new TaiwanCalendar();
            return tc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 計算該週的第一天
        /// </summary>
        /// <param name="year">那一年</param>
        /// <param name="weekNum">那一年的第幾週</param>
        /// <returns></returns>
        public static DateTime GetWeekFirstDate(int year, int weekNum)
        {
            DateTime yearStart = new DateTime(year, 1, 1);
            int dayOfWeekValue = (int)yearStart.DayOfWeek;
            yearStart = yearStart.AddDays(0 - dayOfWeekValue);
            return yearStart.AddDays((weekNum - (dayOfWeekValue == 0 ? 2 : 1)) * 7);
        }


        public static int weekNum(this DateTime dt, System.DayOfWeek dw)
        {
            int weeknow = Convert.ToInt32(dw);
            int daydiff = (-1) * (weeknow + 1);
            int days = dt.AddDays(daydiff).DayOfYear;
            int weeks = days / 7;
            if (days % 7 != 0)
            {
                weeks++;
            }
            if (dt.Year != dt.AddDays(daydiff).Year)
                return 1;
            else
                return (weeks + 1);
        }

        /// <summary>
        /// 取得正規化後的時間，假設正規化1分鐘，17:48:12就會正規化成17:49:00
        /// </summary>
        /// <param name="time"></param>
        /// <param name="blockSec">正規化的block</param>
        /// <param name="isForeward">是否要正規化成前置時間，即17:48:00</param>
        /// <returns></returns>
        public static DateTime GetStandardTime(this DateTime time, int blockSec, bool isForeward = false)
        {
            int timeBlock = ((int)time.TimeOfDay.TotalSeconds / blockSec) + (isForeward ? 0 : 1);
            return time.Date.AddSeconds(timeBlock * blockSec);
        }
    }

}
