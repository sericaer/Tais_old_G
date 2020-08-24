using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modder;
using System.Collections.Generic;
//using Parser.Semantic;
using System.Text.RegularExpressions;

namespace ModderUnitTest
{
    [TestClass]
    public class UnitTestDataVisit
    {
        [TestMethod]
        public void TestVisitGetData()
        {
            TestData data = new TestData() { a = 12,
                                             sub = new TestDataSub { a = 14 },
                                             elems = new List<TestElem>()
                                             {
                                                 new TestElem(){ b= 20},
                                                 new TestElem(){ b= 21}
                                             } };
            DataVisit.InitVisitMap(typeof(TestData));
            DataVisit.SetVisitData(data);

            Assert.AreEqual(DataVisit.Get("test.a"), 12);
            Assert.AreEqual(DataVisit.Get("test.b"), 13);
            Assert.AreEqual(DataVisit.Get("test.sub.a"), 14);

            int i = 0;
            DataVisit.Pos pos = null;
            while (DataVisit.EnumerateVisit("test.elem", ref pos))
            {
                Assert.AreEqual(DataVisit.Get("elem.b", pos), data.elems[i].b);
                i++;
            }
        }
    }

    public class TestData
    {
        [DataVisitorProperty("test.a")]
        public int a;

        [DataVisitorProperty("test.b")]
        public int b
        {
            get
            {
                return a + 1;
            }
        }

        [DataVisitorProperty]
        public TestDataSub sub;

        [DataVisitorPropertyArray("test.elem")]
        public List<TestElem> elems;
    }

    public class TestDataSub
    {
        [DataVisitorProperty("test.sub.a")]
        public int a;
    }

    public class TestElem
    {
        [DataVisitorProperty("elem.b")]
        public int b;
    }
}
