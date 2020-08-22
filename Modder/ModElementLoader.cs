using Parser.Semantic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Syntax;
namespace Modder
{
    public class ModElementLoader
    {
        public static T Load<T>(string fileName, string fileContent)
        {
            try
            {
                var syntaxItem = SyntaxItem.RootParse(fileContent);
                return SemanticParser.DoParser<T>(syntaxItem);
            }
            catch (Exception e)
            {
                throw new Exception($"Parse error in script:{fileName}", e);
            }

        }
    }
}
