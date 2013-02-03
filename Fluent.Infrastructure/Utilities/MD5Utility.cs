using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Fluent.Infrastructure.Utilities
{
    /// <summary>
    /// 制作人：陈春伟  2012.07.20
    /// Md5 哈希计算工具类
    /// </summary>
    public class Md5Utility
    {
        private static readonly MD5 Cryptography = MD5.Create();

        /// <summary>
        /// md5 hash值计算
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMd5HashCode(string str)
        {
            return GetMd5HashCode(Encoding.UTF8.GetBytes(str.Trim()));
        }

        /// <summary>k
        /// md5 hash值计算
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetMd5HashCode(byte[] bytes)
        {
            return ConvertHashBytes2String(Cryptography.ComputeHash(bytes));
        }
        /// <summary>
        /// 转换byte值为16进制值
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static string ConvertHashBytes2String(byte[] bytes)
        {
            var returnValue = "";
            for (var i = 0; i < bytes.Length; i++)
            {
                returnValue += bytes[i].ToString("x").PadLeft(2, '0');
            }
            return returnValue;
        }
    }
}
