using DataVisit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Runtime.Serialization;
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
                inst.day.Value = 1;
                inst.year.Value++;
            }
        }

        public static bool operator ==(Date l, (int? year, int? month, int? day) r)
        {
            if(r.year != null && l.year.Value != r.year.Value)
            {
                return false;
            }
            if (r.month != null && l.month.Value != r.month.Value)
            {
                return false;
            }
            if (r.day != null && l.day.Value != r.day.Value)
            {
                return false;
            }

            return true;
        }

        public static bool operator >(Date l, (int? year, int? month, int? day) r)
        {
            if (r.year != null)
            {
                if(l.year.Value > r.year.Value)
                    return true;
                if (l.year.Value < r.year.Value)
                    return false;
            }
            if (r.month != null)
            {
                if(l.month.Value > r.month.Value)
                    return true;
                if (l.month.Value < r.month.Value)
                    return false;
            }
            if (r.day != null)
            {
                if (l.day.Value > r.day.Value)
                    return true;
                if (l.day.Value < r.day.Value)
                    return false;
            }

            return false;
        }

        public static bool operator <(Date l, (int? year, int? month, int? day) r)
        {
            return !(l > r || l == r);
        }

        public static bool operator <= (Date l, (int? year, int? month, int? day) r)
        {
            return l < r || l == r;
        }

        public static bool operator >=(Date l, (int? year, int? month, int? day) r)
        {
            return l > r || l == r;
        }

        public static bool operator !=(Date l, (int? year, int? month, int? day) r)
        {
            return !(l == r);
        }

        public static bool operator ==(Date l, Date r)
        {
            return l == (r.year.Value, r.month.Value, r.day.Value);
        }

        public static bool operator !=(Date l, Date r)
        {
            return !(l==r);
        }

        public static bool operator >(Date l, Date r)
        {
            return l > (r.year.Value, r.month.Value, r.day.Value);
        }

        public static bool operator <(Date l, Date r)
        {
            return !(l > r || l == r);
        }

        public static bool operator >=(Date l, Date r)
        {
            return l > r || l == r;
        }

        public static bool operator <=(Date l, Date r)
        {
            return l < r || l == r;
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
            return new Date(1,1,1);
        }
    
        public static (int y, int m, int d) Value
        {
            get
            {
                return (inst.year.Value, inst.month.Value, inst.day.Value);
            }
        }

        //internal static void ToJson(ref JObject jObject)
        //{
        //    jObject.Add("date", JToken.FromObject(inst));
        //}

        //internal static void LoadJson(JObject jObject)
        //{
        //    inst = jObject.GetValue("date").ToObject<Date>();
        //}

        private Date(int y, int m, int d)
        {
            year = new SubjectValue<int>(y);
            month = new SubjectValue<int>(m);
            day = new SubjectValue<int>(d);
        }

        [JsonConstructor]
        private Date()
        {
        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            desc = Observable.CombineLatest(year.obs, month.obs, day.obs, (y, m, d) => $"{y}-{m}-{d}").ToOBSValue();
            total_days = Observable.CombineLatest(year.obs, month.obs, day.obs, (y, m, d) => d + (m - 1) * 12 + (y - 1) * 360).ToOBSValue();
        }
    }


}