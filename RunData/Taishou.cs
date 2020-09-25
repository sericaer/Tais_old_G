using DataVisit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Taishou
    {
        [DataVisitorProperty("is_revoke")]
        public bool isRevoke;

        [DataVisitorProperty("name"), JsonProperty]
        public string name;

        [DataVisitorProperty("age"), JsonProperty]
        public int age;

        [DataVisitorProperty("background"), JsonProperty]
        public string background;

        [DataVisitorProperty("party")]
        public Party party
        { 
            get
            {
                return Root.inst.partys.Find(x => x.name == partyName);
            }
        }

        [JsonProperty]
        internal string partyName;

        public Taishou(string name, int age, string background) : this()
        {
            this.name = name;
            this.age = age;
            this.background = background;
        }

        [JsonConstructor]
        private Taishou()
        {
            isRevoke = false;
        }

    }
}
