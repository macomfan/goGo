using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SGFParser
{
    internal class SGFException
    {
        public static void Throw(String message)
        {
            throw new Exception("SGFParser: " + message);
        }
    }
}
