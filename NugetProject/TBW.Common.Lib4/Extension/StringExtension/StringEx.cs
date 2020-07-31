using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBW.Common.Lib4.Extension.StringExtension
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

    }
}
