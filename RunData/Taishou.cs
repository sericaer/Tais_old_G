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

        [DataVisitorProperty("name")]
        public string name;

        [DataVisitorProperty("age")]
        public int age;

        [DataVisitorProperty("background")]
        public string background;

        public Taishou(string name, int age, string background)
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
