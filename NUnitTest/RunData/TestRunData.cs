using DataVisit;
using Modder;
using NUnit.Framework;
using RunData;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestRunData
    {

        [Test()]
        public void TestGetData()
        {
            Root.Init();

            Visitor.InitVisitMap(typeof(Root));
            Visitor.SetVisitData(Root.inst);

            Assert.AreEqual(1, Visitor.Get("date.day"));
            Assert.AreEqual(1, Visitor.Get("date.month"));
            Assert.AreEqual(1, Visitor.Get("date.year"));
        }

        [Test()]
        public void TestSetGetData()
        {
            Root.Init();

            Visitor.InitVisitMap(typeof(Root));
            Visitor.SetVisitData(Root.inst);

            Visitor.Set("economy.value", 123.0);

            Assert.AreEqual(123, Visitor.Get("economy.value"));
        }
    }
}
