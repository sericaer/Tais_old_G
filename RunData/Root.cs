using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using DataVisit;
using Newtonsoft.Json;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Root
    {
        public static Action<string> logger;


        public static Root Init(Define def)
        {
            new Root(def);
            return inst;
        }

        public static void Exit()
        {
            inst = null;
        }

        public static string Serialize()
        {
            return JsonConvert.SerializeObject(inst, Formatting.Indented);
        }


        internal static void Deserialize(string json, Define def)
        {
            inst = JsonConvert.DeserializeObject<Root>(json);
            inst.def = def;
        }

        public static void DaysInc()
        {
            Economy.DaysInc();

            Depart.DaysInc();

            Pop.DaysInc();

            Chaoting.DaysInc();

            Date.Inc();
        }

        public static Root inst;

        [JsonProperty, DataVisitorProperty("taishou")]
        public Taishou taishou;

        [JsonProperty, DataVisitorProperty("date")]
        public Date date;

        [JsonProperty, DataVisitorProperty("chaoting")]
        public Chaoting chaoting;

        [JsonProperty, DataVisitorPropertyArray("pop")]
        public List<Pop> pops;

        [JsonProperty, DataVisitorPropertyArray("depart")]
        public List<Depart> departs;

        [JsonProperty, DataVisitorProperty("economy")]
        public Economy economy;


        internal Define def;

        public bool isEnd
        {
            get
            {
                return taishou.isRevoke;
            }
        }

        public Root(Define def)
        {
            this.def = def;


            inst = this;

            taishou = new Taishou();

            date = Date.Init();

            departs = Depart.Init(def.departs.Keys);

            pops = Pop.Init(departs);

            chaoting = Chaoting.Init(def.chaoting);

            economy = Economy.Init(def.economy);
        }

        [JsonConstructor]
        private Root()
        {

        }
    }
}
