using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn), JsonConverter(typeof(ProcessConverter))]
    public class Process
    {
        [JsonProperty]
        public string name;

        [JsonProperty]
        public int costDays;

        [JsonProperty]
        public int startDays;

        [JsonProperty]
        public SubjectValue<double> percent;

        private bool isFinished
        {
            get
            {
                return percent.Value == 100.0;
            }
        }

        internal Process(int costDays)
        {
            this.costDays = costDays;
            this.startDays = Date.inst.total_days.Value;
            this.name = GetType().Name;

            this.percent = new SubjectValue<double>(0.0);

        }

        internal static List<Process> Init()
        {
            return new List<Process>();
        }

        internal static void DaysInc()
        {
            Root.inst.processes.RemoveAll(x => x.isFinished);
            Root.inst.processes.ForEach(x => x.ProcessDaysInc());
        }

        internal void ProcessDaysInc()
        {
            percent.Value = (Date.inst.total_days.Value - startDays) * 100.0 / costDays;

            if (isFinishedDay())
            {
                DoFinished();
            }
        }

        internal bool isFinishedDay()
        {
            return isFinished;
        }

        internal virtual void DoFinished()
        {

        }
    }

    public class ProcessConverter : JsonConverter
    {
        public override bool CanWrite { get { return false; } }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            JObject jObject = JObject.Load(reader);

            var name = jObject.Value<string>("name");

            Type type = Type.GetType($"RunData.{name}");

            var obj = Activator.CreateInstance(type);
            serializer.Populate(jObject.CreateReader(), obj);

            return obj;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Process);
        }


    }
}
