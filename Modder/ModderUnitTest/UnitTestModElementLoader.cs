using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modder;
using Parser.Semantic;
using System.Text.RegularExpressions;

namespace ModderUnitTest
{
    [TestClass]
    public class UnitTestModElementLoader
    {
        [TestMethod]
        public void TestLoadEvent()
        {
            string test = @"desc=EVENT_TEST_OPTION_1_DESC
}";

            var rslt = Regex.Match(test, @"^[A-Za-z0-9_\.\+\-\*/%]+=");
            if (rslt.Success)
            {

            }

            string raw = @"title = EVENT_TEST_TITLE
desc = EVENT_TEST_DESC

option =
{
    desc = EVENT_TEST_OPTION_1_DESC
}";
            Visitor.GetValue = DataVisit.Get;

            var eventobj = ModElementLoader.Load<GEvent>("test", raw);
            Assert.AreEqual(eventobj.title.Format, "EVENT_TEST_TITLE");
            Assert.AreEqual(eventobj.desc.Format, "EVENT_TEST_DESC");
        }
    }
}
