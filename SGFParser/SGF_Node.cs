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
        protected SGF_Node_Root rootNote_ = null;
        private SGF_Node parent_ = null;
        private SGF_Node child_ = null;
        private List<SGF_Node> stepChildren_ = new List<SGF_Node>();
        private Dictionary<String, SGF_Property> properties_ = new Dictionary<String, SGF_Property>();
        private Dictionary<String, SGF_Property_Entity_Base> entities_ = new Dictionary<String, SGF_Property_Entity_Base>();

        public SGF_Node(SGF_Node_Root root)
        {
            rootNote_ = root;
        }

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

        internal virtual void AddProperty(SGF_Property property)
        {
            if (property.Name.Length == 0)
            {
                SGFException.Throw("A Property name is NULL");
            }
            if (properties_.ContainsKey(property.Name))
            {
                SGFException.Throw("Attempt to add duplicate Property");
            }
            property.Setting = rootNote_.Setting;
            properties_.Add(property.Name, property);
        }

        public void SetProperty(SGF_Property_Entity_Base entity)
        {
            SGF_Property property = null;
            if (properties_.ContainsKey(entity.Name))
            {
                property = properties_[entity.Name];
                property.ClearAllValues();
            }
            else
            {
                property = new SGF_Property(entity.Name);
                AddProperty(property);
            }
            List<byte[]> values = entity.ToPropertyValues(rootNote_.Setting);
            foreach (byte[] value in values)
            {
                property.AddValue(value);
            }
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
            T t = new T();
            if (entities_.ContainsKey(t.Name))
            {
                return entities_[t.Name] as T;
            }
            if (!properties_.ContainsKey(t.Name))
            {
                return t;
            }
            t.BindProperty(properties_[t.Name]);
            entities_.Add(t.Name, t);
            return t;
        }
    }
}
