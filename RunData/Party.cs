using System;
using System.Collections.Generic;
using DataVisit;
using Newtonsoft.Json;

namespace RunData
{
    public class Party
    {
        [DataVisitorProperty("type")]
        public string name;

        public Party(Define.PartyDef def)
        {
            this.name = def.name;
        }

        internal static List<Party> Init(List<Define.PartyDef> partyDefs)
        {
            List<Party> rslt = new List<Party>();

            foreach(var def in partyDefs)
            {
                rslt.Add(new Party(def));
            }

            return rslt;
        }

        [JsonConstructor]
        private Party()
        {
        }
    }
}