using Parser.Syntax;
using System;
using Xunit;
using Parser.Semantic;
using Parser.Syntax;

namespace XUnitText_Parser
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            string raw = @"title = EVENT_TEST_TITLE
desc = EVENT_TEST_DESC

option =
{
	desc = EVENT_TEST_OPTION_1_DESC
}";

            var syntaxItem = SyntaxItem.RootParse(raw);
            SemanticParser.DoParser<Modder.GEvent>(syntaxItem);
        }
    }
}
