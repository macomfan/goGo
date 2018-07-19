using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    public class SGF_Node_Root : SGF_Node
    {
        private SGF_Root_Setting setting_ = new SGF_Root_Setting();

        public SGF_Node_Root()
            : base(null)
        {
            rootNote_ = this;
        }

        internal SGF_Root_Setting Setting
        {
            get { return setting_; }
        }

        internal override void AddProperty(SGF_Property property)
        {
            base.AddProperty(property);
            setting_.FilterProperty(property);
        }
    }
}
