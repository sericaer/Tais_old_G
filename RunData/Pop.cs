using System;
using System.Collections.Generic;

namespace RunData
{
    public class Pop
    {
        public static List<Pop> all = new List<Pop>();

        public string name;
        public string depart_name;

        public Reactive<int> num;

        public Pop(string name, int num, string depart_name)
        {
            this.name = name;
            this.num = new Reactive<int>(num);
            this.depart_name = depart_name;
        }

        internal static void Init(IEnumerable<Depart> departs)
        {
            foreach (var depart in departs)
            {
                foreach (var pop_init in depart.pops_init)
                {
                    all.Add(new Pop(pop_init.name, pop_init.num, depart.name));
                }
            }
        }

        internal static void Exit()
        {
            all = null;
        }
    }
}