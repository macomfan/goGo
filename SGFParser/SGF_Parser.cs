using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace SGFParser
{
    public class SGF_Parser
    {
        private FileStream fileReader_ = null;
        private SGF_Node root_ = null;

        public SGF_Parser()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            string str = version.ToString();
        }

        public SGF_Node GetRoot()
        {
            return root_;
        }

        public void OpenSGF(string filename)
        {
            if (!File.Exists(filename))
            {
                SGFException.Throw("The file: " + filename + "cannot be found");
            }
            fileReader_ = new FileStream(filename, FileMode.Open);
            SGFBufferParser reader = new SGFBufferParser(fileReader_);
            root_ = new SGF_Node();
            reader.Parse(root_);
            //reader.
        }
    }
}
