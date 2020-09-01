﻿using System;
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

        public static Economy Init()
        {
            return new Economy();
        }

        [DataVisitorProperty("value")]
        public double currValue
        {
            get
            {
                return curr.Value;
            }
            set
            {
                curr.Value = value;
            }
        }

        [DataVisitorProperty("month_surplus")]
        public double monthSurplus
        {
            get
            {
                return inComes.Sum(x => x.currValue.value) - outputs.Sum(x => x.currValue.value);
            }
        }

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

        private Economy()
        {
            curr = new SubjectValue<double>(100.0);

            inComes = new List<InCome>() {
                new InCome( "STATIC_POP_TAX", 30, Observable.CombineLatest(Pop.all.Select(x=>x.expectTax.obs), (IList<double> taxs)=>taxs.Sum()))
            };

            outputs = new List<Output>() {
                new Output("STATIC_COUNTRY_TAX", 100, Root.inst.chaoting.requireTax.obs.Select(x=>x))
            };
        }

        private List<InCome> inComes;
        private List<Output> outputs;
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
