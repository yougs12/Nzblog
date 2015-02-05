using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Security; 
using System.Security.Cryptography;

namespace NZBlog.Common
{
    public static class Security
    {
        /// <summary> 
        /// SHA1加密字符串 
        /// </summary> 
        /// <param name="source">源字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public static string SHA1(this string source)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "SHA1");
        }

        /// <summary> 
        /// MD5加密字符串 
        /// </summary> 
        /// <param name="source">源字符串</param> 
        /// <returns>加密后的字符串</returns> 
        public static string MD5(this string source)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5");
        }

        /// <summary>
        /// 对称加密
        /// </summary>
        public static string Encrypt(this string inputString)
        {
            return new Safe().Encrypto(inputString);
        }
        /// <summary>
        /// 对称解密
        /// </summary>
        public static string Decrypt(this string inputString)
        {
            return new Safe().Decrypto(inputString);
        }
    }
}
