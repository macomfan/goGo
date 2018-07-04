using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public class SGF_Property_Entity_Base
    {
        protected string name_ = string.Empty;
        protected List<string> values_ = new List<string>();

        public SGF_Property_Entity_Base(string name)
        {
            name_ = name;
        }

        public string Name
        {
            get { return name_; }
        }

        public List<string> Values
        {
            get { return values_; }
        }

        public bool IsBlank
        {
            get
            {
                return values_.Count == 0;
            }
        }

        public void SetValues(List<string> values)
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
        }
    }


    public class SGF_Property_Entity<T> : SGF_Property_Entity_Base where T : SGF_TypeBase, new()
    {
        private T reader_ = null;

        public SGF_Property_Entity(string name) : base(name)
        {

        }

        public T Reader
        {
            get
            {
                if (reader_ == null)
                {
                    reader_ = new T();
                    reader_.SetProperty(this);
                }
                return reader_;
            }
        }
    }

    public class SGF_Property_C : SGF_Property_Entity<SGF_Type_Text>
    {
        public SGF_Property_C()
            : base("C")
        {

        }
    }

    public class SGF_Property_FF : SGF_Property_Entity<SGF_Type_Number>
    {
        public SGF_Property_FF()
            : base("FF")
        {

        }
    }

    public class SGF_Property_AP : SGF_Property_Entity<SGF_Type_SimpleText>
    {
        public SGF_Property_AP()
            : base("AP")
        {

        }
    }

    public class SGF_Property_W : SGF_Property_Entity<SGF_Type_Text>
    {
        public SGF_Property_W()
            : base("W")
        {

        }
    }
}
