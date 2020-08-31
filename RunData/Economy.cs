using System;
using System.Collections.Generic;
using System.Linq;
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
                return curr.value;
            }
            set
            {
                curr.value = value;
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

        public Reactive<double> curr;

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
            rslt.incomes = this.inComes.ToDictionary(x => x.name, y => y.percent.value);
            rslt.outputs = this.outputs.ToDictionary(x => x.name, y => y.percent.value);

            return rslt;
        }

        public void LoadMemento(Memento memento)
        {
            foreach(var pair in memento.incomes)
            {
                inComes.Find(x => x.name == pair.Key).percent.value = pair.Value;
            }

            foreach (var pair in memento.outputs)
            {
                outputs.Find(x => x.name == pair.Key).percent.value = pair.Value;
            }
        }

        private Economy()
        {
            curr = new Reactive<double>(100);

            inComes = new List<InCome>() {
                new InCome( "", 30, ()=>Pop.all.Sum(x=>x.expectTax.value))
            };

            outputs = new List<Output>() {
                new Output("", 100, ()=>Root.inst.chaoting.requireTax)
            };
        }

        private List<InCome> inComes;
        private List<Output> outputs;
    }

    public class InCome
    {
        public string name;
        public Reactive<double> percent;
        public Reactive<double> currValue;

        public double maxValue
        {
            get
            {
                return GetMaxValue();
            }
        }

        internal Func<double> GetMaxValue;

        internal InCome(string name, double percent, Func<double> func)
        {
            this.name = name;
            this.percent = new Reactive<double>(percent);
            this.GetMaxValue = func;

            this.currValue = Reactive<double>.From(this.percent, (x=> maxValue * x / 100));
        }
    }

    public class Output
    {
        public string name;
        public Reactive<double> percent;
        public Reactive<double> currValue;

        public double maxValue
        {
            get
            {
                return GetMaxValue();
            }
        }

        internal Func<double> GetMaxValue;

        internal Output(string name, double percent, Func<double> func)
        {
            this.name = name;
            this.percent = new Reactive<double>(percent);
            this.GetMaxValue = func;

            this.currValue = Reactive<double>.From(this.percent, (x => maxValue * x / 100));
        }
    }
}
