using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunData
{
    public class Economy
    {
        public static void Init()
        {
            inst = new Economy();
        }

         
        public Reactive<double> curr;

        public static Economy inst;

        private Economy()
        {
            curr = new Reactive<double>(100);
        }
    }
}
