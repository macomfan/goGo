﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    class TextUtil
    {
        // LF 0A
        // CR 0D
        //private static string 

        public static string Escaping(string orgstring)
        {
            string res = orgstring;
            if (orgstring.IndexOf("\\\\") != -1)
            {
                res = res.Replace("\\\\", "\\");
            }
            if (orgstring.IndexOf("\\]") != -1)
            {
                res = res.Replace("\\]", "]");
            }
            if (orgstring.IndexOf("\\:") != -1)
            {
                res = res.Replace("\\:", ":");
            }
            return res;
        }
    }

    public static class SGF_Type_Convertor
    {
        public static class Double
        {
            public static int Decode(List<byte[]> values, string propertyName)
            {
                if (values.Count == 1)
                {
                    return int.Parse(Encoding.ASCII.GetString(values[0]));
                }
                else if (values.Count > 1)
                {
                    SGFException.Throw(string.Format("The Double type has multi values in {0}", propertyName));
                }
                else
                {
                    SGFException.Throw(string.Format("Attempt convert a null property in {0}", propertyName));
                }
                return 0;
            }

            public static List<byte[]> Encode(int value, string propertyName)
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Encoding.ASCII.GetBytes(value.ToString()));
                return result;
            }
        }

        public static class Number
        {
            public static int Decode(List<byte[]> values, string propertyName)
            {
                if (values.Count == 1 )
                {
                    return int.Parse(Encoding.ASCII.GetString(values[0]));
                }
                else if (values.Count > 1)
                {
                    SGFException.Throw(string.Format("The Number type has multi values in {0}", propertyName));
                }
                else
                {
                    SGFException.Throw(string.Format("Attempt convert a null property in {0}", propertyName));
                }
                return 0;
            }

            public static List<byte[]> Encode(int value, string propertyName)
            {
                List<byte[]> result = new List<byte[]>();
                result.Add(Encoding.ASCII.GetBytes(value.ToString()));
                return result;
            }
        }

        public static class SimpleText
        {
            public static string Decode(List<byte[]> values, string propertyName)
            {
                if (values.Count == 1)
                {
                    return TextUtil.Escaping(Encoding.ASCII.GetString(values[0]));
                }
                else if (values.Count > 1)
                {
                    SGFException.Throw(string.Format("The SimpleText type has multi values in {0}", propertyName));
                }
                else
                {
                    SGFException.Throw(string.Format("Attempt convert a null property in {0}", propertyName));
                }
                return string.Empty;
            }
            public static List<byte[]> Encode(string value, string propertyName)
            {
                return null;
            }
        }

        public static class Text
        {
            public static string Decode(List<byte[]> values, string propertyName)
            {
                if (values.Count == 1)
                {
                    return TextUtil.Escaping(Encoding.ASCII.GetString(values[0]));
                }
                else if (values.Count > 1)
                {
                    SGFException.Throw(string.Format("The Text type has multi values in {0}", propertyName));
                }
                else
                {
                    SGFException.Throw(string.Format("Attempt convert a null property in {0}", propertyName));
                }
                return string.Empty;
            }
            public static List<byte[]> Encode(string value, string propertyName)
            {
                return null;
            }
        }
    }
}
