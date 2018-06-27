using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SGFParser
{
    class SGFBufferParser
    {
        private byte[] buffer_ = null;
        private int index_ = 0;
        private int length_ = 0;
        private SGF_Node currentNode_ = null;
        private SGF_Property currentProperty_ = null;

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
                char ch = ReadChar();
                if (ch == ';')
                {
                    currentNode = ProcessNote(currentNode);
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

        public SGF_Node ProcessNote(SGF_Node parent)
        {
            SGF_Node newNode = new SGF_Node();
            parent.AddNode(newNode);
            while (!EOF)
            {
                int indexPropertyValueStart = IndexOfNext('[');
                if (indexPropertyValueStart == -1)
                {
                    // Error
                }
                char[] name = ReadChars(indexPropertyValueStart - Index);
                string nameString = new string(name);
                int indexPropertyValueEnd = IndexOfNext(']');
                if (indexPropertyValueEnd == -1)
                {
                    // Error
                }
                char[] value = ReadChars(indexPropertyValueEnd - Index + 1);
                string valueString = new string(value);
                System.Diagnostics.Debug.WriteLine(string.Format("Property: {0} - {1}", nameString, valueString));
                SGF_Property propertry = new SGF_Property(nameString, valueString);
                newNode.AddProperty(propertry);
                char ch = PeekChar();
                if (ch == ';' || ch == '(' || ch == ')')
                {
                    break;
                }
            }
            return newNode;
        }

        private int IndexOfNext(char ch)
        {
            for (int i = index_; i < length_; i++ )
            {
                if (buffer_[i] == ch)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Parse(SGF_Node root)
        {
            while (!EOF)
            {
                char ch = ReadChar();
                if (ch == '(')
                {
                    ProcessTree(root);
                }
            }
        }

        public byte ReadByte()
        {
            return buffer_[index_++];
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
            if (index_ + count >= length_)
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
