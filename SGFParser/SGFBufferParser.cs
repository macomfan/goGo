using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SGFParser
{
    public class SGFBufferParser
    {
        private byte[] buffer_ = null;
        private int index_ = 0;
        private int length_ = 0;

        public SGFBufferParser(Stream stream)
        {
            if (stream.Length > int.MaxValue)
            {
                SGFException.Throw("The file is too large, not supported");
            }
            buffer_ = new byte[stream.Length];
            length_ = (int)stream.Length;
            index_ = 0;
            stream.Read(buffer_, 0, (int)stream.Length);
        }

        public void ProcessTree(SGF_Node parent)
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
                char[] value = ReadChars(indexPropertyValueEnd - Index + 1);
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

        public SGF_Node ProcessNode(SGF_Node parent)
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
                char[] name = ReadChars(indexPropertyValueStart - Index);
                string nameString = new string(name);
                SGF_Property propertry = new SGF_Property(nameString);
                ProcessPropertyValue(propertry);
                newNode.AddProperty(propertry);
            }
            return newNode;
        }

        public void Parse(SGF_Node root)
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

        public char ReadToNextControlChar()
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

        public char ReadChar()
        {
            return (char)buffer_[index_++];
        }

        public char PeekChar()
        {
            return (char)buffer_[index_];
        }

        public char[] ReadChars(int count)
        {
            if (index_ + count > length_)
            {
                SGFException.Throw("out of bounds");
            }
            char[] chars = new char[count];
            for (int i = 0; i < count; i++ )
            {
                chars[i] = (char)buffer_[i + index_];
            }
            index_ += count;
            return chars;
        }

        public bool EOF
        {
            get { return index_ >= length_; }
        }

        public int Index
        {
            get { return index_; }
        }
    }
}
