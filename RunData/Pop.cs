using System;
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

        public Reactive<int> num;

        [DataVisitorProperty("pop.num")]
        public int numValue
        {
            get
            {
                return num.value;
            }
            set
            {
                num.value = value;
            }
        }

        public Pop(string depart_name, string name, int num)
        {
            this.name = name;
            this.num = new Reactive<int>(num);
            this.depart_name = depart_name;
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
}