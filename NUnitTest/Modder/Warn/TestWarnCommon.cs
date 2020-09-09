using Modder;
using NUnit.Framework;
using System;
using System.Linq;
using UnitTest.Modder.Mock;

namespace UnitTest.Modder.Warn
{
    [TestFixture()]
    public class TestWarnDepart
    {
        private ModFileSystem modFileSystem;

        private (string file, string content) WARN_TEST_DATA_1 = ("WARN_TEST_DATA_1.txt",
            @"
            trigger =
            {
	            equal = {item1.data1, 11}
            }
            ");

        private (string file, string content) WARN_TEST_DATA_2 = ("WARN_TEST_DATA_2.txt",
            @"
            trigger =
            {
	            equal = {item1.data2, 12}
            }
            desc = {item1.data2, 12}");

        public TestWarnDepart()
        {
            ModDataVisit.InitVisitMap(typeof(Demon));

            ModFileSystem.Clear();

            modFileSystem = ModFileSystem.Generate(nameof(TestWarnCommon));
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
            ModFileSystem.Clear();
        }

        [Test()]
        public void TestWarnNotTrigger()
        {
            LoadWarn(WARN_TEST_DATA_1, WARN_TEST_DATA_2);

            Demon.inst.item1.data1 = 10;
            Demon.inst.item1.data2 = 10;

            var warns = Mod.WarnProcess().ToArray();

            Assert.AreEqual(0, warns.Length);
        }

        [Test()]
        public void TestWarnTriggerSecond()
        {
            LoadWarn(WARN_TEST_DATA_1, WARN_TEST_DATA_2);

            Demon.inst.item1.data1 = 11;
            Demon.inst.item1.data2 = 12;

            var warns = Mod.WarnProcess().ToArray();

            Assert.AreEqual(2, warns.Length);

            var warn1 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DATA_1");
            Assert.NotNull(warn1);
            Assert.AreEqual(0, warn1.datas.Count);
            

            var warn2 = warns.SingleOrDefault(x => x.key == "WARN_TEST_DATA_2");
            Assert.NotNull(warn2);
            Assert.AreEqual(1, warn2.datas.Count);
            Assert.AreEqual("12|12", warn2.datas[0]);
        }

        private void LoadWarn(params (string file, string content)[] warns)
        {
            foreach (var warn in warns)
            {
                modFileSystem.AddCommonWarn(warn.file, warn.content);
            }

            Mod.Load(ModFileSystem.path);
        }
    }
}
