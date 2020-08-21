﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Semantic
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SemanticProperty : Attribute
    {
        public SemanticProperty(string key)
        {
            this.key = key;
        }

        internal string key;
    }
}
