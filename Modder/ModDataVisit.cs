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

        internal static bool EnumerateVisit(string key)
        {
            var rslt = DataVisit.Visitor.EnumerateVisit(key, ref pos);
            if(!rslt)
            {
                pos = null;
            }

            return rslt;
        }

        internal static dynamic Get(string arg)
        {
            return DataVisit.Visitor.Get(arg);
        }

        internal static void Set(string arg, object value)
        {
            DataVisit.Visitor.Set(arg, value);
        }

        internal static DataVisit.Visitor.Pos pos = null;

    }
}
