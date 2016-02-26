using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Design.Web.Admin.Helper
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

        /**/
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

        /**/
        /// <summary>
        /// get the last day of a year
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DateTime LastDayOfYear(DateTime datetime)
        {
            return DateTime.Parse(datetime.ToString("yyyy-01-01")).AddYears(1).AddDays(-1);
        }
    }
}
