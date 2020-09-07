using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Semantic;

namespace Modder
{
    public class Mod
    {
        public static Action<string> logger;

        public string name;
        public string path;

        public List<Language> languages;
        public EventGroup eventGroup;

        static Mod()
        {
            Visitor.GetValueFunc = ModDataVisit.Get;
            Visitor.SetValueFunc = ModDataVisit.Set;
        }

        public static IEnumerable<Mod> Enumerate()
        {
            foreach(var elem in modDict)
            {
                yield return elem.Value;
            }
        }

        public static Mod GetByName(string name)
        {
            return modDict[name];
        }

        public static void Load(string path)
        {
            modDict = new Dictionary<string, Mod>();

            foreach (var sub in System.IO.Directory.EnumerateDirectories(path))
            {
                var modname = System.IO.Path.GetFileName(sub);
                modDict.Add(modname, new Mod(modname, sub));
            }
        }

        public static IEnumerable<GEvent> Process((int y, int m, int d) date)
        {
            foreach (var eventObj in EventGroup.Process(date))
            {
                yield return eventObj;
            }

            //var testDialog = new GEvent();
            //testDialog._title = new GroupValue("EVENT_TEST_TITLE");
            //testDialog._desc = new GroupValue("EVENT_TEST_DESC");

            //testDialog.options = new GEvent.Option[]
            //{
            //    new GEvent.Option(){_desc = new GroupValue("OPTION_1_DESC"),
            //                        Selected = ()=>logger("Select OPTION_1_DESC") },
            //    new GEvent.Option(){_desc = new GroupValue("OPTION_2_DESC"),
            //                        Selected = ()=>logger("Select OPTION_2_DESC") },
            //    new GEvent.Option(){_desc = new GroupValue("OPTION_3_DESC"),
            //                        Selected = ()=>logger("Select OPTION_3_DESC") },
            //};

            //showDialogAction(testDialog);
        }

        internal Mod(string modname, string path)
        {
            this.name = modname;
            this.path = path;

            this.languages = Language.Load(path + "/languages");
            this.eventGroup = EventGroup.Load(modname, path + "/events");

        }

        internal static Dictionary<string, Mod> modDict;

    }

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
