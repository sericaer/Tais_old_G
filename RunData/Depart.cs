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

        public static List<Depart> all;

        public static IEnumerable<Depart> Enumerate()
        {
            foreach (var elem in all)
            {
                yield return elem;
            }
        }

        public static Depart GetByColor(int r, int g, int b)
        {
            return all.SingleOrDefault(x => x.SameColor((r, g, b)));
        }

        public string name;
        public IEnumerable<Pop> pops
        {
            get
            {
                return Pop.all.Where(x => x.depart_name == name);
            }
        }

        internal (string name, int num)[] pops_init;

        internal static void Init()
        {
            all = new List<Depart>();

            all.Add(new Depart("JIXIAN", 
                               (63, 72, 204), 
                               new (string name, int num)[]{ ("HAOQIANG", 3000), ("MINHU", 60000) }));
        }


        private (int r, int g, int b) color;

        private Depart(string name, (int r, int g, int b) color, (string name, int num)[] pops_init)
        {
            this.name = name;
            this.color = color;
            this.pops_init = pops_init;
        }

        private bool SameColor((int r, int g, int b) p)
        {
            return (color.r == p.r && color.g == p.g && color.b == p.b);
        }

        internal static void Exit()
        {
            all = null;
        }
    }
}
