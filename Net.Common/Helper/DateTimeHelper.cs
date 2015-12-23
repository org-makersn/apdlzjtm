using System;

namespace Net.Common.Helper
{
    public class DateTimeHelper
    {
        /// <summary>
        /// get the first day of a month
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /// <summary>
        /// get the last day of a month
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// get the first day of a year
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DateTime FirstDayOfYear(DateTime datetime)
        {
            return DateTime.Parse(datetime.ToString("yyyy-01-01"));
        }

        /// <summary>
        /// get the last day of a year
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DateTime LastDayOfYear(DateTime datetime)
        {
            return DateTime.Parse(datetime.ToString("yyyy-01-01")).AddYears(1).AddDays(-1);
        }

        /// <summary>
        /// Convert a date time object to Unix time representation.
        /// </summary>
        /// <param name="datetime">The datetime object to convert to Unix time stamp.</param>
        /// <returns>Returns a numerical representation (Unix time) of the DateTime object.</returns>
        public long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(datetime - sTime).TotalSeconds;
        }


        /// <summary>
        /// Convert Unix time value to a DateTime object.
        /// </summary>
        /// <param name="unixtime">The Unix time stamp you want to convert to DateTime.</param>
        /// <returns>Returns a DateTime object that represents value of the Unix time.</returns>
        public DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return sTime.AddSeconds(unixtime);
        }
    }
}