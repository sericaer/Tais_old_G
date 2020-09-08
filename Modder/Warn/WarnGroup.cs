using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Modder
{
    internal class WarnGroup
    {
        internal List<Warn> _common = new List<Warn>();
        internal List<Warn> _depart = new List<Warn>();
        internal List<Warn> _pop = new List<Warn>();

        internal static IEnumerable<(string mod, List<Warn> events)> common
        {
            get
            {
                return Mod.modDict.Select(x => (x.Key, x.Value.warnGroup._common));
            }
        }

        internal static IEnumerable<(string mod, List<Warn> events)> depart
        {
            get
            {
                return Mod.modDict.Select(x => (x.Key, x.Value.warnGroup._depart));
            }
        }

        internal static IEnumerable<(string mod, List<Warn> events)> pop
        {
            get
            {
                return Mod.modDict.Select(x => (x.Key, x.Value.warnGroup._pop));
            }
        }

        internal static WarnGroup Load(string modname, string path)
        {
            return new WarnGroup()
            {
                _common = LoadSub(path + "/common"),
                _depart = LoadSub(path + "/depart"),
                _pop = LoadSub(path + "/pop")
            };
        }

        internal static (string key, List<string> datas)[] Process()
        {
            var rslt = new List<(string, List<string>)>();
            foreach (var warn in common.SelectMany(x => x.events))
            {
                if (warn.isValid())
                {
                    rslt.Add((warn.key, new List<string>()));
                }
            }

            foreach (var warn in depart.SelectMany(x => x.events))
            {
                var departs = new List<string>();
                while (ModDataVisit.EnumerateVisit("depart"))
                {
                    if (warn.isValid())
                    {
                        departs.Add(ModDataVisit.Get("depart.name") as string);
                    }
                }

                if (departs.Count != 0)
                {
                    rslt.Add((warn.key, departs));
                }
            }

            return rslt.ToArray();
        }

        private static List<Warn> LoadSub(string path)
        {
            List<Warn> rslt = new List<Warn>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var warnobj = ModElementLoader.Load<Warn>(file, File.ReadAllText(file));
                warnobj.file = file;

                rslt.Add(warnobj);
            }

            return rslt;
        }
    }
}
