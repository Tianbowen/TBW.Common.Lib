using System;
using System.Collections.Generic;
using System.Text;

namespace TBW.Common.Lib.Extension.DateTimeExtension
{
    public static class DateTimeEX
    {
        public static bool IsMinor(this DateTime dob, DateTime on)
        {
            var timespan = on - dob;
            var age = (int)timespan.TotalDays / 365.2425;
            return age < 18;
        }

        public static string ToString_yMdHms(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToString_yMd(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}
