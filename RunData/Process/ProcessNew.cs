using System;
using System.Collections.Generic;
using System.Linq;
using DataVisit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RunDefine;

namespace RunData
{
    //[JsonObject(MemberSerialization.OptIn), JsonConverter(typeof(ProcessConverter))]
    public class ProcessNew
    {
        [JsonProperty]
        public string name;

        [JsonProperty]
        public int startDays;

        [JsonProperty]
        public SubjectValue<double> percent;

        [DataVisitorProperty("param")]
        public object param;

        internal static void Cancel(string value)
        {
            Root.inst.processNews.RemoveAll(x => x.name == value);
        }

        internal static void Start(string value)
        {
            Root.inst.processNews.Add(new ProcessNew(value));
        }

        private bool isFinished
        {
            get
            {
                return percent.Value == 100.0;
            }
        }

        private Define.ProcessDef def
        {
            get
            {
                return Root.def.process.Single(x => x.name == name);
            }
        }

        public ProcessNew(string name)
        {
            this.name = name;
        }


        internal static void DaysInc()
        {
            Root.inst.processNews.RemoveAll(x => x.isFinished);
            Root.inst.processNews.ForEach(x => x.ProcessDaysInc());
        }

        internal void ProcessDaysInc()
        {
            percent.Value += def.speed / 100;
        }
    }

    //public class ProcessConverter : JsonConverter
    //{
    //    public override bool CanWrite { get { return false; } }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {

    //        JObject jObject = JObject.Load(reader);

    //        var name = jObject.Value<string>("name");

    //        Type type = Type.GetType($"RunData.{name}");

    //        var obj = Activator.CreateInstance(type);
    //        serializer.Populate(jObject.CreateReader(), obj);

    //        return obj;
    //    }

    //    public override bool CanConvert(Type objectType)
    //    {
    //        return objectType == typeof(Process);
    //    }


    //}
}
