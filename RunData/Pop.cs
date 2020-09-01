using System;
using System.Linq;
using System.Collections.Generic;
using DataVisit;
using System.Reactive.Linq;

namespace RunData
{
    public class Pop
    {
        public static List<Pop> all
        {
            get
            {
                return Root.inst.pops;
            }
        }

        public string name;
        public string depart_name;

        public SubjectValue<double> num;
        public ObservableValue<double> expectTax;


        public Pop(string depart_name, string name, int num)
        {
            this.name = name;
            this.depart_name = depart_name;

            this.num = new SubjectValue<double>(num);
            this.expectTax = this.num.obs.Select(x => def.is_collect_tax ? x * 0.01 : 0).ToOBSValue();
        }

        internal Define.PopDef def
        {
            get
            {
                return Root.inst.def.pops[name];
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
            });
        }
    }
}