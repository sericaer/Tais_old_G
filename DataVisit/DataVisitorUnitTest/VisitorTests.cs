using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataVisit;
//using Parser.Semantic;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace DataVisit.Tests
{
    [TestClass]
    public class VisitorTests
    {
        [TestMethod]
        public void TestVisitGetData()
        {
            TestData data = new TestData()
            {
                a = 12,
                sub = new TestDataSub { a = 14 },
                elems = new List<TestElem>()
                                             {
                                                 new TestElem(){ b= 20},
                                                 new TestElem(){ b= 21}
                                             }
            };
            Visitor.InitVisitMap(typeof(TestData));
            Visitor.SetVisitData(data);

            Assert.AreEqual(Visitor.Get("test.a"), 12);
            Assert.AreEqual(Visitor.Get("test.b"), 13);
            Assert.AreEqual(Visitor.Get("test.sub.a"), 14);

            int i = 0;
            Visitor.Pos pos = null;
            while (Visitor.EnumerateVisit("test.elem", ref pos))
            {
                Assert.AreEqual(Visitor.Get("elem.b", pos), data.elems[i].b);
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