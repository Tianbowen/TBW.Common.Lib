using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TBW.Common.Lib.Extension.EnumExtension
{
    public static class EnumEx
    {
        public static string ToDescriptionString(this Enum val)
        {
            // ai自动生成
            //var type = val.GetType();
            //var name = Enum.GetName(type, val);
            //if (name == null) return null;
            //var field = type.GetField(name);
            //if (field == null) return null;
            //var attr = Attribute.GetCustomAttribute(field, typeof(System.ComponentModel.DescriptionAttribute)) as System.ComponentModel.DescriptionAttribute;
            //return attr == null ? null : attr.Description;

            var attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString())?.GetCustomAttributes(typeof(DescriptionAttribute), false);

            //return attributes is { Length > 0 } ? attributes.First().Description : val.ToString();
            return attributes.Length > 0 ? attributes.First().Description : val.ToString();
        }
    }
}
