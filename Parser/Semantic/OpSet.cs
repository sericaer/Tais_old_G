using System;
using Parser.Syntax;

namespace Parser.Semantic
{
    public class OpSet
    {
        internal static OpSet Parse(SyntaxItem subItem)
        {
            throw new NotImplementedException();
        }

        internal void Do()
        {
            Visitor.SetValue(right, Visitor.GetValue(left));
        }

        string right;
        string left;
    }
}
