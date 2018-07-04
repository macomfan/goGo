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
        private SGF_Node child_ = null;
        private List<SGF_Node> stepChildren_ = new List<SGF_Node>();
        private Dictionary<String, SGF_Property> properties_ = new Dictionary<String, SGF_Property>();
        private Dictionary<String, SGF_Property_Entity_Base> entities_ = new Dictionary<String, SGF_Property_Entity_Base>();

        public SGF_Node Parent
        {
            get { return parent_; }
        }

        public SGF_Node Child
        {
            get { return child_; }
        }

        public ReadOnlyCollection<SGF_Node> StepChildren
        {
            get { return stepChildren_.AsReadOnly(); }
        }

        public void AddNode(SGF_Node node)
        {
            node.parent_ = this;
            if (child_ == null)
            {
                child_ = node;
            }
            else
            {
                stepChildren_.Add(node);
            }
        }

        public void AddProperty(SGF_Property property)
        {
            if (property.Name.Length == 0)
            {
                // SGFException.Throw("A Property name is NULL");
            }
            if (properties_.ContainsKey(property.Name))
            {
                SGFException.Throw("Attempt to add duplicate Property");
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

        public T GetProperty<T>() where T : SGF_Property_Entity_Base, new()
        {
            T t = new T();
            if (entities_.ContainsKey(t.Name))
            {
                return entities_[t.Name] as T;
            }
            if (!properties_.ContainsKey(t.Name))
            {
                return t;
            }
            t.SetValues(properties_[t.Name].Values);
            entities_.Add(t.Name, t);
            return t;
        }
    }
}
