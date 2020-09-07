using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class Date
    {
        [SemanticProperty("year")]
        public int? year;

        [SemanticProperty("month")]
        public int? month;

        [SemanticProperty("day")]
        public int? day;

        public static Date Parse(SyntaxItem item)
        {
            return SemanticParser.DoParser<Date>(item);
        }
    }
}
