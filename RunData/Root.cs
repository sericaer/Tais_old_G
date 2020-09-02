using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using DataVisit;

namespace RunData
{
    public class Root
    {
        public static Action<string> logger;


        public static Root Init(Define def)
        {
            new Root(def);



            //Background.Init();
            //Chaoting.Init(Background.Enumerate());

            //Depart.Init();
            //Pop.Init(Depart.Enumerate());
            //Family.Init(Pop.Enumerate(), Background.Enumerate());
            //Person.Init(Family.Enumerate());

            return inst;
        }

        public static void Exit()
        {
            inst = null;
        }

        //public static void Load(string path)
        //{
        //    var str = File.ReadAllText(path);
        //    var josnObject = JObject.Parse(str);

        //    Date.LoadJson(josnObject);
        //}

        //public static void Save(string path)
        //{
        //    JObject josnObject = new JObject();
        //    Date.ToJson(ref josnObject);

        //    var json = josnObject.ToString();

        //    File.WriteAllText(path, json);
        //}



        public static void DaysInc()
        {
            Date.Inc();

            Pop.DaysInc();
        }

        public static Root inst;

        [DataVisitorProperty("date")]
        public Date date;

        [DataVisitorProperty("economy")]
        public Economy economy;

        public Chaoting chaoting;

        [DataVisitorPropertyArray("depart")]
        public List<Depart> departs;

        [DataVisitorPropertyArray("pop")]
        public List<Pop> pops;

        internal Define def;

        public Root(Define def)
        {
            this.def = def;

            inst = this;

            date = Date.Init();

            departs = Depart.Init(def.departs.Keys);

            pops = Pop.Init(departs);

            chaoting = Chaoting.Init(def.chaoting);

            economy = Economy.Init(def.economy);
        }
    }
}
