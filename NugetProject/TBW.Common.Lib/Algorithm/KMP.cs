using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TBW.Common.Lib.Algorithm
{
    /// <summary>
    /// 字符串匹配算法
    /// </summary>
    public class KMP
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s">原字符串</param>
        /// <param name="t">待匹配字符串</param>
        /// <returns>匹配字符串出现次数</returns>
        public static int KmpIndexOf(string s, string t)
        {
            int i = 0, j = 0, v;
            int[] nextVal = GetNextVal(t);

            while (i < s.Length && j < t.Length)
            {
                if (j == -1 || s[i] == t[j])
                {
                    i++;
                    j++;
                }
                else
                {
                    j = nextVal[j];
                }
            }

            if (j >= t.Length)
                v = i - t.Length;
            else
                v = -1;

            return v;
        }



        private static int[] GetNextVal(string t)
        {
            int j = 0, k = -1;
            int[] nextVal = new int[t.Length];

            nextVal[0] = -1;

            while (j < t.Length - 1)
            {
                if (k == -1 || t[j] == t[k])
                {
                    j++;
                    k++;
                    if (t[j] != t[k])
                    {
                        nextVal[j] = k;
                    }
                    else
                    {
                        nextVal[j] = nextVal[k];
                    }
                }
                else
                {
                    k = nextVal[k];
                }
            }

            return nextVal;
        }
    }
}
