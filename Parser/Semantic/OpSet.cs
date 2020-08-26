using System;
using System.Collections.Generic;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class OpSet
    {
        public static OpSet Parse(SyntaxItem item)
        {
            var sub = new List<(string right, string left)>();

            foreach(var value in item.values)
            {
                var subItem = value as SyntaxItem;
                if(subItem  == null)
                {
                    throw new Exception("set sub iteam must be key-value pair!");
                }
                if(subItem.values.Count != 1 || !(subItem.values[0] is StringValue))
                {
                    throw new Exception("set sub iteam must be have one StringValue!");
                }

                sub.Add((subItem.key, subItem.values[0].ToString()));
            }

            return new OpSet() { list = sub };
        }

        public void Do()
        {
            list.ForEach(x => Visitor.SetValue(x.right, Visitor.GetValue(x.left)));
        }

        public List<(string right, string left)> list;
    }
}
