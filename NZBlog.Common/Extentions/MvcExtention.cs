using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    public static class MvcExtention
    {
        public static string CutHtml(this string strInput, int length)
        {
            return NZBlog.Common.Provider.SubStringHTML.subStringHTML(strInput, length);
        }

        /// <summary>
        /// 按ASCIIEncoding编码截取字符串
        /// </summary>
        /// <param name="inputString">需要截取的字符串</param>
        /// <param name="len">要保留的长度</param>
        /// <returns>截取后的字符串</returns>
        public static string CutStr(this string inputString, int len)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    tempLen += 2;
                }
                else
                {
                    tempLen += 1;
                }
                try
                {
                    tempString += inputString.Substring(i, 1);
                }
                catch
                {
                    break;
                }
                if (tempLen >= len)
                    break;
            }
            //如果截过则加上半个省略号 
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
            if (mybyte.Length > len)
                tempString += "…";
            ascii = null;
            return tempString;
        }
    }
}
