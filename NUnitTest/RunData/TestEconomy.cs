﻿using DataVisit;
using Modder;
using Newtonsoft.Json;
using NUnit.Framework;
using RunData;
using System;
using System.Collections.Generic;
using System.Linq;
namespace UnitTest.RunData
{
    [TestFixture()]
    public class TestEconomy : TestRunData
    {
        [Test()]
        public void Test_EconomyCurrValue()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);

            Assert.AreEqual(Root.def.economy.curr, Visitor.Get("economy.value"));

            Economy.inst.curr.Value = 100;

            var json = JsonConvert.SerializeObject(Economy.inst, Formatting.Indented);

            Root.inst.economy = JsonConvert.DeserializeObject<Economy>(json);

            Assert.AreEqual(100, Visitor.Get("economy.value"));
        }

        [Test()]
        public void Test_EconomyInCome_PopTax()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);

            var popTax = Economy.inst.incomes.Single(x => x.name == "STATIC_POP_TAX");

            Assert.AreEqual(Root.def.economy.pop_tax_percent, popTax.percent.Value);
            Assert.AreEqual(Depart.all.Sum(x => x.tax.Value), popTax.maxValue.Value);
            Assert.AreEqual(popTax.maxValue.Value * popTax.percent.Value / 100, popTax.currValue.Value);

            popTax.percent.Value = 12.3;

            var json = JsonConvert.SerializeObject(Economy.inst, Formatting.Indented);

            Root.inst.economy = JsonConvert.DeserializeObject<Economy>(json);

            Assert.AreEqual(12.3, popTax.percent.Value);
            Assert.AreEqual(Depart.all.Sum(x => x.tax.Value), popTax.maxValue.Value);
            Assert.AreEqual(popTax.maxValue.Value * popTax.percent.Value / 100, popTax.currValue.Value);
        }

        [Test()]
        public void Test_EconomyOutput_AdminExpend()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);

            var adminExpend = Economy.inst.outputs.Single(x => x.name == "STATIC_ADMIN_EXPEND");

            Assert.AreEqual(Root.def.economy.expend_depart_admin, adminExpend.percent.Value);
            Assert.AreEqual(Depart.all.Sum(x => x.adminExpendBase.Value), adminExpend.maxValue.Value);
            Assert.AreEqual(adminExpend.maxValue.Value * adminExpend.percent.Value / 100, adminExpend.currValue.Value);

            adminExpend.percent.Value = 12.3;

            var json = JsonConvert.SerializeObject(Economy.inst, Formatting.Indented);

            Root.inst.economy = JsonConvert.DeserializeObject<Economy>(json);

            Assert.AreEqual(12.3, adminExpend.percent.Value);
            Assert.AreEqual(Depart.all.Sum(x => x.adminExpendBase.Value), adminExpend.maxValue.Value);
            Assert.AreEqual(adminExpend.maxValue.Value * adminExpend.percent.Value / 100, adminExpend.currValue.Value);
        }

        [Test()]
        public void Test_EconomyOutput_ReportChaoting()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);

            var report = Economy.inst.outputs.Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX");

            Assert.AreEqual(Root.def.economy.report_chaoting_percent, report.percent.Value);
            Assert.AreEqual(Chaoting.inst.expectMonthTaxValue.Value, report.maxValue.Value);
            Assert.AreEqual(report.maxValue.Value * report.percent.Value / 100, report.currValue.Value);

            report.percent.Value = 12.3;

            var json = JsonConvert.SerializeObject(Economy.inst, Formatting.Indented);

            Root.inst.economy = JsonConvert.DeserializeObject<Economy>(json);

            Assert.AreEqual(12.3, report.percent.Value);
            Assert.AreEqual(Chaoting.inst.expectMonthTaxValue.Value, report.maxValue.Value);
            Assert.AreEqual(report.maxValue.Value * report.percent.Value / 100, report.currValue.Value);

            Date.inst.day.Value = 30;
            Economy.DaysInc();

            Assert.AreEqual(Chaoting.inst.expectMonthTaxValue.Value - report.currValue.Value, Chaoting.inst.oweTax);
        }

        [Test()]
        public void Test_EconomyMonthSurplus()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);

            Assert.AreEqual(Economy.inst.incomes.Sum(x => x.currValue.Value), Economy.inst.incomes.total.Value);
            Assert.AreEqual(Economy.inst.outputs.Sum(x => x.currValue.Value), Economy.inst.outputs.total.Value);

            Assert.AreEqual(Economy.inst.incomes.total.Value - Economy.inst.outputs.total.Value, Economy.inst.monthSurplus.Value);

        }

        [Test()]
        public void Test_EconomyDayInc()
        {
            ModDataVisit.InitVisitMap(typeof(Root));

            Root.Init(init);
            ModDataVisit.InitVisitData(Root.inst);

            Date.inst.day.Value = 29;
            Economy.DaysInc();

            Assert.AreEqual(Root.def.economy.curr, Visitor.Get("economy.value"));

            Date.inst.day.Value = 30;
            Economy.DaysInc();

            Assert.AreEqual(Root.def.economy.curr + Economy.inst.monthSurplus.Value, Visitor.Get("economy.value"));
        }
    }
}
