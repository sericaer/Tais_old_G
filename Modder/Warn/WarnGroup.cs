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

        internal static IEnumerable<Warn> Process()
        {
            foreach (var warn in common.SelectMany(x => x.events))
            {
                if (warn.isValid())
                {
                    yield return warn;
                }
            }

            //while (ModDataVisit.EnumerateVisit("depart"))
            //{
            //    foreach (var gEvent in depart.SelectMany(x => x.events))
            //    {
            //        if (gEvent.isValid(date))
            //        {
            //            yield return gEvent;
            //        }
            //    }
            //}
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
