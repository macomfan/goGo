﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GoModel
{
    public class GoException
    {
        public static void Throw(String message)
        {
            throw new Exception("GoData: " + message);
        }
    }
}
