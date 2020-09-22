using System;
using System.Collections.Generic;

namespace RunData
{
    public class Define
    {
        public Dictionary<string, DepartDef> departs;
        public Dictionary<string, PopDef> pops;
        public EconomyDef economy;
        public ChaotingDef chaoting;
        public CropDef crop;
        public List<TaxEffect> pop_tax;

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
            public double? consume;
        }

        public class EconomyDef
        {
            public double curr;
            public double pop_tax_percent;
            public double expend_depart_admin;
        }

        public class ChaotingDef
        {
            public double reportPopPercent;
            public double taxPercent;
        }

        public class CropDef
        {
            public double growSpeed;
            public (int?, int?, int?) growStartDay;
            public (int?, int?, int?) harvestDay;
        }

        public class TaxEffect
        {
            public string name;
            public double per_tax;
            public double consume_effect;
        }
    }
}
