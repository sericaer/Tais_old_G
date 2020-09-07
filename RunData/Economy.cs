using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVisit;

namespace RunData
{
    public class Economy
    {
        public class Memento
        {
            internal Dictionary<string, double> incomes;
            internal Dictionary<string, double> outputs;
        }

        public static Economy inst
        {
            get
            {
                return Root.inst.economy;
            }
        }

        public static Economy Init(Define.EconomyDef def)
        {
            return new Economy(def);
        }

        [DataVisitorProperty("value")]
        public SubjectValue<double> curr;

        public IEnumerable<InCome> EnumerateInCome()
        {
            foreach(var elem in inComes)
            {
                yield return elem;
            }
        }

        public IEnumerable<Output> EnumerateOutput()
        {
            foreach (var elem in outputs)
            {
                yield return elem;
            }
        }

        public Memento CreateMemento()
        {
            var rslt = new Memento();
            rslt.incomes = this.inComes.ToDictionary(x => x.name, y => y.percent.Value);
            rslt.outputs = this.outputs.ToDictionary(x => x.name, y => y.percent.Value);

            return rslt;
        }

        internal static void DaysInc()
        {
            if(Date.inst == (null, null, 30))
            {
                inst.curr.Value += inst.monthSurplus.Value;
            }
        }

        public void LoadMemento(Memento memento)
        {
            foreach(var pair in memento.incomes)
            {
                inComes.Find(x => x.name == pair.Key).percent.Value = pair.Value;
            }

            foreach (var pair in memento.outputs)
            {
                outputs.Find(x => x.name == pair.Key).percent.Value = pair.Value;
            }
        }

        private Economy(Define.EconomyDef def)
        {
            curr = new SubjectValue<double>(def.curr);

            inComes = new List<InCome>() {
                new InCome( "STATIC_POP_TAX", def.pop_tax_percent, Observable.CombineLatest(Pop.all.Select(x=>x.expectTax.obs), (IList<double> taxs)=>taxs.Sum()))
            };

            outputs = new List<Output>() {
                new Output("STATIC_REPORT_CHAOTING_TAX", def.report_tax_percent, Root.inst.chaoting.currMonthTax.obs.Select(x=>x))
            };

            IncomeTotal = Observable.CombineLatest(inComes.Select(x => x.currValue.obs), (IList<double> all) => all.Sum()).ToOBSValue();
            OuputTotal = Observable.CombineLatest(outputs.Select(x => x.currValue.obs), (IList<double> all) => all.Sum()).ToOBSValue();
            monthSurplus = Observable.CombineLatest(IncomeTotal.obs, OuputTotal.obs, (i, o) => i - o).ToOBSValue();
        }

        public ObservableValue<double> IncomeTotal;
        public ObservableValue<double> OuputTotal;

        [DataVisitorProperty("month_surplus")]
        public ObservableValue<double> monthSurplus;

        [DataVisitorProperty("report_chaoting_tax_percent")]
        public double reportTaxPercent
        {
            get
            {
                return outputs.Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX").percent.Value;
            }
            set
            {
                outputs.Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX").percent.Value = value;
            }
        }

        internal List<InCome> inComes;
        internal List<Output> outputs;
    }

    public class InCome
    {
        public string name;
        public SubjectValue<double> percent;
        public ObservableValue<double> currValue;
        public ObservableValue<double> maxValue;

        internal InCome(string name, double percent, IObservable<double> max)
        {
            this.name = name;
            this.percent = new SubjectValue<double>(percent);
            this.maxValue = max.ToOBSValue();
            this.currValue = Observable.CombineLatest(this.percent.obs, this.maxValue.obs, (p, m) => p * m / 100).ToOBSValue();
        }
    }

    public class Output
    {
        public string name;
        public SubjectValue<double> percent;
        public ObservableValue<double> currValue;
        public ObservableValue<double> maxValue;

        internal Output(string name, double percent, IObservable<double> max)
        {
            this.name = name;
            this.percent = new SubjectValue<double>(percent);
            this.maxValue = max.ToOBSValue();
            this.currValue = Observable.CombineLatest(this.percent.obs, this.maxValue.obs, (p, m) => p * m / 100).ToOBSValue();
        }
    }
}
