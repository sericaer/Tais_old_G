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
                    pop_tax_percent = 30,
                    report_tax_percent = 100,
                },

                chaoting = new Define.ChaotingDef()
                {
                    reportPopPercent = 130,
                    taxPercent = 20
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

            foreach(var income in Root.inst.economy.EnumerateInCome())
            {
                switch(income.name)
                {
                    case "STATIC_POP_TAX":
                        Assert.AreEqual(def.economy.pop_tax_percent, income.percent.Value);
                        Assert.AreEqual(Pop.all.Sum(x => x.expectTax.Value) * income.percent.Value / 100, income.currValue.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;

                }
            }

            foreach (var output in Root.inst.economy.EnumerateOutput())
            {
                switch (output.name)
                {
                    case "STATIC_REPORT_COUNTRY_TAX":
                        Assert.AreEqual(def.economy.report_tax_percent, output.percent.Value);
                        Assert.AreEqual(Chaoting.inst.currMonthTax.Value * output.percent.Value / 100, output.currValue.Value);
                        break;
                    default:
                        Assert.Fail();
                        break;

                }
            }

            foreach (var departName in def.departs.Keys)
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

            Assert.AreEqual((int)(Depart.all.Sum(x=>x.popNum.Value)*def.chaoting.reportPopPercent/100), Chaoting.inst.reportPopNum.Value);
            Assert.AreEqual(def.chaoting.taxPercent, Chaoting.inst.taxPercent.Value);
            Assert.AreEqual(Chaoting.inst.reportPopNum.Value * 0.01 * def.chaoting.taxPercent/100, Chaoting.inst.currMonthTax.Value);
        }

        [SetUp()]
        public void SetUp()
        {

        }

        [Test()]
        public void TestSetGetEconomyData()
        {
            Root.Init(def);

            Visitor.InitVisitMap(typeof(Root));
            Visitor.SetVisitData(Root.inst);

            Visitor.Set("economy.value", 123.0);

            Assert.AreEqual(123, Visitor.Get("economy.value"));
        }

        [Test()]
        public void TestDateInc()
        {
            Root.Init(def);

            Visitor.InitVisitMap(typeof(Root));
            Visitor.SetVisitData(Root.inst);

            for (int y = 1; y<=10; y++)
            {
                for(int m=1; m<=12; m++)
                {
                    for(int d=1; d<=30; d++)
                    {
                        Assert.AreEqual(d, Visitor.Get("date.day"));
                        Assert.AreEqual(m, Visitor.Get("date.month"));
                        Assert.AreEqual(y, Visitor.Get("date.year"));

                        Date.Inc();
                    }
                }
            }
        }

        [Test()]
        public void TestChaotingDaysInc()
        {
            Root.Init(def);

            Visitor.InitVisitMap(typeof(Root));
            Visitor.SetVisitData(Root.inst);

            double yearExpertTax = 0;
            for (int y = 1; y <= 10; y++)
            {
                yearExpertTax = 0;

                for (int m = 1; m <= 12; m++)
                {
                    for (int d = 1; d <= 30; d++)
                    {
                        Chaoting.DaysInc();

                        if(d == 30)
                        {
                            yearExpertTax += Chaoting.inst.currMonthTax.Value;
                        }

                        Assert.AreEqual(yearExpertTax, Chaoting.inst.expectYearTax.Value);
                        Assert.AreEqual(Chaoting.inst.expectYearTax.Value, Visitor.Get("chaoting.expect_year_tax"));
                        Date.Inc();
                    }
                }
            }
        }
    }
}
