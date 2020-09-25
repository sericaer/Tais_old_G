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
    public class TestChaoting
    {
        public InitData init;

        public TestChaoting()
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
                    { "minhu", new Define.PopDef() { is_collect_tax = true, consume = 100} },
                    { "yinhu", new Define.PopDef() { is_collect_tax = false } },
                },

                partys = new Dictionary<string, Define.PartyDef>()
                {
                    { "shizu", new Define.PartyDef() { name = "shizu"} },
                    { "huanguan", new Define.PartyDef() { name = "huanguan"} }
                },

                economy = new Define.EconomyDef()
                {
                    curr = 456,
                    pop_tax_percent = 30,
                    expend_depart_admin = 100,
                },

                chaoting = new Define.ChaotingDef()
                {
                    reportPopPercent = 130,
                    taxPercent = 20,
                    powerParty = "huanguan"
                },

                crop = new Define.CropDef()
                {
                    growSpeed = 0.4,
                    growStartDay = (null, 2, 1),
                    harvestDay = (null, 9, 1),
                },

                pop_tax = new List<Define.TaxEffect>()
                {
                    new Define.TaxEffect(){name = "level1", per_tax = 0.001, consume_effect = -10},
                    new Define.TaxEffect(){name = "level2", per_tax = 0.002, consume_effect = -20},
                    new Define.TaxEffect(){name = "level3", per_tax = 0.003, consume_effect = -30 },
                    new Define.TaxEffect(){name = "level4", per_tax = 0.0035, consume_effect = -40 },
                    new Define.TaxEffect(){name = "level5", per_tax = 0.004, consume_effect = -50 }
                }
            };

            init = new InitData()
            {
                common = new InitData.Common()
                {
                    name = "TEST1",
                    age = 34,
                    background = "BACKGROUND1",
                    party = "shizu"
                }
            };
        }

        [Test()]
        public void Test_ChaotingExtraTax()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);

            Assert.AreEqual(0, Visitor.Get("chaoting.extra_tax"));
            Assert.AreEqual(0, Visitor.Get("chaoting.owe_tax"));

            Chaoting.inst._extraTax = 100;

            Assert.AreEqual(100, Visitor.Get("chaoting.extra_tax"));
            Assert.AreEqual(0, Visitor.Get("chaoting.owe_tax"));

            Chaoting.inst._extraTax = -100;

            Assert.AreEqual(0, Visitor.Get("chaoting.extra_tax"));
            Assert.AreEqual(100, Visitor.Get("chaoting.owe_tax"));
        }

        [Test()]
        public void Test_ChaotingPowerParty()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);

            Assert.AreEqual(Root.def.chaoting.powerParty, Visitor.Get("chaoting.power_party.type"));
        }
    }
}
