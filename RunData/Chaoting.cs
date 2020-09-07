using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using DataVisit;
using Newtonsoft.Json;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Chaoting
    {
        public ObservableValue<double> currMonthTax;

        [JsonProperty]
        public SubjectValue<int> reportPopNum;

        [JsonProperty]
        public SubjectValue<double> taxPercent;

        [JsonProperty, DataVisitorProperty("expect_year_tax")]
        public SubjectValue<double> expectYearTax;

        [JsonProperty, DataVisitorProperty("real_year_tax")]
        public SubjectValue<double> realYearTax;

        public static Chaoting inst
        {
            get
            {
                return Root.inst.chaoting;
            }
        }

        internal static Chaoting Init(Define.ChaotingDef def)
        {
            return new Chaoting(def);
        }

        internal static void DaysInc()
        {
            if (Date.inst == (null, 1, 1))
            {
                inst.expectYearTax.Value = 0;
                inst.realYearTax.Value = 0;
            }
            if (Date.inst == (null, null, 30))
            {
                inst.expectYearTax.Value += inst.currMonthTax.Value;
                inst.realYearTax.Value += Economy.inst.outputs.Single(x => x.name == "STATIC_REPORT_CHAOTING_TAX").currValue.Value;
            }
        }

        internal Chaoting(Define.ChaotingDef def)
        {
            taxPercent = new SubjectValue<double>(def.taxPercent);
            reportPopNum = new SubjectValue<int>((int)(Depart.all.Sum(x => x.popNum.Value) * def.reportPopPercent / 100));
            expectYearTax = new SubjectValue<double>(0);
            realYearTax = new SubjectValue<double>(0);

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Chaoting()
        {
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            currMonthTax = Observable.CombineLatest(reportPopNum.obs, taxPercent.obs, (n, p) => n * 0.01 * p / 100).ToOBSValue();
        }
    }
}
