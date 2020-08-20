using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace RunData
{
    public class Root
    {
        public static Action<string> logger;

        public static void Init()
        {
            Date.Init();
            //Background.Init();
            //Chaoting.Init(Background.Enumerate());

            Depart.Init();
            Pop.Init(Depart.Enumerate());
            //Family.Init(Pop.Enumerate(), Background.Enumerate());
            //Person.Init(Family.Enumerate());
        }

        public static void Load(string path)
        {
            var str = File.ReadAllText(path);
            var josnObject = JObject.Parse(str);

            Date.LoadJson(josnObject);
        }

        public static void Save(string path)
        {
            JObject josnObject = new JObject();
            Date.ToJson(ref josnObject);

            var json = josnObject.ToString();

            File.WriteAllText(path, json);
        }

        public static void DaysInc()
        {
            Date.Inc();

            Pop.DaysInc();
            Depart.DaysInc();
        }

        public static void Exit()
        {
            Pop.Exit();
            Depart.Exit();
            Date.Exit();
        }
    }
}
