using DataVisit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Date
    {
        public static Date inst
        {
            get
            {
                return Root.inst.date;
            }
        }

        public static void Inc()
        {
            if (inst.day.Value != 30)
            {
                inst.day.Value++;
            }
            else if (inst.month.Value != 12)
            {
                inst.day.Value = 1;
                inst.month.Value++;
                return;
            }
            else
            {
                inst.month.Value = 1;
                inst.year.Value++;
            }
        }



        [JsonProperty, DataVisitorProperty("year")]
        public SubjectValue<int> year;

        [JsonProperty, DataVisitorProperty("month")]
        public SubjectValue<int> month;

        [JsonProperty, DataVisitorProperty("day")]
        public SubjectValue<int> day;

        public ObservableValue<string> desc;

        public ObservableValue<int> total_days;

        //public int total_days
        //{
        //    get
        //    {
        //        return day.Value + (month.Value - 1) * 12 + (year.Value - 1) * 360;
        //    }
        //}

        public static Date Init()
        {
            return new Date();
        }

        //internal static void ToJson(ref JObject jObject)
        //{
        //    jObject.Add("date", JToken.FromObject(inst));
        //}

        //internal static void LoadJson(JObject jObject)
        //{
        //    inst = jObject.GetValue("date").ToObject<Date>();
        //}

        private Date()
        {
            year = new SubjectValue<int>(1);
            month = new SubjectValue<int>(1);
            day = new SubjectValue<int>(1);

            desc = Observable.CombineLatest(year.obs, month.obs, day.obs, (y, m, d) => $"{y}-{m}-{d}").ToOBSValue();
            total_days = Observable.CombineLatest(year.obs, month.obs, day.obs, (y, m, d) => d + m * 12 + y * 360).ToOBSValue();
        }
    }


}