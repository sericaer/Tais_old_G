using System;
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

                var bValue = item.values[0] as BoolValue;
                if(bValue != null)
                {
                    return new ConditionDefault(bValue.data);
                }

                var value = item.values[0] as SyntaxItem;
                if (value == null)
                {
                    throw new Exception($"not support value type");
                }

                switch (value.key)
                {
                    case "equal":
                        return new ConditionEqual(value);
                    case "less":
                        return new ConditionLess(value);
                    case "greater":
                        return new ConditionGreater(value);
                    //case "or":
                    //    return new ConditionOr(value);
                    //case "and":
                    //    return new ConditionAnd(value);
                    //case "not":
                    //    return new ConditionNot(value);
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
