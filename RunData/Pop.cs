using System;
using System.Linq;
using System.Collections.Generic;
using DataVisit;

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

        public Reactive<double> num;
        public Reactive<double> expectTax;

        [DataVisitorProperty("pop.num")]
        public int numValue
        {
            get
            {
                return (int)num.value;
            }
            set
            {
                num.value = value;
            }
        }

        public Pop(string depart_name, string name, int num)
        {
            this.name = name;
            this.depart_name = depart_name;

            this.num = new Reactive<double>(num);

            if(def.is_collect_tax)
            {
                this.expectTax = Reactive<double>.From(this.num, (x) => x * 0.01);
            }
            else
            {
                this.expectTax = new Reactive<double>(0);
            }

        }

        internal PopDef def
        {
            get
            {
                return PopDef.dict[name];
            }
        }

        internal static List<Pop> Init(IEnumerable<Depart> departs)
        {
            var all = new List<Pop>();
            foreach(var depart in departs)
            {
                foreach (var pop_init in depart.pops_init)
                {
                    all.Add(new Pop(depart.name, pop_init.name, pop_init.num));
                }
            }
            return all;
        }

        internal static void DaysInc()
        {
            all.ForEach(pop =>
            {
                pop.num.value++;
            });
        }
    }

    public class PopDef
    {
        static PopDef()
        {
            dict = new Dictionary<string, PopDef>()
            {
                { "haoqiang", new PopDef(){ is_collect_tax = true} },
                { "minhu", new PopDef(){ is_collect_tax = true} },
                { "yinhu", new PopDef(){ is_collect_tax = false} },
            };
        }

        public static Dictionary<string, PopDef> dict;


        public bool is_collect_tax;
    }
}