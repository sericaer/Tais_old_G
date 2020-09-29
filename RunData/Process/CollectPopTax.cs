using System;
using System.Linq;
using Newtonsoft.Json;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class COLLECT_POP_TAX : ProcessNew
    {
        [JsonProperty]
        public int? selectedLevel;

        [JsonProperty]
        public double? collectedTax;

        public static COLLECT_POP_TAX inst
        {
            get
            {
                return Root.inst.processNews.SingleOrDefault(x => x is COLLECT_POP_TAX) as COLLECT_POP_TAX;
            }
        }

        public int maxTaxLevel
        {
            get
            {
                return Root.def.pop_tax.Count();
            }
        }

        public double expectTax
        {
            get
            {
                return Chaoting.inst.reportPopNum.Value * 0.001;
            }
        }

        public new static bool isFinishedDay()
        {
            if(inst == null)
            {
                return false;
            }

            return ((Process)inst).isFinishedDay();
        }

        public static void Start()
        {
            Root.inst.processes.Add(new COLLECT_POP_TAX());
        }

        public double CalcTax(int level)
        {
            return Depart.all.Sum(x => x.pops.Sum(y => y.CalcTax(level)));
        }


        public void SetLevel(int level)
        {
            selectedLevel = level;
        }

        internal override void DoFinished()
        {
            collectedTax = Depart.all.Sum(x => x.pops.Sum(y => y.CollectTax(selectedLevel.Value)));
        }

        public COLLECT_POP_TAX() : base(30)
        {

        }
    }
}
