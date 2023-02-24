using System;
using System.Collections.Generic;
using System.Text;

namespace TBW.Common.Lib.Format
{
    public class StringFormat
    {
        public static string FormatVersion_V1(string major, string minor, string build, string revision)
        {
            return string.Format("{0}.{1}.{2}.{3}", major, minor, build, revision);
        }

        public static string FormatVerion_V2(string major, string minor, string build, string revision) =>
            $"{major}.{minor}.{build}.{revision}";

        public static string FormatVerion_V3(string major, string minor, string build, string revision)
        {
            // //in .NET 10 C# 6
            //var handler = new DefaultInterpolatedStringHandler(literalLength: 3, formattedCount: 4);
            //handler.AppendFormatted(major);
            //handler.AppendFormatted(".");
            //handler.AppendFormatted(minor);
            //handler.AppendFormatted(".");
            //handler.AppendFormatted(build);
            //handler.AppendFormatted(".");
            //handler.AppendFormatted(revision);
            //return handler.ToStringAndClear();

            var array = new object[4];
            array[0] = major;
            array[1] = minor;
            array[2] = build;
            array[3] = revision;
            return string.Format("{0}.{1}.{2}.{3}",array);
        }
    }
}
