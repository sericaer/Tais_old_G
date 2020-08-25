using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class Select
    {
        internal static Select Parse(SyntaxItem item)
        {
            var rslt = new Select();

            foreach(var elem in item.values)
            {
                var subItem = elem as SyntaxItem;
                if(subItem == null)
                {
                    throw new Exception("must key-value pair in select");
                }
                switch(subItem.key)
                {
                    case "SET":
                        rslt.list.Add(OpSet.Parse(subItem));
                        break;
                    default:
                        throw new Exception($"can not support {subItem.key} in select");
                }
            }

            return rslt;
        }


        public void Do()
        {
            list.ForEach(x => x.Do());
        }

        internal List<OpSet> list = new List<OpSet>();
    }
}
