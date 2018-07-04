using System;
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

    public abstract class SGF_TypeBase
    {
        protected bool isBlank_ = false;

        protected SGF_Property_Entity_Base property_ = null;

        protected abstract void OnSetValue();

        public void SetProperty(SGF_Property_Entity_Base property)
        {
            property_ = property;
            OnSetValue();
        }

        public bool IsBlank
        {
            get { return isBlank_; }
        }
    }

    public class SGF_Type_Double : SGF_TypeBase
    {
        private int value_ = 0;

        public SGF_Type_Double()
        {

        }

        protected override void OnSetValue()
        {
            if (property_.Values.Count > 1)
            {
                SGFException.Throw(string.Format("The Double type has multi values in {0}", property_.Name));
            }
            if (property_.Values.Count != 0 )
            {
                value_ = int.Parse(property_.Values[0]);
                isBlank_ = false;
            }
        }

        public int Value
        {
            get
            {
                if (isBlank_)
                {
                    SGFException.Throw("Attempt to query a blank property");
                }
                return value_;
            }
        }
    }

    public class SGF_Type_Number : SGF_TypeBase
    {
        private int value_ = 0;

        public SGF_Type_Number()
        {

        }

        protected override void OnSetValue()
        {
            if (property_.Values.Count > 1)
            {
                SGFException.Throw(string.Format("The Number type has multi values in {0}", property_.Name));
            }
            if (property_.Values.Count != 0)
            {
                value_ = int.Parse(property_.Values[0]);
                isBlank_ = false;
            }
        }

        public int Value
        {
            get
            {
                if (isBlank_)
                {
                    SGFException.Throw("Attempt to query a blank property");
                }
                return value_;
            }
        }
    }

    public class SGF_Type_SimpleText : SGF_TypeBase
    {
        private string value_ = string.Empty;

        public SGF_Type_SimpleText()
        {

        }

        protected override void OnSetValue()
        {
            if (property_.Values.Count > 1)
            {
                SGFException.Throw(string.Format("The SimpleText type has multi values in {0}", property_.Name));
            }
            if (property_.Values.Count != 0)
            {
                value_ = TextUtil.Escaping(property_.Values[0]);
                isBlank_ = false;
            }
        }

        public string Value
        {
            get
            {
                if (isBlank_)
                {
                    SGFException.Throw("Attempt to query a blank property");
                }
                return value_;
            }
        }
    }

    public class SGF_Type_Text : SGF_TypeBase
    {
        private string value_ = string.Empty;

        public SGF_Type_Text()
        {

        }

        protected override void OnSetValue()
        {
            if (property_.Values.Count > 1)
            {
                SGFException.Throw(string.Format("The Text type has multi values in {0}", property_.Name));
            }
            if (property_.Values.Count != 0)
            {
                value_ = TextUtil.Escaping(property_.Values[0]);
                isBlank_ = false;
            }
        }

        public string Value
        {
            get
            {
                if (isBlank_)
                {
                    SGFException.Throw("Attempt to query a blank property");
                }
                return value_;
            }
        }
    }
}
