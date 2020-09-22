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
    public class TestProcess
    {
        public InitData init;

        public TestProcess()
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

                economy = new Define.EconomyDef()
                {
                    curr = 456,
                    pop_tax_percent = 30,
                    expend_depart_admin = 100,
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
                    background = "BACKGROUND1"
                }
            };
        }

        [Test()]
        public void Test_CollectPopTax()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);


            CollectPopTax.Start();
            Assert.AreEqual(Chaoting.inst.reportPopNum.Value * 0.001, CollectPopTax.inst.expectTax);

            int selected = 1;
            
            CollectPopTax.inst.SetLevel(selected);
            for (int i=0; i<30; i++)
            {
                Assert.IsFalse(CollectPopTax.isFinishedDay());
                Assert.AreEqual(1, Root.inst.processes.Count);

                Date.Inc();
                Process.DaysInc();
            }

            Assert.IsTrue(CollectPopTax.isFinishedDay());

            var currPopTax = Root.def.pop_tax.Single(x => x.name == $"level{selected}");
            var expectCollectedTax = Pop.all.Where(x => x.def.is_collect_tax).Sum(x => x.num.Value) * currPopTax.per_tax;
            Assert.AreEqual(expectCollectedTax, CollectPopTax.inst.collectedTax);

            Pop.all.ForEach(pop =>
            {
                if (pop.def.consume != null)
                {
                    Assert.AreEqual(pop.def.consume.Value + currPopTax.consume_effect, pop.consume.Value);
                }
            });

            Date.Inc();
            Process.DaysInc();
            Assert.AreEqual(0, Root.inst.processes.Count);
        }
    }
}
