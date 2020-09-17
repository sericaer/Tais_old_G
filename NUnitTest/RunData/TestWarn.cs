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
    public class TestWarn
    {
        public InitData init;

        public TestWarn()
        {
            Root.def = new Define()
            {
                departs = new Dictionary<string, Define.DepartDef>()
                {
                    { "JIXIAN", new Define.DepartDef() { color = (63, 72, 204),
                        pop_init = new (string name, int num)[] { ("haoqiang", 3000), ("minhu", 60000), ("yinhu", 20000) } } }
                },

                pops = new Dictionary<string, Define.PopDef>()
                {
                    { "haoqiang", new Define.PopDef() { is_collect_tax = true } },
                    { "minhu", new Define.PopDef() { is_collect_tax = true } },
                    { "yinhu", new Define.PopDef() { is_collect_tax = false } },
                },

                economy = new Define.EconomyDef()
                {
                    curr = 456,
                    pop_tax_percent = 30,
                    report_tax_percent = 100,
                },

                chaoting = new Define.ChaotingDef()
                {
                    reportPopPercent = 130,
                    taxPercent = 20
                },

                crop = new Define.CropDef()
                {
                    growSpeed = 0.4,
                    growStartDay = (null, 2, 1),
                    harvestDay = (null, 9, 1),
                }
            };

            init = new InitData()
            {
                common = new InitData.Common()
                {
                    name = "TEST1",
                    age = 34,
                    background = "BACKGROUND1"
                }
            };
        }

        [Test()]
        public void Test_CHAOTING_TAX_NOT_FULL()
        {
            //Root.Init(init);

            //ModDataVisit.InitVisitData(Root.inst);

            //Visitor.InitVisitMap(typeof(Root));
            //Visitor.SetVisitData(Root.inst);

            //Root.inst.chaoting.expectYearTax.Value = 100;
            //Root.inst.chaoting.realYearTax.Value = 100;

            //var warns = Root.GenerateWarns().ToList();
            //Assert.AreEqual(0, warns.Count);


            //Root.inst.chaoting.expectYearTax.Value = 100;
            //Root.inst.chaoting.realYearTax.Value = 110;

            //warns = Root.GenerateWarns().ToList();
            //Assert.AreEqual(0, warns.Count);

            //Root.inst.chaoting.expectYearTax.Value = 110;
            //Root.inst.chaoting.realYearTax.Value = 100;

            //warns = Root.GenerateWarns().ToList();
            //Assert.AreEqual(1, warns.Count);
            //Assert.AreEqual("CHAOTING_TAX_NOT_FULL", warns[0].name);
            //Assert.AreEqual(1, warns[0].datas.Count);
            //Assert.AreEqual("10", warns[0].datas[0]);
        }
    }
}
