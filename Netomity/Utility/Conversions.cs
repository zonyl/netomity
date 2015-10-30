using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Netomity.Utility
{
    public static class Conversions
    {
        public static string BytesToHex(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string BytesToHex(byte ba)
        {
            var hex = new StringBuilder(2);
            hex.AppendFormat("{0:x2}", ba);
            return hex.ToString();
        }

        public static string BytesToAscii(byte[] ba)
        {
//            return System.Text.Encoding.UTF8.GetString(ba);
            string r = "";
            foreach(var b in ba)
            {
                r += Convert.ToChar(b);
            }
            return r;
        }

        public static byte[] HexToBytes(string hexString)
        {
            hexString = new Regex("[^a-zA-Z0-9]").Replace(hexString,"");
            int NumberChars = hexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            return bytes;
        }

        public static string HexToAscii(string hexString)
        {
            return BytesToAscii(HexToBytes(hexString));
        }

        public static object AsciiToHex(string ascii)
        {

            char[] charValues = ascii.ToCharArray();
            string hexOutput = "";
            foreach (char _eachChar in charValues)
            {
                // Get the integral value of the character.
                int value = Convert.ToInt32(_eachChar);
                // Convert the decimal value to a hexadecimal value in string form.
                hexOutput += String.Format("{0:X2}", value);
                // to make output as your eg 
                //  hexOutput +=" "+ String.Format("{0:X}", value);

            }
            return hexOutput;
        }

        public static byte[] AsciiToBytes(string ascii)
        {
            var r = ascii.Select(c => (byte)c).ToArray();
            return r; 
        }

        internal static int BytesToInt(byte b)
        {
            return (int)b;
        }

        public static T ValueToStringEnum<T>(string value)
        {
            T e = default(T);
            foreach (var fieldS in typeof(T).GetFields())
            {
                if (fieldS.GetValue(null).ToString().ToLower() == value.ToLower())
                {
                    e = (T)fieldS.GetValue(null);
                    break;
                }
            }
            return e;
        }

        public static byte[] BinaryStrToBytes(string str)
        {
            str = str.Replace(" ", string.Empty);

            var strings = SplitString(str, 8).ToList();
            return strings.Select(s => Convert.ToByte(Convert.ToInt32(s,2))).ToArray();
        }

        private static IEnumerable<string> SplitString(string str, int length)
        {
            return str.Select((x, i) => i)
                .Where(i => i % length == 0)
                .Select(i => str.Substring(i, str.Length - i >= length ? length : str.Length - i));

        }

        public static string BinaryStrToAscii(string str)
        {
            return BytesToAscii(BinaryStrToBytes(str));
        }

        public static byte[] BytesReverse(byte[] bytes)
        {
            for (var i = 0; i  <bytes.Count(); i++)
            {
                bytes[i] = (byte) ((bytes[i] * 0x202020202 & 0x010884422010) % 1023);
            }
            return bytes;
        }
    }
}
