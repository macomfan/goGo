using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public class SGF_Property_Entity_Base
    {
        protected static string name_ = string.Empty;
        protected List<string> values_ = new List<string>();

        public static string Name
        {
            get { return name_; }
        }

        public static void SetName(string name)
        {
            name_ = name;
        }

        public void SetValues(List<string> value)
        {
            values_ = new List<string>(value);
        }
    }


    public class SGF_Property_Entity<T> : SGF_Property_Entity_Base where T : SGF_TypeBase, new()
    {
        private T reader_ = null;

        public SGF_Property_Entity()
        {

        }

        public T Reader()
        {
            if (reader_ == null)
            {
                reader_ = new T();
                //reader_.
            }
            return reader_;
        }
    }
}
