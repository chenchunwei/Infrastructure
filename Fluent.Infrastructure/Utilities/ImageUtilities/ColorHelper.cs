using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Fluent.Infrastructure.Utilities.ImageUtilities
{
    public class ColorHelper
    {
        public static Color HexToColor(String hexString)
        {
            Color actColor;
            if (hexString.ToUpper() == "OOOOOO")
                return Color.Transparent;
            if (hexString.Length == 6)
            {
                var r = HexToInt(hexString.Substring(0, 2), 16);
                var g = HexToInt(hexString.Substring(2, 2), 16);
                var b = HexToInt(hexString.Substring(4, 2), 16);
                actColor = Color.FromArgb(r, g, b);
            }
            else
            {
                actColor = Color.White;
            }
            return actColor;
        }
        public static String ColorToHex(Color aColor)
        {
            if (aColor == Color.Transparent)
                return "oooooo";
            return IntToHex(aColor.R, 16) + IntToHex(aColor.G, 16) + IntToHex(aColor.B, 16);
        }

        /// <summary>  
        /// 十进制数转换成二、八、十六进制数  
        /// </summary>  
        /// <param name="intValue">十进制数</param>  
        /// <param name="mod">进制</param>  
        /// <returns></returns>  
        public static string IntToHex(int intValue, int mod)
        {
            var hexValue = string.Empty;
            var temp = intValue;
            while (temp > 0)
            {
                var addValue = temp / mod;
                var modValue = temp % mod;
                char charModValue;
                if (modValue >= 10)
                {
                    charModValue = (char)(modValue + 55);
                }
                else
                {
                    charModValue = (char)(modValue + 48);
                }
                hexValue = charModValue + hexValue;
                temp = addValue;
            }
            return hexValue; ;
        }


        /// <summary>  
        /// 16进制转换成十进制  
        /// </summary>  
        /// <param name="hexValue">非十进制数</param>  
        /// <param name="mod">传入的字串进制</param>  
        /// <returns>十进制数</returns>  
        public static int HexToInt(string hexValue, int mod)
        {
           return Convert.ToInt32(hexValue, 16);
        }

    }
}
