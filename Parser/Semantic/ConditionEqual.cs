using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class ConditionEqual : Condition
    {
        internal static new ConditionEqual Parse(SyntaxItem item)
        {
            if(item.values.Count() != 2)
            {
                throw new Exception("conditon equal must have 2 values!");
            }

            var value0 = item.values[0] as SingleValue;
            var value1 = item.values[1] as SingleValue;
            if (value0 == null || value1 == null)
            {
                throw new Exception("conditon equal value must be SingleValue");
            }

            return new ConditionEqual() { right = value0, left = value1 };
        }


        public override bool Rslt()
        {
            return Visitor.GetValue(right) == Visitor.GetValue(left);
        }

        public SingleValue right;
        public SingleValue left;
    }
}
