﻿using System;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class OperationAssign : Operation
    {
        internal static Operation Parse(SyntaxItem subItem)
        {
            if (subItem.values.Count != 2
                || !(subItem.values[0] is StringValue)
                || !(subItem.values[1] is SingleValue))
            {
                throw new Exception("add sub iteam must be have one SingleValue!");
            }

            return new OperationAssign() { left = subItem.values[0] as SingleValue, right = subItem.values[1] as SingleValue };
        }

        public override void Do()
        {
            Visitor.SetValue(left.ToString(), right);
        }
    }
}