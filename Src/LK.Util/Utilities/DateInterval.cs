namespace LK.Util
{
    public class DateInterval
    {
        private int _fullMonthInterval = 0;
        /// <summary>
        /// 兩日期的月份間距(頭尾不滿一個月時去除)
        /// </summary>
        public int FullMonthInterval
        {
            get
            {
                return _fullMonthInterval;
            }
            set
            {
                _fullMonthInterval = value;
            }
        }

        private int _startMonthToEndDayInterval = 0;
        /// <summary>
        /// 起始月到月底的日期間距(如起始月是1號時，該月為一整個月，固間距為0)
        /// </summary>
        public int StartMonthToEndDayInterval
        {
            get
            {
                return _startMonthToEndDayInterval;
            }
            set
            {
                _startMonthToEndDayInterval = value;
            }
        }

        private int _endMonthToCurrentDayInterval = 0;

        /// <summary>
        /// 結束月從1號到現在日的日期間距(如結束月的現在日為月底時，該月為一整個月，固間距為0)
        /// </summary>
        public int EndMonthToCurrentDayInterval
        {
            get
            {
                return _endMonthToCurrentDayInterval;
            }
            set
            {
                _endMonthToCurrentDayInterval = value;
            }
        }

        private int _dayIntervalOfSameYearMonth = 0;
        /// <summary>
        /// 兩日期為相同年月時的日期間距
        /// </summary>
        public int DayIntervalOfSameYearMonth
        {
            get
            {
                return _dayIntervalOfSameYearMonth;
            }
            set
            {
                _dayIntervalOfSameYearMonth = value;
            }
        }

        private int _totalYearInterval = 0;
        /// <summary>
        /// 總年份間距
        /// </summary>
        public int TotalYearInterval
        {
            get
            {
                return _totalYearInterval;
            }
            set
            {
                _totalYearInterval = value;
            }
        }

        private int _totalMonthInterval = 0;
        /// <summary>
        /// 總月份間距
        /// </summary>
        public int TotalMonthInterval
        {
            get
            {
                return _totalMonthInterval;
            }
            set
            {
                _totalMonthInterval = value;
            }
        }

        private int _totalDayInterval = 0;
        /// <summary>
        /// 總日期間距
        /// </summary>
        public int TotalDayInterval
        {
            get
            {
                return _totalDayInterval;
            }
            set
            {
                _totalDayInterval = value;
            }

        }

        private int _totalWorkDay = 0;
        /// <summary>
        /// 總工作日(單純去除六、日)
        /// </summary>
        public int TotalWorkDay
        {
            get
            {
                return _totalWorkDay;
            }
            set
            {
                _totalWorkDay = value;
            }
        }
    }
}
