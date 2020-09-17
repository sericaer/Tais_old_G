using System;
using System.Linq;
using System.Collections.Generic;
using DataVisit;
using System.Reactive.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Pop
    {
        public static List<Pop> all
        {
            get
            {
                return Root.inst.pops;
            }
        }

        [JsonProperty]
        public string name;

        [JsonProperty]
        public string depart_name;

        [JsonProperty]
        public SubjectValue<double> num;

        [JsonProperty, DataVisitorProperty("is_tax_collecting")]
        public bool taxCollecting;

        [JsonProperty, DataVisitorProperty("curr_tax_level")]
        public int currTaxLevel;

        public ObservableValue<double> expectTax;

        [DataVisitorProperty("depart")]
        public Depart depart
        {
            get
            {
                return Depart.all.Single(x => x.name == depart_name);
            }
        }

        internal Define.PopDef def
        {
            get
            {
                return Root.def.pops[name];
            }
        }

        internal static List<Pop> Init(IEnumerable<Depart> departs)
        {
            var all = new List<Pop>();
            foreach(var depart in departs)
            {
                all.AddRange(InitDepartPop(depart));
            }
            return all;
        }

        private static List<Pop> InitDepartPop(Depart depart)
        {
            var pops = new List<Pop>();

            foreach (var pop_init in depart.def.pop_init)
            {
                pops.Add(new Pop(depart.name, pop_init.name, pop_init.num));
            }

            depart.popNum = Observable.CombineLatest(pops.Where(x => x.def.is_collect_tax).Select(x => x.num.obs),
                                              (IList<double> nums) => nums.Sum(y => (int)y)).ToOBSValue();

            return pops;
        }

        internal static void DaysInc()
        {
            all.ForEach(pop =>
            {
                pop.num.Value++;
                pop.taxCollecting = false;
            });
        }

        internal Pop(string depart_name, string name, int num)
        {
            this.name = name;
            this.depart_name = depart_name;

            this.num = new SubjectValue<double>(num);

            InitObservableData(new StreamingContext());
        }

        internal double CalcTax(int level)
        {
            if (!def.is_collect_tax)
            {
                return 0;
            }

            return num.Value * PopTax.getTaxUnit(level);
        }

        internal double CollectTax(int level)
        {
            if (!def.is_collect_tax)
            {
                return 0;
            }

            taxCollecting = true;
            currTaxLevel = level;

            return CalcTax(level);
        }

        [JsonConstructor]
        private Pop()
        {

        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            this.expectTax = this.num.obs.Select(x => def.is_collect_tax ? x * 0.01 : 0).ToOBSValue();
        }


    }
}