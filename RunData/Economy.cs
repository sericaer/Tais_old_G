using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataVisit;

namespace RunData
{
    public class Economy
    {
        public static Economy inst
        {
            get
            {
                return Root.inst.economy;
            }
        }

        public static Economy Init()
        {
            return new Economy();
        }


        [DataVisitorProperty("economy.value")]
        public double currValue
        {
            get
            {
                return curr.value;
            }
            set
            {
                curr.value = value;
            }
        }
        
        public Reactive<double> curr;


        private Economy()
        {
            curr = new Reactive<double>(100);
        }
    }
}
