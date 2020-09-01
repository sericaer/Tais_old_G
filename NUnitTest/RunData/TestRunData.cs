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
        public Define def;

        public TestRunData()
        {
            def = new Define()
            {
                departs = new Dictionary<string, Define.DepartDef>()
                {
                    { "JIXIAN", new Define.DepartDef(){color = (63, 72, 204),
                                                       pop_init= new (string name, int num)[]{ ("haoqiang", 3000), ("minhu", 60000), ("yinhu", 20000) } } }
                },

                pops = new Dictionary<string, Define.PopDef>()
                {
                    { "haoqiang", new Define.PopDef(){ is_collect_tax = true} },
                    { "minhu", new Define.PopDef(){ is_collect_tax = true} },
                    { "yinhu", new Define.PopDef(){ is_collect_tax = false} },
                },

                economy = new Define.EconomyDef()
                {
                    curr = 456,
                }    

            };
        }

        [Test()]
        public void TestInitData()
        {

            Root.Init(def);

            Visitor.InitVisitMap(typeof(Root));
            Visitor.SetVisitData(Root.inst);

            Assert.AreEqual(1, Visitor.Get("date.day"));
            Assert.AreEqual(1, Visitor.Get("date.month"));
            Assert.AreEqual(1, Visitor.Get("date.year"));

            Assert.AreEqual(456, Visitor.Get("economy.value"));

            foreach(var departName in def.departs.Keys)
            {
                var departDef = def.departs[departName];

                var depart = Depart.GetByColor(departDef.color.r, departDef.color.g, departDef.color.b);
                Assert.AreEqual(departName, depart.name);

                Assert.AreEqual(departDef.pop_init.Where(x=> def.pops[x.name].is_collect_tax).Sum(x=>x.num), depart.popNum.Value);

                foreach(var init in departDef.pop_init)
                {
                    var pop = depart.pops.Single(x => x.name == init.name);
                    Assert.AreEqual(init.num, pop.num.Value);
                }
            }

        }

        [Test()]
        public void TestSetGetData()
        {
            Root.Init(def);

            Visitor.InitVisitMap(typeof(Root));
            Visitor.SetVisitData(Root.inst);

            Visitor.Set("economy.value", 123.0);

            Assert.AreEqual(123, Visitor.Get("economy.value"));
        }
    }
}
