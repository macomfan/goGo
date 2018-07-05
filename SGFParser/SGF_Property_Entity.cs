using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public abstract class SGF_Property_Entity_Base
    {
        protected string name_ = string.Empty;
        protected SGF_Property bindedProperty_ = null;
        protected List<byte[]> values_ = new List<byte[]>();
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

        public SGF_Property BindedProperty
        {
            get { return bindedProperty_; }
        }

        internal void BindProperty(SGF_Property property)
        {
            values_ = property.Values;
            OnSetValue();
        }

        protected abstract void OnSetValue();
    }


    public class SGF_Property_Entity<T> : SGF_Property_Entity_Base
    {
        private T value_ = default(T);

        protected delegate T Decoder(List<byte[]> values, string name);
        protected Decoder decoder_;
        protected delegate List<byte[]> Encoder(T value, string name);
        protected Encoder encoder_;

        protected SGF_Property_Entity(string name, Decoder d, Encoder e)
            : base(name)
        {
            decoder_ = d;
            encoder_ = e;
        }

        protected override void OnSetValue()
        {
            value_ = decoder_(values_, name_);
            isBlank_ = false;
        }

        protected void OnValueChange()
        {
            if (bindedProperty_ == null)
            {
                return;
            }
            bindedProperty_.Values.Clear();
            bindedProperty_.Values.AddRange(encoder_(value_, name_));
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
            set 
            {
                value_ = value;
                isBlank_ = false;
                OnValueChange();
            }
        }
    }

    public class SGF_Property_C : SGF_Property_Entity<string>
    {
        public SGF_Property_C()
            : base("C", SGF_Type_Convertor.SimpleText.Decode, SGF_Type_Convertor.SimpleText.Encode)
        {
        }
    }

    public class SGF_Property_FF : SGF_Property_Entity<int>
    {
        public SGF_Property_FF()
            : base("FF", SGF_Type_Convertor.Number.Decode, SGF_Type_Convertor.Number.Encode)
        {
        }
    }

    public class SGF_Property_AP : SGF_Property_Entity<string>
    {
        public SGF_Property_AP()
            : base("AP", SGF_Type_Convertor.SimpleText.Decode, SGF_Type_Convertor.SimpleText.Encode)
        {
            
        }
    }

    public class SGF_Property_W : SGF_Property_Entity<string>
    {
        public SGF_Property_W()
            : base("W", SGF_Type_Convertor.Text.Decode, SGF_Type_Convertor.Text.Encode)
        {

        }
    }
}
