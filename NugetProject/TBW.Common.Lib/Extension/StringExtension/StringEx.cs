using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace TBW.Common.Lib.Extension.StringExtension
{
    public static class StringEx
    {
        /// <summary>
        /// 判断字符串是否为空 null 空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 字符串转换Int32类型
        /// </summary>
        /// <param name="str">默认或转换失败为0</param>
        /// <returns></returns>
        public static int ToInt32(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                int tempResult;
                if (int.TryParse(str, out tempResult))
                {
                    return tempResult;
                }
            }
            return 0;
        }        
        /// <summary>
        /// 字符串转换Long类型
        /// </summary>A
        /// <param name="str">默认或转换失败为long默认值</param>
        /// <returns></returns>
        public static long ToLong(this string str)
        {
            long nL;
            if (!long.TryParse(str,out nL))
            {

            }
            return nL;
        }

        public static DateTime ToDateTime(this string str)
        {
            return DateTime.Now;
        }

        #region Nuke.Common.
        // PureAttribute 类表示类型或方法在功能上是“纯”的，即它不会进行任何可见的状态更改
        [Pure]
        public static bool ContainsOrdinalIgnoreCase(this string str, string other)
        {
            return str.IndexOf(other, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        [Pure]
        public static bool EqualsOrdinalIgnoreCase(this string str, string other)
        {
            return str.Equals(other, StringComparison.OrdinalIgnoreCase);
        }

        [Pure]
        public static bool ContainsAnyOrdinalIgnoreCase(this string str, params string[] others)
        {
            return others.Any(str.ContainsOrdinalIgnoreCase);
        }

        [Pure]
        public static bool ContainsAnyOrdinalIgnoreCase(this string str, IEnumerable<string> others)
        {
            return others.Any(str.ContainsOrdinalIgnoreCase);
        }

        [Pure]
        public static bool EqualsAnyOrdinalIgnoreCase(this string str, params string[] others)
        {
            return others.Any(str.EqualsOrdinalIgnoreCase);
        }

        [Pure]
        public static bool EqualsAnyOrdinalIgnoreCase(this string str, IEnumerable<string> others)
        {
            return others.Any(str.EqualsOrdinalIgnoreCase);
        }

        [Pure]
        public static bool StartsWithOrdinalIgnoreCase(this string str, string other)
        {
            return str.StartsWith(other, StringComparison.OrdinalIgnoreCase);
        }

        [Pure]
        public static bool EndsWithOrdinalIgnoreCase(this string str, string other)
        {
            return str.EndsWith(other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool StartsWithAny(this string str, params string[] others)
        {
            return str.StartsWithAny(others.AsEnumerable());
        }

        public static bool StartsWithAny(this string str, IEnumerable<string> others)
        {
            return others.Any(str.StartsWith);
        }

        public static bool StartsWithAnyOrdinalIgnoreCase(this string str, params string[] others)
        {
            return str.StartsWithAnyOrdinalIgnoreCase(others.AsEnumerable());
        }

        public static bool StartsWithAnyOrdinalIgnoreCase(this string str, IEnumerable<string> others)
        {
            return others.Any(str.StartsWithOrdinalIgnoreCase);
        }

        public static bool EndsWithAny(this string str, params string[] others)
        {
            return str.EndsWithAny(others.AsEnumerable());
        }

        public static bool EndsWithAny(this string str, IEnumerable<string> others)
        {
            return others.Any(str.EndsWith);
        }

        public static bool EndsWithAnyOrdinalIgnoreCase(this string str, params string[] others)
        {
            return str.EndsWithAnyOrdinalIgnoreCase(others.AsEnumerable());
        }

        public static bool EndsWithAnyOrdinalIgnoreCase(this string str, IEnumerable<string> others)
        {
            return others.Any(str.EndsWithOrdinalIgnoreCase);
        }

        #endregion

    }
}
