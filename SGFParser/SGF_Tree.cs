using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace SGFParser
{
    public class SGF_Tree
    {
        private SGF_Node root_ = null;

        private byte[] buffer_ = null;
        private int index_ = 0;
        private int length_ = 0;

        public SGF_Tree()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string str = version.ToString();
        }

        public SGF_Node GetRoot()
        {
            return root_;
        }

        public void SaveSGF(string filename)
        {
            FileStream filewriter = new FileStream(filename, FileMode.OpenOrCreate);
            List<byte> result = new List<byte>();
            result.AddRange(PersistTree(root_.Child));
            foreach (SGF_Node node in root_.StepChildren)
            {
                result.AddRange(PersistTree(node));
            }
            filewriter.Write(result.ToArray(), 0, result.Count);
        }

        private List<byte> PersistTree(SGF_Node node)
        {
            List<byte> result = new List<byte>();
            result.Add((byte)'(');
            result.AddRange(PersistNode(node));
            foreach (SGF_Node stepNode in node.StepChildren)
            {
                result.AddRange(PersistTree(stepNode));
            }
            result.Add((byte)')');
            result.Add((byte)'\n');
            return result;
        }


        private List<byte> PersistNode(SGF_Node node)
        {
            List<byte> result = new List<byte>();
            result.Add((byte)';');
            foreach (SGF_Property property in node.Properties)
            {
                result.AddRange(System.Text.Encoding.ASCII.GetBytes(property.Name));
                result.Add((byte)'[');
                result.Add((byte)']');
            }
            if (node.StepChildren.Count == 0)
            {
                if (node.Child != null)
                {
                    result.AddRange(PersistNode(node.Child));
                }
            }
            else
            {
                result.AddRange(PersistTree(node.Child));
                foreach (SGF_Node stepNode in node.StepChildren)
                {
                    result.AddRange(PersistTree(stepNode));
                }
            }
            return result;
        }

#region Reader
        public void OpenSGF(string filename)
        {
            if (!File.Exists(filename))
            {
                SGFException.Throw("The file: " + filename + "cannot be found");
            }
            FileStream fileReader = new FileStream(filename, FileMode.Open);

            if (fileReader.Length > int.MaxValue)
            {
                SGFException.Throw("The file is too large, not supported");
            }
            buffer_ = new byte[fileReader.Length];
            length_ = (int)fileReader.Length;
            index_ = 0;
            fileReader.Read(buffer_, 0, length_);
            root_ = new SGF_Node();
            Parse(root_);
            buffer_ = null;
        }

        private void ProcessTree(SGF_Node parent)
        {
            SGF_Node currentNode = parent;
            while (!EOF)
            {
                char ch = ReadToNextControlChar();
                if (ch == ';')
                {
                    currentNode = ProcessNode(currentNode);
                }
                else if (ch == '(')
                {
                    ProcessTree(currentNode);
                }
                else if (ch == ')')
                {
                    //End Tree
                    break;
                }
            }
        }

        private int FindNextPropertyValueEnd()
        {
            for (int i = index_; i < length_; i++)
            {
                char ch = (char)buffer_[i];
                if (ch == '\\')
                {
                    if (i + 1 < length_)
                    {
                        char chAdd1 = (char)buffer_[i + 1];
                        if (chAdd1 == ']')
                        {
                            i++;
                            continue;
                        }
                    }
                }
                else if (ch == ']')
                {
                    return i;
                }
            }
            return -1;
        }

        private void ProcessPropertyValue(SGF_Property property)
        {
            if (PeekChar() != '[')
            {
                return;
            }
            bool isContinue = true;
            do
            {
                int indexPropertyValueEnd = FindNextPropertyValueEnd();
                char[] value = ReadChars(indexPropertyValueEnd - index_ + 1);
                string valueString = new string(value);
                property.AddValue(valueString);
                while (!EOF)
                {
                    char ch = PeekChar();
                    if (IsUnusedChar(ch))
                    {
                        ReadChar();
                        continue;
                    }
                    else if (ch != '[' || IsControlChar(ch))
                    {
                        isContinue = false;
                    }
                    break;
                }
            } while (!EOF && isContinue);
            return;
        }

        private SGF_Node ProcessNode(SGF_Node parent)
        {
            SGF_Node newNode = new SGF_Node();
            parent.AddNode(newNode);
            while (!EOF)
            {
                char ch = PeekChar();
                if (IsControlChar(ch))
                {
                    break;
                }
                else if (IsUnusedChar(ch))
                {
                    ReadChar();
                    continue;
                }
                int indexPropertyValueStart = FindToPropertyValueStart();
                if (indexPropertyValueStart == -1)
                {
                    break;
                }
                char[] name = ReadChars(indexPropertyValueStart - index_);
                string nameString = new string(name);
                SGF_Property propertry = new SGF_Property(nameString);
                ProcessPropertyValue(propertry);
                newNode.AddProperty(propertry);
            }
            return newNode;
        }

        private void Parse(SGF_Node root)
        {
            while (!EOF)
            {
                char ch = ReadToNextControlChar();
                if (ch == '(')
                {
                    ProcessTree(root);
                }
                else
                {
                    // Error
                }
            }
        }

        private int FindToPropertyValueStart()
        {
            for (int i = index_; i < length_; i++)
            {
                char ch = (char)buffer_[i];
                if (IsControlChar(ch))
                {
                    return -1;
                }
                else if (ch == '[')
                {
                    return i;
                }
            }
            return -1;
        }

        private bool IsUnusedChar(char ch)
        {
            if (ch == ' ' || ch == (char)0x0D || ch == (char)0x0A)
            {
                return true;
            }
            return false;
        }

        private bool IsControlChar(char ch)
        {
            if (ch == '(' || ch == ';' || ch == ')')
            {
                return true;
            }
            return false;
        }

        private char ReadToNextControlChar()
        {
            for (int i = index_; i < length_; i++)
            {
                char ch = (char)buffer_[index_++];
                if (IsControlChar(ch))
                {
                    return ch;
                }
            }
            return ' ';
        }

        private char ReadChar()
        {
            return (char)buffer_[index_++];
        }

        private char PeekChar()
        {
            return (char)buffer_[index_];
        }

        private char[] ReadChars(int count)
        {
            if (index_ + count > length_)
            {
                SGFException.Throw("out of bounds");
            }
            char[] chars = new char[count];
            for (int i = 0; i < count; i++)
            {
                chars[i] = (char)buffer_[i + index_];
            }
            index_ += count;
            return chars;
        }
#endregion

        private bool EOF
        {
            get { return index_ >= length_; }
        }
    }
}
