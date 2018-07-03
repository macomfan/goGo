using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;

namespace SGFParser
{
    public class SGF_Node
    {
        private SGF_Node parent_ = null;
        private List<SGF_Node> children_ = new List<SGF_Node>();
        private Dictionary<String, SGF_Property> properties_ = new Dictionary<String, SGF_Property>();

        public SGF_Node Parent
        {
            get { return parent_; }
        }

        public ReadOnlyCollection<SGF_Node> Children
        {
            get { return children_.AsReadOnly(); }
        }

        public void AddNode(SGF_Node node)
        {
            node.parent_ = this;
            children_.Add(node);
        }

        public void AddProperty(SGF_Property property)
        {
            if (property.Name.Length == 0)
            {
                // SGFException.Throw("A Property name is NULL");
            }
            if (properties_.ContainsKey(property.Name))
            {
                // SGFException.Throw("Attempt to add duplicate Property");
            }
            properties_.Add(property.Name, property);
        }

        public ReadOnlyCollection<SGF_Property> Properties
        {
            get
            {
                return properties_.Values.ToList().AsReadOnly();
            }
        }

        public T GetPropertyAs<T>() where T : SGF_Property_Entity_Base, new()
        {
            if (!properties_.ContainsKey(SGF_Property_Entity_Base.Name))
            {
                return null;
            }
            T t = new T();
            t.SetValues(properties_[SGF_Property_Entity_Base.Name].Values);
            return t;
        }
    }
}
