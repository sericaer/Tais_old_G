using System;
using System.Linq;

namespace RunData
{
    public class CollectPopTax : Process
    {
        public static CollectPopTax inst
        {
            get
            {
                return Root.inst.processes.SingleOrDefault(x => x is CollectPopTax) as CollectPopTax;
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
            Root.inst.processes.Add(new CollectPopTax());
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

        private CollectPopTax() : base(30)
        {
            expectTax = Chaoting.inst.reportPopNum.Value * 0.001;
        }
    }
}
