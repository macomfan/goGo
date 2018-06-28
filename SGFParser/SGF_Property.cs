using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public class SGF_Property
    {
        private string name_ = string.Empty;
        private List<string> values_ = new List<string>();

        public string Name
        {
            get { return name_; }
        }

        public SGF_Property(string name)
        {
            name_ = name;
        }

        public void AddValue(string value)
        {
            values_.Add(value);
        }

        public List<string> Values
        {
            get { return values_; }
        }
    }
}
