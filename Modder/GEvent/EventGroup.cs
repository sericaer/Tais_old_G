using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Modder
{
    public class EventGroup
    {
        internal List<GEvent> _common = new List<GEvent>();
        internal List<GEvent> _depart = new List<GEvent>();
        internal List<GEvent> _pop = new List<GEvent>();

        internal static IEnumerable<(string mod, List<GEvent> events)> common
        {
            get
            {
                return Mod.modDict.Select(x => (x.Key, x.Value.eventGroup._common));
            }
        }

        internal static IEnumerable<(string mod, List<GEvent> events)> depart
        {
            get
            {
                return Mod.modDict.Select(x => (x.Key, x.Value.eventGroup._depart));
            }
        }

        internal static IEnumerable<(string mod, List<GEvent> events)> pop
        {
            get
            {
                return Mod.modDict.Select(x => (x.Key, x.Value.eventGroup._pop));
            }
        }

        internal static EventGroup Load(string modname, string path)
        {
            return new EventGroup()
            {
                _common = LoadSub(path + "/common"),
                _depart = LoadSub(path + "/depart"),
                _pop = LoadSub(path + "/pop")
            };
        }

        internal static IEnumerable<GEvent> Process((int y, int m, int d) date)
        {
            foreach (var gEvent in common.SelectMany(x => x.events))
            {
                if (gEvent.isValid(date))
                {
                    yield return gEvent;
                }
            }

            while (ModDataVisit.EnumerateVisit("depart"))
            {
                foreach (var gEvent in depart.SelectMany(x => x.events))
                {
                    if (gEvent.isValid(date))
                    {
                        yield return gEvent;
                    }
                }
            }
        }

        private static List<GEvent> LoadSub(string path)
        {
            List<GEvent> rslt = new List<GEvent>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var eventobj = new GEvent(file);
                rslt.Add(eventobj);
            }

            return rslt;
        }
    }
}
