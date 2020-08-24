using System;
using System.Collections.Generic;
using System.Reflection;

namespace Modder
{
    public class ModDataVisit
    {
        public class Pos
        {
            internal int index;
        }

        public static void InitVisitMap(List<Type> types)
        {

        }

        public static void InitVisitData(Dictionary<string, List<object>> dictObjs)
        {

        }

        public static void InitVisitData(List<object> obj)
        {

        }

        public static object Get(string raw)
        {
            throw new NotImplementedException();

            int intRslt;
            if (int.TryParse(raw, out intRslt))
            {
                return intRslt;
            }

            double doubleRslt;
            if (double.TryParse(raw, out doubleRslt))
            {
                return doubleRslt;
            }

            if (!raw.Contains("."))
            {
                return raw;
            }

            var splits = raw.Split('.');
            if (splits.Length < 2)
            {
                throw new Exception("error value " + raw);
            }

            ReflectionInfo[] reflections;
            if (!visitMap.TryGetValue(raw, out reflections))
            {
                throw new Exception($"can not find data with visit property:{raw}");
            }

            reflections[0].GetType();
        }

        internal static bool VisitNextDepart(ref Pos pos)
        {
            if(pos == null)
            {
                pos = new Pos() { index = 0 };
            }
            else
            {
                pos.index++;
            }
            
            if (pos.index + 1 > departs.Count)
            {
                return false;
            }

            return true;
        }

        internal static bool VisitNextPop(ref Pos pos)
        {
            if (pos == null)
            {
                pos = new Pos() { index = 0 };
            }
            else
            {
                pos.index++;
            }

            if (pos.index + 1 > pops.Count)
            {
                return false;
            }

            return true;
        }

        static List<object> departs;
        static List<object> pops;

        static Dictionary<Type, object> dictObj;

        static Dictionary<string, ReflectionInfo[]> visitMap;
    }
}
