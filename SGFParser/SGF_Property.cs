﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public class SGF_Property
    {
        private string name_ = string.Empty;
        private List<byte[]> values_ = new List<byte[]>();

        public string Name
        {
            get { return name_; }
        }

        public SGF_Property(string name)
        {
            name_ = name;
        }

        internal void AddValue(byte[] value)
        {
            values_.Add(value);
        }

        public List<byte[]> Values
        {
            get { return values_; }
        }
    }
}
