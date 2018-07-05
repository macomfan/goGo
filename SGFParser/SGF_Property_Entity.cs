using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public abstract class SGF_Property_Entity_Base
    {
        protected string name_ = string.Empty;
        protected List<string> values_ = new List<string>();
        protected bool isBlank_ = true;

        public SGF_Property_Entity_Base(string name)
        {
            name_ = name;
        }

        public string Name
        {
            get { return name_; }
        }

        public bool IsBlank
        {
            get
            {
                return isBlank_;
            }
        }

        internal void SetValues(List<string> values)
        {
            List<string> contents = new List<string>();
            foreach (string value in values)
            {
                int start = value.IndexOf('[');
                int end = value.LastIndexOf(']');
                if (start == 0 && end == value.Length - 1)
                {
                    contents.Add(value.Substring(start + 1, end - start - 1));
                }
            }
            values_ = new List<string>(contents);
            OnSetValue();
        }

        protected abstract void OnSetValue();
    }


    public class SGF_Property_Entity<T> : SGF_Property_Entity_Base
    {
        private T value_ = default(T);
        protected delegate T Convertor(List<string> values, string name);
        protected Convertor convertor_;
        protected SGF_Property_Entity(string name, Convertor c) : base(name)
        {
            convertor_ = c;
        }

        protected override void OnSetValue()
        {
            value_ = convertor_(values_, name_);
            isBlank_ = false;
        }

        public T Value
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

    public class SGF_Property_C : SGF_Property_Entity<string>
    {
        public SGF_Property_C()
            : base("C", SGF_Type_Convertor.ConvertFromSimpleText)
        {
        }
    }

    public class SGF_Property_FF : SGF_Property_Entity<int>
    {
        public SGF_Property_FF()
            : base("FF", SGF_Type_Convertor.ConvertFromNumber)
        {
        }
    }

    public class SGF_Property_AP : SGF_Property_Entity<string>
    {
        public SGF_Property_AP()
            : base("AP", SGF_Type_Convertor.ConvertFromSimpleText)
        {

        }
    }

    public class SGF_Property_W : SGF_Property_Entity<string>
    {
        public SGF_Property_W()
            : base("W", SGF_Type_Convertor.ConvertFromText)
        {

        }
    }
}
