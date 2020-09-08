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
        internal EventGroup eventGroup;
        internal WarnGroup warnGroup;

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

        public static IEnumerable<GEvent> EventProcess((int y, int m, int d) date)
        {
            foreach (var eventObj in EventGroup.Process(date))
            {
                yield return eventObj;
            }
        }

        public static IEnumerable<(string key, List<string> datas)> WarnProcess()
        {
            foreach (var warnObj in WarnGroup.Process())
            {
                yield return (warnObj.key, warnObj.datas);
            }
        }

        internal Mod(string modname, string path)
        {
            this.name = modname;
            this.path = path;

            this.languages = Language.Load(path + "/languages");
            this.eventGroup = EventGroup.Load(modname, path + "/events");
            this.warnGroup = WarnGroup.Load(modname, path + "/warns");
        }

        internal static Dictionary<string, Mod> modDict;

    }
}
