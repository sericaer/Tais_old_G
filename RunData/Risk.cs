using System;
using System.Collections.Generic;
using System.Linq;
using DataVisit;
using Newtonsoft.Json;
using DefineRisk = Define.Risk;

namespace RunData
{
    public class RiskMgr
    {
        [DataVisitorProperty("start")]
        public string start
        {
            set
            {
                Risk.all.Add(new Risk(value));
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class Risk
    {
        [JsonProperty]
        public string key;

        [JsonProperty]
        public double percent;

        private DefineRisk def
        {
            get
            {
                return Root.def.risks.Single(x => x.key == key);
            }
        }

        public Risk(string key)
        {
            this.key = key;
        }

        public static List<Risk> all
        {
            get
            {
                return Root.inst.risks;
            }
        }

        internal static void DaysInc()
        {
            all.RemoveAll(risk => risk.percent >= 100);

            all.ForEach(risk =>
            {
                risk.percent += 1 / risk.def.cost_days;
            });
        }

        internal static List<Risk> Init()
        {
            return new List<Risk>();
        }
    }
}