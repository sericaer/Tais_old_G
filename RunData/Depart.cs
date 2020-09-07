using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DataVisit;
using Newtonsoft.Json;

namespace RunData
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Depart
    {
        [JsonProperty]
        public string name;
        public ObservableValue<int> popNum;

        [JsonProperty, DataVisitorProperty("crop_grown")]
        public SubjectValue<double> cropGrown;

        public IEnumerable<Pop> pops
        {
            get
            {
                return Pop.all.Where(x => x.depart_name == name);
            }
        }

        internal Define.DepartDef def
        {
            get
            {
                return Root.inst.def.departs[name];
            }
        }

        internal static List<Depart> all
        {
            get
            {
                return Root.inst.departs;
            }
        }

        public static Depart GetByColor(int r, int g, int b)
        {
            return all.SingleOrDefault(x => x.SameColor((r, g, b)));
        }

        internal static List<Depart> Init(IEnumerable<string> departs)
        {
            return departs.Select(x => new Depart(x)).ToList();
        }

        internal static void DaysInc()
        {
            all.ForEach(x =>
            {
                if (Date.inst >= Root.inst.def.crop.growStartDay && Date.inst <= Root.inst.def.crop.harvestDay)
                {
                    x.cropGrown.Value += Root.inst.def.crop.growSpeed;
                }
                else
                {
                    x.cropGrown.Value = 0;
                }
            });

        }

        internal Depart(string name)
        {
            this.name = name;
            this.cropGrown = new SubjectValue<double>(0);
        }

        [JsonConstructor]
        private Depart()
        {

        }

        [OnDeserialized]
        private void InitObservableData(StreamingContext context)
        {
            popNum = Observable.CombineLatest(pops.Where(x => x.def.is_collect_tax).Select(x => x.num.obs),
                                  (IList<double> nums) => nums.Sum(y => (int)y)).ToOBSValue();
        }

        private bool SameColor((int r, int g, int b) p)
        {
            return (def.color.r == p.r && def.color.g == p.g && def.color.b == p.b);
        }
    }
}
