using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SGFParser
{
    public class SGF_Node
    {
        private SGF_Node parent_ = null;
        private List<SGF_Node> children_ = new List<SGF_Node>();
        private List<SGF_Property> properties_ = new List<SGF_Property>();

        public SGF_Node GetNextNode()
        {
            if (children_ == null || children_.Count == 0)
            {
                return null;
            }
            return children_[0];
        }

        public void AddNode(SGF_Node node)
        {
            node.parent_ = this;
            children_.Add(node);
        }

        public void AddProperty(SGF_Property property)
        {
            properties_.Add(property);
        }

        public int GetBranchNumber()
        {
            if (children_ != null)
            {
                return children_.Count;
            }
            return 0;
        }
    }
}
