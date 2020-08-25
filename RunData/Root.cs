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


        public static Root Init()
        {
            inst = new Root();



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
            Depart.DaysInc();
        }

        public static Root inst;

        [DataVisitorProperty]
        public Date date;

        [DataVisitorProperty]
        public Economy economy;

        [DataVisitorPropertyArray]
        public List<Depart> departs;

        [DataVisitorPropertyArray]
        public List<Pop> pops;

        public Root()
        {
            date = Date.Init();

            departs = Depart.Init();

            pops = Pop.Init(departs);

            economy = Economy.Init();
        }
    }
}
