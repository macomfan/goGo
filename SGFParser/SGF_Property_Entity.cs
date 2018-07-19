using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public abstract class SGF_Property_Entity_Base
    {
        protected string name_ = string.Empty;
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

        internal void BindProperty(SGF_Property property)
        {
            OnSetValue(property.Values, property.Setting);
        }

        protected abstract void OnSetValue(List<byte[]> values, SGF_Root_Setting setting);

        internal abstract List<byte[]> ToPropertyValues(SGF_Root_Setting setting);
    }


    public class SGF_Property_Entity<T> : SGF_Property_Entity_Base where T : SGF_Type_Base, new()
    {
        private T value_ = null;

        protected SGF_Property_Entity(string name)
            : base(name)
        {
            value_ = new T();
        }

        protected override void OnSetValue(List<byte[]> values, SGF_Root_Setting setting)
        {
            value_.Decode(values, name_, setting);
        }

        internal override List<byte[]> ToPropertyValues(SGF_Root_Setting setting)
        {
            value_.Encode(name_, setting);
            return value_.Encoded;
        }

        public T Value
        {
            get
            {
                return value_;
            }
            set 
            {
                value_ = value;
                isBlank_ = false;
            }
        }
    }

    public class SGF_Property_C : SGF_Property_Entity<SGF_Type_SimpleText>
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

    public class SGF_Property_CA : SGF_Property_Entity<SGF_Type_SimpleText>
    {
        public SGF_Property_CA()
            : base("CA")
        {

        }
    }

    public class SGF_Property_GM : SGF_Property_Entity<SGF_Type_Number>
    {
        public SGF_Property_GM()
            : base("GM")
        {
        }
    }

    public class SGF_Property_ST : SGF_Property_Entity<SGF_Type_Number>
    {
        public SGF_Property_ST()
            : base("ST")
        {
        }
    }

    public class SGF_Property_SZ : SGF_Property_Entity<SGF_Type_Composed<SGF_Type_Number, SGF_Type_Number>>
    {
        public SGF_Property_SZ()
            : base("SZ")
        {
        }
    }
// 
//     public class SGF_Property_W : SGF_Property_Entity<string>
//     {
//         public SGF_Property_W()
//             : base("W", SGF_Type_Convertor.Text.Decode, SGF_Type_Convertor.Text.Encode)
//         {
// 
//         }
//     }
}
