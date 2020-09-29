using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DataVisit;
using Newtonsoft.Json;
using RunDefine;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
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

        [JsonProperty, DataVisitorProperty("value")]
        public SubjectValue<double> curr;

        //public ObservableValue<double> IncomeTotal;
        public ObservableValue<double> OuputTotal;

        //[DataVisitorProperty("month_surplus")]
        //public ObservableValue<double> monthSurplus;

        //[DataVisitorProperty("report_chaoting_tax_percent")]
        //public double reportTaxPercent
        //{
        //    get
        //    {
        //        return outputs.Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX").percent.Value;
        //    }
        //    set
        //    {
        //        outputs.Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX").percent.Value = value;
        //    }
        //}

        //[JsonProperty]
        //internal List<InCome> inComes;

        [JsonProperty]
        internal List<Output> outputs;

        //public IEnumerable<InCome> EnumerateInCome()
        //{
        //    foreach(var elem in inComes)
        //    {
        //        yield return elem;
        //    }
        //}

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
            //rslt.incomes = this.inComes.ToDictionary(x => x.name, y => y.percent.Value);
            rslt.outputs = this.outputs.ToDictionary(x => x.name, y => y.percent.Value);

            return rslt;
        }

        internal static void DaysInc()
        {
            if (Date.inst == (null, null, 30))
            {
                var newValue = inst.curr.Value - inst.OuputTotal.Value;
                inst.curr.Value = newValue > 0 ? newValue : 0;
            }
        }

        public void LoadMemento(Memento memento)
        {
            //foreach (var pair in memento.incomes)
            //{
            //    inComes.Find(x => x.name == pair.Key).percent.Value = pair.Value;
            //}

            foreach (var pair in memento.outputs)
            {
                outputs.Find(x => x.name == pair.Key).percent.Value = pair.Value;
            }
        }

        private Economy(Define.EconomyDef def)
        {
            curr = new SubjectValue<double>(def.curr);

            //inComes = InCome.Generate(def);
            outputs = Output.Generate(def);

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Economy()
        {

        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            //IncomeTotal = Observable.CombineLatest(inComes.Select(x => x.currValue.obs), (IList<double> all) => all.Sum()).ToOBSValue();
            OuputTotal = Observable.CombineLatest(outputs.Select(x => x.currValue.obs), (IList<double> all) => all.Sum()).ToOBSValue();
            //monthSurplus = Observable.CombineLatest(IncomeTotal.obs, OuputTotal.obs, (i, o) => i - o).ToOBSValue();
        }
    }

    //[JsonObject(MemberSerialization.OptIn)]
    //public class InCome
    //{
    //    [JsonProperty]
    //    public string name;

    //    [JsonProperty]
    //    public SubjectValue<double> percent;

    //    public ObservableValue<double> currValue;

    //    private static Dictionary<string, ObservableValue<double>> dictMax = new Dictionary<string, ObservableValue<double>>()
    //    {
    //        //{ "STATIC_POP_TAX", Observable.CombineLatest(Pop.all.Select(x=>x.expectTax.obs), (IList<double> taxs)=>taxs.Sum()).ToOBSValue()}
    //    };

    //    public ObservableValue<double> maxValue
    //    {
    //        get
    //        {
    //            return dictMax[name];
    //        }
    //    }

    //    internal static List<InCome> Generate(Define.EconomyDef def)
    //    {
    //        return new List<InCome>()
    //        {
    //            //new InCome("STATIC_POP_TAX", def.pop_tax_percent)
    //        };
    //    }

    //    internal InCome(string name, double percent)
    //    {
    //        this.name = name;
    //        this.percent = new SubjectValue<double>(percent);

    //        InitObservableData(new StreamingContext());
    //    }

    //    [JsonConstructor]
    //    private InCome()
    //    {

    //    }

    //    [OnDeserialized]
    //    private void InitObservableData(StreamingContext context)
    //    {
    //        this.currValue = Observable.CombineLatest(this.percent.obs, maxValue.obs, (p, m) => p * m / 100).ToOBSValue();
    //    }
    //}

    [JsonObject(MemberSerialization.OptIn)]
    public class Output
    {
        [JsonProperty]
        public string name;

        [JsonProperty]
        public SubjectValue<double> percent;

        public ObservableValue<double> currValue;

        private static Dictionary<string, ObservableValue<double>> dictMax = new Dictionary<string, ObservableValue<double>>()
        {
            { "STATIC_DEPART_ADMIN_EXPEND", Observable.CombineLatest(Root.inst.departs.Select(x=>x.adminExpendBase.obs), (expends)=>expends.Sum()).ToOBSValue()}
        };

        public ObservableValue<double> maxValue
        {
            get
            {
                return dictMax[name];
            }
        }

        internal static List<Output> Generate(Define.EconomyDef def)
        {
            return new List<Output>()
            {
                new Output("STATIC_DEPART_ADMIN_EXPEND", def.expend_depart_admin)
            };
        }

        internal Output(string name, double percent)
        {
            this.name = name;
            this.percent = new SubjectValue<double>(percent);

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Output()
        {

        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            this.currValue = Observable.CombineLatest(this.percent.obs, maxValue.obs, (p, m) => p * m / 100).ToOBSValue();
        }
    }
}
