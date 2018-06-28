using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public class SGF_Property
    {
        private string name_ = string.Empty;
        private string value_ = string.Empty;

        public string Name
        {
            get { return name_; }
        }

        public string Value
        {
            get { return value_; }
        }

        public SGF_Property(string name, string value)
        {
            name_ = name;
            value_ = value;
        }
    }
}
