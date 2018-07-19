using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    class Util
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

        public static int MoveToInt(byte move)
        {
            if (move >= (byte)'a' && move <= 'z')
            {
                return move - (byte)'a';
            }
            else if (move >= (byte)'A' && move <= 'Z')
            {
                return move - (byte)'A';
            }
            return -1;
        }

        public static byte IntToMove(int value)
        {
            byte move = 0xFF;
            if (value >= 0 && value <= 52)
            {
                if (value < 26)
                {
                    move = (byte)(value + 'a');
                }
                else
                {
                    move = (byte)(value + 'A');
                }
            }
            return move;
        }

        public static List<byte[]> SplitByteArrayBy(byte[] value, char separator)
        {
            List<byte[]> result = new List<byte[]>();
            if (value.Length == 0)
            {
                return result;
            }
            int index = value.Length;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == (byte)separator)
                {
                    index = i;
                    break;
                }
            }
            byte[] left = new byte[index];
            result.Add(left);
            Buffer.BlockCopy(value, 0, left, 0, index);
            if (index < value.Length)
            {
                byte[] right = new byte[value.Length - index - 1];
                Buffer.BlockCopy(value, index + 1, right, 0, value.Length - index - 1);
                result.Add(right);
            }
            return result;
        }
    }

    public abstract class SGF_Type_Base
    {
        protected bool isBlank_ = true;
        protected List<byte[]> encoded_ = null;

        public bool IsBlank
        {
            get { return isBlank_; }
        }

        protected void LogSingleValueVerification(List<byte[]> values, string propertyName, SGF_Root_Setting setting)
        {
            if (values.Count > 1)
            {
                setting.PushError(propertyName, "The property has multi values");
            }
        }

        internal List<byte[]> Encoded
        {
            get { return encoded_; }
        }

        public abstract void Decode(List<byte[]> values, string propertyName, SGF_Root_Setting setting);
        public abstract void Encode(string propertyName, SGF_Root_Setting setting);
    }

    public class SGF_Type_Number : SGF_Type_Base
    {
        private int value_ = 0;

        public int Number
        {
            get { return value_; }
            set
            {
                value_ = value;
                isBlank_ = false;
            }
        }

        public override void Decode(List<byte[]> values, string propertyName, SGF_Root_Setting setting)
        {
            LogSingleValueVerification(values, propertyName, setting);
            if (values.Count == 1)
            {
                value_ = int.Parse(Encoding.ASCII.GetString(values[0]));
                isBlank_ = false;
            }
        }

        public override void Encode(string propertyName, SGF_Root_Setting setting)
        {
            List<byte[]> result = new List<byte[]>();
            result.Add(Encoding.ASCII.GetBytes(value_.ToString()));
            encoded_ = result;
        }
    }

    public class SGF_Type_Double : SGF_Type_Number
    {

    }

    public class SGF_Type_Text : SGF_Type_Base
    {
        private string text_ = string.Empty;

        public string Text
        {
            get { return text_; }
        }

        public override void Decode(List<byte[]> values, string propertyName, SGF_Root_Setting setting)
        {
            LogSingleValueVerification(values, propertyName, setting);
            if (values.Count == 1)
            {
                text_ = Util.Escaping(Encoding.ASCII.GetString(values[0]));
            }
        }
        public override void Encode(string propertyName, SGF_Root_Setting setting)
        {
            return;
        }
    }

    public class SGF_Type_SimpleText : SGF_Type_Text
    {

    }

    public class SGF_Type_Point : SGF_Type_Base
    {
        private int x_ = -1;
        private int y_ = -1;

        public int X
        {
            get { return x_; }
            set
            {
                x_ = value;
                isBlank_ = false;
            }
        }

        public int Y
        {
            get { return y_; }
            set
            {
                y_ = value;
                isBlank_ = false;
            }
        }

        public override void Decode(List<byte[]> values, string propertyName, SGF_Root_Setting setting)
        {
            LogSingleValueVerification(values, propertyName, setting);
            if (values.Count == 1)
            {
                if (values[0].Length != 2)
                {
                    setting.PushError(propertyName, "The point type has the incorrect format");
                    return;
                }
                x_ = Util.MoveToInt(values[0][0]);
                y_ = Util.MoveToInt(values[0][1]);
                if (x_ != -1 || y_ != -1)
                {
                    isBlank_ = false;
                }
                else
                {
                    setting.PushError(propertyName, "The point type has the incorrect format");
                }
            }
        }

        public override void Encode(string propertyName, SGF_Root_Setting setting)
        {
            List<byte[]> result = new List<byte[]>();
            result.Add(new byte[2] { Util.IntToMove(x_), Util.IntToMove(y_) });
            encoded_ = result;
        }
    }

    public class SGF_Type_Composed<T, U> : SGF_Type_Base where T : SGF_Type_Base, new() where U : SGF_Type_Base, new()
    {
        private T leftValue_ = null;
        private U rightValue_ = null;

        public T Left
        {
            get { return leftValue_; }
        }

        public U Right
        {
            get { return rightValue_; }
        }

        public override void Decode(List<byte[]> values, string propertyName, SGF_Root_Setting setting)
        {
            LogSingleValueVerification(values, propertyName, setting);
            if (values.Count == 1)
            {
                List<byte[]> oneValue = Util.SplitByteArrayBy(values[0], ':');
                if (oneValue.Count > 0)
                {
                    // Left
                    leftValue_ = new T();
                    List<byte[]> leftvalues = new List<byte[]>();
                    leftvalues.Add(oneValue[0]);
                    leftValue_.Decode(leftvalues, propertyName, setting);
                    isBlank_ = false;
                }
                if (oneValue.Count > 1)
                {
                    // Right
                    rightValue_ = new U();
                    List<byte[]> rightvalues = new List<byte[]>();
                    rightvalues.Add(oneValue[1]);
                    rightValue_.Decode(rightvalues, propertyName, setting);
                }
               
            }
        }

        public override void Encode(string propertyName, SGF_Root_Setting setting)
        {
            encoded_ = new List<byte[]>();
            if (!Left.IsBlank)
            {
                Left.Encode(propertyName, setting);
            }
            if (!Right.IsBlank)
            {
                Right.Encode(propertyName, setting);
            }
            if (Left.Encoded.Count != 0 && Right.Encoded.Count != 0)
            {
                byte[] buffer = new byte[Left.Encoded[0].Length + Right.Encoded[0].Length + 1];
                Buffer.BlockCopy(Left.Encoded[0], 0, buffer, 0, Left.Encoded[0].Length);
                buffer[Left.Encoded[0].Length] = (byte)':';
                Buffer.BlockCopy(Right.Encoded[0], 0, buffer, Left.Encoded[0].Length + 1, Right.Encoded[0].Length);
                encoded_.Add(buffer);
            }
            else if (Left.Encoded.Count != 0)
            {
                encoded_.Add(Left.Encoded[0]);
            }
            else if (Right.Encoded.Count != 0)
            {
                byte[] buffer = new byte[Right.Encoded[0].Length + 1];
                buffer[0] = (byte)':';
                Buffer.BlockCopy(Right.Encoded[0], 0, buffer, 1, Right.Encoded[0].Length);
                encoded_.Add(buffer);
            }
        }
    }

    public class SGF_Type_List<T> : SGF_Type_Base where T : SGF_Type_Base, new()
    {
        private List<T> list_ = new List<T>();

        public override void Decode(List<byte[]> values, string propertyName, SGF_Root_Setting setting)
        {

        }

        public override void Encode(string propertyName, SGF_Root_Setting setting)
        {

        }
    }
}
