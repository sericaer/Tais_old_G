using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RunData
{
    public static class Extensions
    {
        //public static double? calcExpectTax(this Pop pop)
        //{
        //    if(!pop.def.is_collect_tax)
        //    {
        //        return null;
        //    }

        //    return pop.num.value * 0.001;
        //    //return calcExpectTax(pop, Economy.inst.popTaxLevel);
        //}

        public static ObservableValue<T> ToOBSValue<T>(this IObservable<T> obs)
        {
            return new ObservableValue<T>(obs);
        }

    }
}
