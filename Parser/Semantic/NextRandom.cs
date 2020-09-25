using System;
using System.Collections.Generic;
using System.Linq;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class NextRandom
    {
        public static NextRandom Parse(SyntaxItem item)
        {
            return new NextRandom(item.values);
        }

        public IEnumerable<(string name, double value)> Calc()
        {
            return list.Select(x => (x.name, x.modifier.CalcValue()));
        }

        List<(string name, ModifierGroup modifier)> list;

        public NextRandom(List<Value> values)
        {
            list = new List<(string name, ModifierGroup cond)>();

            foreach (var value in values)
            {
                var subItem = value as SyntaxItem;
                if (subItem == null)
                {
                    throw new Exception($"not support value type");
                }

                list.Add((subItem.key, ModifierGroup.Parse(subItem)));
            }
        }
    }
}