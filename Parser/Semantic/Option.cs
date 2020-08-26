using Parser.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.Semantic
{
    public class Option
    {
        [SemanticProperty("desc")]
        public GroupValue desc;

        [SemanticProperty("set")]
        public OpSet set;

        public static Option Parse(SyntaxItem item)
        {
            return SemanticParser.DoParser<Option>(item);

            //if (item.values.Count() != 1)
            //{
            //    throw new Exception($"option values count must be 1, but real is {item.values.Count()}");
            //}

            //var itemValue = item.values[0] as SyntaxItem;
            //if(itemValue == null)
            //{
            //    throw new Exception($"item format error, must be key-value, raw:${itemValue}");
            //}

            //itemValue.values.ForEach(x =>
            //{
            //    var subItem = x as SyntaxItem;
            //    if (subItem == null)
            //    {
            //        throw new Exception($"item format error, must be key-value, raw:${x}");
            //    }

            //    if(subItem.key == "desc")
            //    {

            //    }
            //});

        }
    }
}
