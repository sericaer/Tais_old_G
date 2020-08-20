using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    class ConditionEqual : Condition
    {
        internal static new ConditionEqual Parse(SyntaxItem item)
        {
            if(item.values.Count() != 2)
            {
                throw new Exception("conditon equal must have 2 values!");
            }

            var value0 = item.values[0] as StringValue;
            var value1 = item.values[1] as StringValue;
            if (value0 == null || value1 == null)
            {
                throw new Exception("conditon equal value must be string or digit");
            }

            return new ConditionEqual() { right = new DataVisit(value0.ToString()), left = new DataVisit(value1.ToString()) };
        }


        public override bool Rslt()
        {
            return right.Get() == left.Get();
        }

        internal DataVisit right;
        internal DataVisit left;
    }
}
