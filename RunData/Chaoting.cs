using System;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using DataVisit;
using Newtonsoft.Json;
using RunDefine;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Chaoting
    {
        [JsonProperty]
        public SubjectValue<int> reportPopNum;

        [DataVisitorProperty("extra_tax")]
        public double extraTax
        {
            get
            {
                return _extraTax > 0 ? _extraTax : 0;
            }
        }

        [DataVisitorProperty("owe_tax")]
        public double oweTax
        {
            get
            {
                return _extraTax < 0 ? _extraTax*-1 : 0;
            }
        }

        [JsonProperty]
        public double _extraTax;

        [DataVisitorProperty("power_party")]
        public Party powerParty
        {
            get
            {
                return Root.inst.partys.Find(x => x.name == powerPartyName);
            }
        }

        [JsonProperty]
        internal string powerPartyName;

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
            powerPartyName = def.powerParty;
            if(powerParty == null)
            {
                throw new Exception($"can not find chaoting power party ${powerPartyName}");
            }

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
