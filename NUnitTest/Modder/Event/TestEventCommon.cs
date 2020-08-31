using Modder;
using NUnit.Framework;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Event
{
    [TestFixture()]
    public class TestEventCommon
    {
        public TestEventCommon()
        {
            ModDataVisit.InitVisitMap(typeof(Demon));

            ModFileSystem.Clear();

            var modFileSystem = ModFileSystem.Generate(nameof(TestEventCommon));
            modFileSystem.AddCommonEvent("EVENT_TEST.txt",
@"title = EVENT_DIFF_TITLE
desc = EVENT_DIFF_DESC

trigger =
{
	EQUAL = {item1.data1, 11}
}

occur = 1

option =
{
    desc = EVENT_TEST_OPTION_1_DESC
    select =
    {
        assign = {item1.data2, 101}
    }
}

option =
{
    desc = EVENT_TEST_OPTION_2_DESC
    select =
    {
        assign = {item1.data2, 102}
    }
}

option =
{
    desc = EVENT_TEST_OPTION_3_DESC
    select =
    {
        assign = {item1.data2, 103}
    }
}");

            modFileSystem.AddCommonEvent("EVENT_TEST_DEFAULT.txt",
@"

trigger =
{
	EQUAL = {item1.data1, 12}
}

occur = 1

option =
{
    select =
    {
        add = {item1.data2, 0}
    }
}

option =
{
    select =
    {
        add = {item1.data2, 1}
    }
}

option =
{
    select =
    {
        add = {item1.data2, 2}
    }
}");

            Mod.Load(ModFileSystem.path);
        }

        [SetUpFixture]
        public class TestSetup
        {
            public TestSetup()
            {

            }
        }

        [SetUp]
        public void Setup()
        {
            ModDataVisit.InitVisitData(Demon.Init());
        }

        [Test()]
        public void TestEventNotTrigger()
        {
            Demon.inst.item1.data1 = 10;

            foreach(var eventobj in Mod.Process())
            {
                Assert.Fail();
            }
        }

        [Test()]
        public void TestEventBase()
        {

            Demon.inst.item1.data1 = 11;

            var eventobjs = Mod.Process().ToArray();

            Assert.AreEqual(eventobjs.Count(), 1);

            var eventobj = eventobjs[0];

            Assert.AreEqual(eventobj.title.Format, "EVENT_DIFF_TITLE");
            Assert.AreEqual(eventobj.desc.Format, "EVENT_DIFF_DESC");

            Assert.AreEqual(eventobj.options.Length, 3);
            Assert.AreEqual(eventobj.options[0].desc.Format, "EVENT_TEST_OPTION_1_DESC");
            Assert.AreEqual(eventobj.options[1].desc.Format, "EVENT_TEST_OPTION_2_DESC");
            Assert.AreEqual(eventobj.options[2].desc.Format, "EVENT_TEST_OPTION_3_DESC");

            eventobj.options[0].Selected();
            Assert.AreEqual(101, Demon.inst.item1.data2);

            eventobj.options[1].Selected();
            Assert.AreEqual(102, Demon.inst.item1.data2);

            eventobj.options[2].Selected();
            Assert.AreEqual(103, Demon.inst.item1.data2);

        }

        [Test()]
        public void TestEventCommonDefaultAndAdd()
        {

            Demon.inst.item1.data1 = 12;
            Demon.inst.item1.data2 = 101;

            var eventobjs = Mod.Process().ToArray();

            Assert.AreEqual(eventobjs.Count(), 1);

            var eventobj = eventobjs[0];

            Assert.AreEqual(eventobj.title.Format, "EVENT_TEST_DEFAULT_TITLE");
            Assert.AreEqual(eventobj.desc.Format, "EVENT_TEST_DEFAULT_DESC");

            Assert.AreEqual(eventobj.options.Length, 3);
            Assert.AreEqual(eventobj.options[0].desc.Format, "EVENT_TEST_DEFAULT_OPTION_1_DESC");
            Assert.AreEqual(eventobj.options[1].desc.Format, "EVENT_TEST_DEFAULT_OPTION_2_DESC");
            Assert.AreEqual(eventobj.options[2].desc.Format, "EVENT_TEST_DEFAULT_OPTION_3_DESC");

            eventobj.options[0].Selected();
            Assert.AreEqual(101, Demon.inst.item1.data2);

            eventobj.options[1].Selected();
            Assert.AreEqual(102, Demon.inst.item1.data2);

            eventobj.options[2].Selected();
            Assert.AreEqual(104, Demon.inst.item1.data2);

        }
    }
}
