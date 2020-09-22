using System;
using System.Linq;

namespace RunData
{
    public class COLLECT_POP_TAX : Process
    {
        public static COLLECT_POP_TAX inst
        {
            get
            {
                return Root.inst.processes.SingleOrDefault(x => x is COLLECT_POP_TAX) as COLLECT_POP_TAX;
            }
        }

        public readonly double expectTax;

        public int? selectedLevel;

        public double? collectedTax;

        public int maxTaxLevel
        {
            get
            {
                return Root.def.pop_tax.Count();
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

        private COLLECT_POP_TAX() : base(30)
        {
            expectTax = Chaoting.inst.reportPopNum.Value * 0.001;
        }
    }
}
