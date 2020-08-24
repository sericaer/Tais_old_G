using System;
using System.Collections.Generic;
using System.Reflection;

namespace Modder
{
    public class ModDataVisit
    {
        public static void InitVisitMap(Type type)
        {
            DataVisit.InitVisitMap(type);
        }

        public static void InitVisitData(object data)
        {
            DataVisit.SetVisitData(data);
        }

        internal static bool EnumerateVisit(string v)
        {
            throw new NotImplementedException();
        }

        internal static dynamic Get(string arg)
        {
            return DataVisit.Get(arg);
        }
    }
}
