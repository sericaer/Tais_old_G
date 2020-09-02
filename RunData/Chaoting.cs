using System;
using System.Linq;
using System.Reactive.Linq;

namespace RunData
{
    public class Chaoting
    {
        public ObservableValue<double> requireTax;

        public SubjectValue<int> reportPopNum;

        public SubjectValue<double> taxPercent;

        public static Chaoting inst
        {
            get
            {
                return Root.inst.chaoting;
            }
        }

        public Chaoting(Define.ChaotingDef def)
        {
            taxPercent = new SubjectValue<double>(def.taxPercent);
            reportPopNum = new SubjectValue<int>((int)(Depart.all.Sum(x=>x.popNum.Value) * def.reportPopPercent / 100));

            requireTax = Observable.CombineLatest(reportPopNum.obs, taxPercent.obs, (n, p) => n * 0.01 * p / 100).ToOBSValue();
        }

        internal static Chaoting Init(Define.ChaotingDef def)
        {
            return new Chaoting(def);
        }
    }
}
