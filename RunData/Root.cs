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
using RunDefine;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Root
    {
        public static Action<string> logger;

        public static void SetDef(Define def)
        {
            Root.def = def;
        }

        public static void Init(InitData initData)
        {
            inst = new Root(initData);
        }

        public static void Exit()
        {
            inst = null;
        }

        public static IEnumerable<Process> GetTask()
        {
            return inst.processes;
        }

        public static string Serialize()
        {
            return JsonConvert.SerializeObject(inst, Formatting.Indented);
        }

        public static void Deserialize(string json)
        {
            inst = JsonConvert.DeserializeObject<Root>(json);
        }

        public static void DaysInc()
        {
            Economy.DaysInc();

            Depart.DaysInc();

            Pop.DaysInc();

            Chaoting.DaysInc();

            Date.Inc();

            Process.DaysInc();
        }

        public static IEnumerable<Warn> GenerateWarns()
        {
            return Warn.All.Where(x => x.IsValid());
        }

        public static Define def;
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

        [JsonProperty]
        public List<Process> processes;

        [JsonProperty]
        public List<ProcessNew> processNews;

        [JsonProperty]
        public List<Party> partys;

        [DataVisitorProperty("process")]
        public ProcessCOM processCOM;

        //public List<Party> parties;

        public bool isEnd
        {
            get
            {
                return taishou.isRevoke;
            }
        }

        public Root(InitData initData)
        {
            inst = this;

            processCOM = new ProcessCOM();

            processNews = new List<ProcessNew>();

            partys = Party.Init(def.partys.Values.ToList());

            taishou = new Taishou(initData.common.name, initData.common.age, initData.common.party);

            date = Date.Init();

            departs = Depart.Init(def.departs.Keys);

            pops = Pop.Init(departs);

            chaoting = Chaoting.Init(def.chaoting);

            economy = Economy.Init(def.economy);

            processes = Process.Init();
            //parties = Party.Init();
        }

        [JsonConstructor]
        private Root()
        {
            inst = this;
        }
    }

    public abstract class Warn
    {
        public static List<Warn> All = new List<Warn>();

        static Warn()
        {
            Warn.All.Add(new CHAOTING_TAX_NOT_FULL());
        }

        public string name
        {
            get
            {
                return GetType().Name;
            }
        }

        public List<string> datas = new List<string>();

        internal abstract bool IsValid();
    }

    internal class CHAOTING_TAX_NOT_FULL : Warn
    {
        internal override bool IsValid()
        {
            if (Chaoting.inst.oweTax > 0)
            {
                datas.Clear();
                datas.Add(Chaoting.inst.oweTax.ToString());
                return true;
            }
            else
            {
                datas.Clear();
                return false;
            }
        }
    }
}
