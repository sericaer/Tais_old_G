using System;
using System.Collections.Generic;

namespace RunData
{
    public class Define
    {
        public Dictionary<string, DepartDef> departs;
        public Dictionary<string, PopDef> pops;
        public EconomyDef economy;

        public Define()
        {
        }

        public class DepartDef
        {
            public string name;
            public (int r, int g, int b) color;
            public (string name, int num)[] pop_init;
        }

        public class PopDef
        {
            public string name;
            public bool is_collect_tax;
        }

        public class EconomyDef
        {
            public double curr;
        }
    }
}
