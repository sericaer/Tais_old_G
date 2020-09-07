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
                    default:
                        throw new Exception($"not support value {value.key}");
                }
            }
            catch(Exception e)
            {
                throw new Exception($"Parse conditon faild! key:{item.key}", e);
            }

        }

        internal Condition(SyntaxItem item)
        {
            if(item == null && this is ConditionDefault)
            {
                return;
            }

            if (item.values.Count() != 2)
            {
                throw new Exception("conditon equal must have 2 values!");
            }

            var value0 = item.values[0] as SingleValue;
            var value1 = item.values[1] as SingleValue;
            if (value0 == null || value1 == null)
            {
                throw new Exception("conditon equal value must be SingleValue");
            }

            right = value0;
            left = value1;
        }

        public abstract bool Rslt();

        internal SingleValue right;
        internal SingleValue left;
    }
}
