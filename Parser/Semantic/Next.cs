using System;
using System.Collections.Generic;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class Next
    {
        public static Next Parse(SyntaxItem item)
        {
            return new Next(item.values);
        }

        public string Rslt()
        {
            foreach(var sub in list)
            {
                if(sub.cond.Rslt())
                {
                    return sub.name;
                }
            }

            return null;
        }

        List<(string name, Condition cond)> list;

        public Next(List<Value> values)
        {
            list = new List<(string name, Condition cond)>();

            foreach (var value in values)
            {
                var subItem = value as SyntaxItem;
                if (subItem == null)
                {
                    throw new Exception($"not support value type");
                }

                list.Add((subItem.key, Condition.Parse(subItem)));
            }
        }
    }
}