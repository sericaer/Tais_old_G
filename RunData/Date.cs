using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Date
    {
        public static Date inst;

        public static void Inc()
        {
            if (inst.day != 30)
            {
                inst.day++;
            }
            else if (inst.month != 12)
            {
                inst.day = 1;
                inst.month++;
                return;
            }
            else
            {
                inst.month = 1;
                inst.year++;
            }

            inst.desc.value = inst.ToString();
        }

        public Reactive<string> desc;

        [JsonProperty]
        public int year;

        [JsonProperty]
        public int month;

        [JsonProperty]
        public int day;

        public int total_days
        {
            get
            {
                return day + (month - 1) * 12 + (year - 1) * 360;
            }
        }

        public override string ToString()
        {
            return $"{year}-{month}-{day}";
        }

        internal static void Init()
        {
            inst = new Date();
        }

        internal static void Exit()
        {
            inst = null;
        }
        
        internal static void ToJson(ref JObject jObject)
        {
            jObject.Add("date", JToken.FromObject(inst));
        }

        internal static void LoadJson(JObject jObject)
        {
            inst = jObject.GetValue("date").ToObject<Date>();
        }

        private Date()
        {
            year = 1;
            month = 1;
            day = 1;

            desc = new Reactive<string>(ToString());
        }
    }

    public class Reactive<T>
    {
        public Reactive(T value)
        {
            this.value = value;
        }

        public void Bind(Action<string> act)
        {
            _act = act;
            _act.Invoke(_value.ToString());
        }

        public T value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == null || !_value.Equals(value))
                {
                    _value = value;
                    _act?.Invoke(_value.ToString());
                }
                
            }
        }

        internal T _value;
        internal Action<string> _act;
    }
}