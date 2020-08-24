using System;
using System.Collections.Generic;
using System.Reflection;

namespace Modder
{
    public class ModDataVisit
    {
        public static void InitVisitMap(Type type)
        {
            DataVisit.Visitor.InitVisitMap(type);
        }

        public static void InitVisitData(object data)
        {
            DataVisit.Visitor.SetVisitData(data);
        }

        internal static bool EnumerateVisit(string v)
        {
            throw new NotImplementedException();
        }

        internal static dynamic Get(string arg)
        {
            return DataVisit.Visitor.Get(arg);
        }
    }
}
