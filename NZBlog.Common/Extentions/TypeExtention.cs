using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;

namespace NZBlog.Common
{
    public static class TypeExtention
    {
        /// <summary>
        /// 判断指定字符是否为null或空字符串
        /// </summary>
        public static bool IsNullOrEmpty(this string inputString)
        {
            return string.IsNullOrEmpty(inputString);
        }

        /// <summary>
        /// 指定字符是否为null
        /// </summary>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// 判断指定对象是否为Int32类型
        /// </summary>
        public static bool IsInt32(this object obj)
        {
            if (obj == null) return false;
            int i;
            return int.TryParse(obj.ToString(), out i);
        }
        /// <summary>
        /// 将指定对象转换为Int32类型
        /// </summary>
        public static int ToInt32(this object obj, int defaultVal = 0)
        {
            if (obj == null) return defaultVal;
            int i;
            if (!int.TryParse(obj.ToString(), out i))
            {
                return defaultVal;
            }
            return i;
        }

        /// <summary>
        /// 判断指定对象是否为decimal类型
        /// </summary>
        public static bool IsDecimal(this object obj)
        {
            if (obj == null) return false;
            decimal i;
            return !decimal.TryParse(obj.ToString(), out i);
        }
        /// <summary>
        /// 将指定对象转换为decimal类型
        /// </summary>
        public static decimal ToDecimal(this object obj, decimal defaultVal = 0)
        {
            if (obj == null) return defaultVal;
            decimal i;
            if (!decimal.TryParse(obj.ToString(), out i))
            {
                return defaultVal;
            }
            return i;
        }

        /// <summary>
        /// 判断指定对象是否为DateTime类型
        /// </summary>
        public static bool IsDateTime(this object obj)
        {
            if (obj == null) return false;
            DateTime i;
            return !DateTime.TryParse(obj.ToString(), out i);
        }
        /// <summary>
        /// 将指定对象转换为DateTime类型
        /// </summary>
        public static DateTime ToDateTime(this object obj, DateTime defaultVal = default(DateTime))
        {
            if (obj == null) return defaultVal;
            DateTime i;
            if (!DateTime.TryParse(obj.ToString(), out i))
            {
                return defaultVal;
            }
            return i;
        }

        /// <summary>
        /// 将数组对象链接成字符串
        /// </summary>
        /// <param name="source">数据源</param>
        /// <param name="splitString">分隔符</param>
        /// <returns></returns>
        public static string SumToString<TSource>(this IEnumerable<TSource> source, string splitString = ",",
            bool isRemoveEmpty = false)
        {
            if (source == null || source.Count() == 0)
                return "";
            int i = 0;
            StringBuilder sb = new StringBuilder("");
            foreach (var item in source)
            {
                string itemValue = "";
                if (item != null)
                {
                    itemValue = item.ToString();
                }
                if (isRemoveEmpty)
                {

                    continue;
                }
                if (i == 0)
                    sb.Append(item.ToString());
                else
                    sb.Append(splitString + item.ToString());
                i++;
            }
            return sb.ToString();
        }
    }
}
