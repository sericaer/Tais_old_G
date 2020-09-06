﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public abstract class Condition
    {
        public static Condition Parse(SyntaxItem item)
        {
            try
            {
                if (item.values.Count() != 1)
                {
                    throw new Exception($"have more than one values");
                }

                var strValue = item.values[0] as StringValue;
                if(strValue != null)
                {
                    bool bValue;
                    if(!bool.TryParse(strValue.ToString(), out bValue))
                    {
                        throw new Exception($"key {item.key} must have single value true or false");
                    }

                    return new ConditionDefault(bValue);
                }

                var value = item.values[0] as SyntaxItem;
                if (value == null)
                {
                    throw new Exception($"not support value type");
                }

                switch (value.key)
                {
                    case "EQUAL":
                        return ConditionEqual.Parse(value);
                    default:
                        throw new Exception($"not support value {value.key}");
                }
            }
            catch(Exception e)
            {
                throw new Exception($"Parse conditon faild! key:{item.key}", e);
            }

        }

        public abstract bool Rslt();
    }
}
