using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using Microsoft.VisualBasic;
using System.Collections;
using System.Drawing;
using System.Net;
using System.Web.UI.WebControls;
using System.Collections.Generic;

namespace Fluent.Infrastructure.Utilities
{
    /// <summary>
    /// 对字符串操作的类
    /// </summary>
    public class MyString
    {
        /// <summary>
        /// 将string 的List转为元素的字符串连接
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static string convertListToString(IList<string> lst)
        {
            StringBuilder str = new StringBuilder();
            foreach (string s in lst)
            {
                str.Append(s);
            }
            return str.ToString();
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(object Expression, int defValue)
        {
            return TypeParse.StrToInt(Expression, defValue);
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(object strValue, float defValue)
        {
            return TypeParse.StrToFloat(strValue, defValue);
        }

        #region object转成byte型
        /// <summary>
        /// string型转换为Byte型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static Byte StrToByte(object strValue, byte defValue)
        {
            try
            {
                return Convert.ToByte(strValue);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// string型转换为Byte型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的int类型结果</returns>
        public static Byte StrToByte(object strValue)
        {
            return StrToByte(strValue, 0);
        }
        #endregion

        #region object转成日期类型
        /// <summary>
        /// string型转换为DateTime型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime StrToDateTime(object strValue, DateTime defValue)
        {
            try
            {
                return Convert.ToDateTime(strValue);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// string型转换为DateTime型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的DateTime类型结果</returns>
        public static DateTime StrToDateTime(object strValue)
        {
            return StrToDateTime(strValue, DateTime.MinValue);
        }
        #endregion

        #region object转成Boolean类型
        /// <summary>
        /// string型转换为Boolean型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的Boolean类型结果</returns>
        public static Boolean StrToBoolean(object strValue, Boolean defValue)
        {
            try
            {
                return Convert.ToBoolean(strValue);
            }
            catch
            {
                return defValue;
            }
        }

        /// <summary>
        /// string型转换为Boolean型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <returns>转换后的Boolean类型结果</returns>
        public static Boolean StrToBoolean(object strValue)
        {
            return StrToBoolean(strValue, false);
        }
        #endregion

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 检测字符串的长度是否在2数字之间
        /// </summary>
        /// <param name="str">要检测的字符串</param>
        /// <param name="min">最短必须多少</param>
        /// <param name="max">最长必须多少</param>
        /// <returns></returns>
        public static bool StrLengthInBetween(string str, int min, int max)
        {
            int length = string.IsNullOrEmpty(str) ? 0 : str.Length;
            return (length >= min) && (length <= max);
        }

        /// <summary>
        /// 将IEnumerator类型的元素用制定分割符号连接起来成为字符串比如你可以将int数组通过","将所有元素连接成一个字符串
        /// ConnectArray({1,2,3,4,5,6},"|")="1|2|3|4|5|6"
        /// </summary>
        /// <param name="ie"></param>
        /// <param name="border"></param>
        /// <returns></returns>
        public static string ConnectArray(IEnumerable ie, string border)
        {
            if (ie == null) return "";
            string str = "";
            foreach (object i in ie)
            {
                str += i.ToString() + border;
            }
            if (string.IsNullOrEmpty(str)) return "";
            return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int IndexOf(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置，并且区分大小写
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int IndexOf(string strSearch, string[] stringArray)
        {
            return IndexOf(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return IndexOf(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素，不区分大小写
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素用逗号分隔stringarray 如  判断"a"是否在"a,b,c,d"中，
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串如："a,b,c,d"</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串如："a,b,c,d"</param>
        /// <param name="strsplit">分割字符串如：","</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串如："a,b,c,d"</param>
        /// <param name="strsplit">分割字符串如：","</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        }

        /// <summary>
        /// 删除字符串的回车/换行
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveRNString(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            return str.Replace("\r", "").Replace("\n", "");
        }
        /// <summary>
        /// 删除所有空白字符比如换行符 空格符等
        /// </summary>
        /// <param name="sourcestr"></param>
        /// <returns></returns>
        public static string RemoveSpaceString(string sourcestr)
        {
            return Regex.Replace(sourcestr, "\\s", "");
        }

        /// <summary>
        /// 截取填充字符串到多少位字符,
        /// </summary>
        /// <param name="str">原字符床</param>
        /// <param name="n">要截取的长度</param>
        /// <param name="fill">超出后的填补字符串</param>
        /// <returns></returns>
        public static string StrSub(string str, int length, string fill)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (str.Length <= length) return str;
            if (fill == null) fill = "";
            return str.Substring(0, length - fill.Length) + fill;
        }



        #region 截取字符串
        /// <summary>
        /// 截取字符串,一个汉字占位2.
        /// </summary>
        /// <param name="str">待截取字符串</param>
        /// <param name="len">截取长度</param>
        /// <param name="rep">过长字符串添加后缀</param>
        /// <returns></returns>
        public static string SubString(string str, int len, string rep)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int tempLen = 0;
            string tempString = "";
            byte[] s = ascii.GetBytes(str);
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
                    tempString += str.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (tempLen > len)
                    break;
            }
            //如果截过则加上半个省略号
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(str);
            if (mybyte.Length > len + 1)
                tempString += rep;
            return tempString;

        }
        #endregion




        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }


        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        public static string Spaces(int nSpaces)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < nSpaces; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }


        /// <summary>
        /// 移除字符串中的可能引起危险Sql字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSqlUnsafeString(string str)
        {
            string p = @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']";
            return Regex.Replace(str, p, "");
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 替换sql语句中的有问题符号
        /// </summary>
        public static string ChkSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("'", "''");
                str2 = str;
            }
            return str2;
        }

        /// <summary>
        /// 改正sql语句中的转义字符
        /// </summary>
        public static string MashSQL(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\'", "'");
                str2 = str;
            }
            return str2;
        }

        /// <summary>
        /// 替换回车换行符为html换行符
        /// </summary>
        public static string StrFormat(string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                str2 = str;
            }
            return str2;
        }

         

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 为脚本替换特殊字符串将字符串中的引号进行转义
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceStrToScript(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\\"");
            return str;
        }

        

        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {
            string[] userip = MyString.SplitString(ip, @".");
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = MyString.SplitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (r == 4)
                {
                    return true;
                }
            }
            return false;

        }


        [Flags()]
        public enum FileType
        {
            JPG = 1,
            GIF = 2,
            BMP = 4,
            PNG = 8,
            EXE = 16,
            RAR = 32
        }

        /// <summary>
        /// 检测文件是否为允许的类型，非简单扩展名检测，从文件流检测
        /// </summary>
        /// <param name="fs">文件流</param>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        public static bool IsAllowedExtension(Stream fs, FileType type)
        {
            #region 函数体
            System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
            string fileclass = "";
            byte buffer;
            try
            {
                buffer = r.ReadByte();
                fileclass = buffer.ToString();
                buffer = r.ReadByte();
                fileclass += buffer.ToString();

            }
            catch
            {

            }
            r.Close();
            fs.Close();
            if ((type & FileType.BMP) == FileType.BMP)
            {
                if (fileclass == "6677") return true;
            }
            if ((type & FileType.EXE) == FileType.EXE)
            {
                if (fileclass == "7790") return true;
            }
            if ((type & FileType.GIF) == FileType.GIF)
            {
                if (fileclass == "7173") return true;
            }
            if ((type & FileType.JPG) == FileType.JPG)
            {
                if (fileclass == "255216") return true;
            }
            if ((type & FileType.PNG) == FileType.PNG)
            {
                if (fileclass == "13780") return true;
            }
            if ((type & FileType.RAR) == FileType.RAR)
            {
                if (fileclass == "8297") return true;
            }
            //说明255216是jpg;7173是gif;6677是BMP,13780是PNG;7790是exe,8297是rar
            return false;
            #endregion
        }

        /// <summary>
        /// 将字串用指定分割符号,分开,将每个元素转换为INT并用数组返回
        /// </summary>
        /// <param name="str">要转换的字符串如:1,2,3,4</param>
        /// <param name="splite">分隔符如:,</param>
        /// <param name="defaultval">如果有某个元素转换不成功时使用默认值替代</param>
        /// <returns></returns>
        public static int[] StrToIntArray(string str, string splite, int defaultval)
        {
            if (string.IsNullOrEmpty(str)) return null;
            string[] strs = str.Split(new string[] { splite }, StringSplitOptions.RemoveEmptyEntries);
            if (strs == null || strs.Length == 0) return null;
            int[] vals = new int[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                vals[i] = TypeParse.StrToInt(strs[i], defaultval);
            }
            return vals;
        }

        /// <summary>
        /// 将字串用指定分割符号,分开,将每个元素转换为INT并用数组返回,
        /// </summary>
        /// <param name="str">要转换的字符串如:1,2,3,4</param>
        /// <param name="splite">分隔符如:,</param>
        /// <returns></returns>
        public static int[] StrToIntArray(string str, string splite)
        {
            if (string.IsNullOrEmpty(str)) return null;
            string[] strs = str.Split(new string[] { splite }, StringSplitOptions.RemoveEmptyEntries);
            if (strs == null || strs.Length == 0) return null;
            List<int> vals = new List<int>();
            for (int i = 0; i < strs.Length; i++)
            {
                if (TypeParse.IsNumeric((strs[i])))
                {
                    vals.Add(Convert.ToInt32(strs[i]));
                }
            }
            return vals.ToArray();
        }

        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearLastChar(string str)
        {
            if (str == "")
                return "";
            else
                return str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }



    }

}
