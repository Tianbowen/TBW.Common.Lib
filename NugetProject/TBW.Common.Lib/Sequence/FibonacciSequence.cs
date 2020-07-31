using System;
using System.Collections.Generic;
using System.Text;

namespace TBW.Common.Lib.Sequence
{
    /// <summary>
    /// 斐波那契数列（Fibonacci sequence），又称黄金分割数列
    /// 指的是这样一个数列：1、1、2、3、5、8、13、21、34、……
    ///  for (int i = 0; i < 15; i++)
    ///  {
    ///      Console.WriteLine($"{i + 1}: {FibonacciNumber(i)}");
    ///  }
    /// </summary>
    public class FibonacciSequence
    {
        public static int FibonacciNumber(int n)
        {
            int a = 0;
            int b = 1;
            int tmp;

            for (int i = 0; i < n; i++)
            {
                tmp = a;
                a = b;
                b += tmp;
            }

            return a;
        }
    }

    /// <summary>
    /// 斐波那契数列（Fibonacci sequence），又称黄金分割数列
    /// 指的是这样一个数列：1、1、2、3、5、8、13、21、34、……
    ///  var generator = new FibonacciGenerator();
    ///  foreach (var digit in generator.Generate(15))
    /// </summary>
    public class FibonacciGenerator
    {
        private Dictionary<int, int> _cache = new Dictionary<int, int>();

        private int Fib(int n) => n < 2 ? n : FibValue(n - 1) + FibValue(n - 2);

        private int FibValue(int n)
        {
            if (!_cache.ContainsKey(n))
            {
                _cache.Add(n, Fib(n));
            }

            return _cache[n];
        }

        public IEnumerable<int> Generate(int n)
        {
            for (int i = 0; i < n; i++)
            {
                yield return FibValue(i);
            }
        }
    }
}
