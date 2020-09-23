using System;
using System.Linq;
using System.Collections.Generic;
using DataVisit;
using System.Reactive.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;

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

        public ObservableValue<double> expectTax;
        public ObservableValue<double> adminExpend;

        public SubjectValue<double> consume;

        [JsonProperty]
        public ObservableCollection<(string name, double value, int endDays)> consumeDetail;

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
            depart.adminExpendBase = Observable.CombineLatest(pops.Select(x => x.adminExpend.obs),
                       (IList<double> nums) => nums.Sum()).ToOBSValue();
            return pops;
        }

        internal static void DaysInc()
        {
            all.ForEach(pop =>
            {
                pop.num.Value++;
            });
        }

        internal Pop(string depart_name, string name, int num)
        {
            this.name = name;
            this.depart_name = depart_name;

            this.num = new SubjectValue<double>(num);

            if(def.consume != null)
            {
                this.consume = new SubjectValue<double>(def.consume.Value);
                this.consumeDetail = new ObservableCollection<(string name, double value, int endDays)>()
                {
                    ("BASE_VALUE", def.consume.Value, -1)
                };
            }

            InitObservableData(new StreamingContext());
        }

        internal double CalcTax(int level)
        {
            if (!def.is_collect_tax)
            {
                return 0;
            }

            return num.Value * Root.def.pop_tax.Single(x=>x.name == $"level{level}").per_tax;
        }

        internal double CollectTax(int level)
        {
            if (!def.is_collect_tax)
            {
                return 0;
            }

            if(def.consume != null)
            {
                consumeDetail.Add(("COLLECT_TAX", Root.def.pop_tax.Single(x => x.name == $"level{level}").consume_effect, Date.inst.total_days.Value + 360));
            }

            return CalcTax(level);
        }

        [JsonConstructor]
        private Pop()
        {
            this.consumeDetail = new ObservableCollection<(string name, double value, int endDays)>();
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            this.expectTax = this.num.obs.Select(x => def.is_collect_tax ? x * 0.01 : 0).ToOBSValue();
            this.adminExpend = this.num.obs.Select(x => def.is_collect_tax ? x * 0.0003 : 0).ToOBSValue();

            if (def.consume != null)
            {
                this.consume = new SubjectValue<double>(def.consume.Value);

                this.consumeDetail.CollectionChanged += (a, r) =>
                {
                    UpdateConsume();
                };

                UpdateConsume();
            }
        }

        private void UpdateConsume()
        {
            consume.Value = this.consumeDetail.Sum(x => x.value);
        }
    }
}