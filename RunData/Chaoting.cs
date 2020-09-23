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
        [JsonProperty]
        public SubjectValue<int> reportPopNum;

        [JsonProperty, DataVisitorProperty("extra_tax")]
        public double extraTax;

        //[DataVisitorProperty("power_party")]
        //public Party powerParty;
        
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

        }

        internal Chaoting(Define.ChaotingDef def)
        {
            reportPopNum = new SubjectValue<int>((int)(Depart.all.Sum(x => x.popNum.Value) * def.reportPopPercent / 100));

            InitObservableData(new StreamingContext());
        }

        [JsonConstructor]
        private Chaoting()
        {
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
        }
    }
}
