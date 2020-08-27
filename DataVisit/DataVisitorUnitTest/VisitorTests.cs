﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataVisit;
//using Parser.Semantic;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

namespace DataVisit.Tests
{
    [TestClass]
    public class VisitorTests
    {
        [TestMethod]
        public void TestVisitGetData()
        {
            TestData.inst = new TestData()
            {
                sub = new TestDataSub { a = 14 },
                elems = new List<TestElem>()
                                             {
                                                 new TestElem(){ b= 20},
                                                 new TestElem(){ b= 21}
                                             }
            };

            Visitor.InitVisitMap(new List<Type> { typeof(TestData),  typeof(TestDataSub), typeof(TestElem) });
            Visitor.SetVisitData(TestData.inst);

            Assert.AreEqual(Visitor.Get("sub.a"), 14);

            Visitor.Set("sub.a", 22);
            Assert.AreEqual(Visitor.Get("sub.a"), 22);

            Assert.AreEqual(Visitor.Get("sub.c"), 23);
            Assert.AreEqual(Visitor.Get("sub.elem.b"), 20);

            int i = 0;
            Visitor.Pos pos = null;
            while (Visitor.EnumerateVisit("elem", ref pos))
            {
                Assert.AreEqual(Visitor.Get("elem.b"), TestData.inst.elems[i].b);
                i++;
            }
        }
    }

    public class TestData
    {
        public static TestData inst;

        [DataVisitorProperty("sub")]
        public TestDataSub sub;

        [DataVisitorPropertyArray("elem")]
        public List<TestElem> elems;
    }

    public class TestDataSub
    {
        [DataVisitorProperty("a")]
        public int a;

        [DataVisitorProperty("c")]
        public int c
        {
            get
            {
                return a + 1;
            }
        }

        [DataVisitorProperty("elem")]
        public TestElem elem
        {
            get
            {
                return TestData.inst.elems[0];
            }
        }
    }

    public class TestElem
    {
        [DataVisitorProperty("b")]
        public int b;
    }
}