using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunData
{
    public class Depart
    {

        public static Depart GetByColor(int r, int g, int b)
        {
            return all.SingleOrDefault(x => x.SameColor((r, g, b)));
        }

        public string name;
        public Reactive<int> popNum;

        public IEnumerable<Pop> pops
        {
            get
            {
                return Pop.all.Where(x => x.depart_name == name);
            }
        }

        internal (string name, int num)[] pops_init;


        internal static List<Depart> Init()
        {
            var all = new List<Depart>();

            all.Add(new Depart("JIXIAN",
                               (63, 72, 204),
                               new (string name, int num)[] { ("haoqiang", 3000), ("minhu", 60000), ("yinhu", 20000) }));
            return all;
        }

        internal static void DaysInc()
        {
            all.ForEach(depart =>
            {
                depart.popNum.value = depart.pops.Sum(x => x.num.value);
            });
        }

        private (int r, int g, int b) color;

        private static List<Depart> all
        {
            get
            {
                return Root.inst.departs;
            }
        }

        private Depart(string name, (int r, int g, int b) color, (string name, int num)[] pops_init)
        {
            this.name = name;
            this.color = color;
            this.pops_init = pops_init;

            this.popNum = new Reactive<int>(pops_init.Sum(x=>x.num));
        }

        private bool SameColor((int r, int g, int b) p)
        {
            return (color.r == p.r && color.g == p.g && color.b == p.b);
        }
    }
}
