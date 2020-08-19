using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modder
{
    public class Mod
    {
        public string name;
        public string path;

        public List<Language> languages;

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
            foreach (var sub in System.IO.Directory.EnumerateDirectories(path))
            {
                var modname = System.IO.Path.GetFileName(sub);
                modDict.Add(modname, new Mod(modname, sub));
            }
        }

        public Mod(string modname, string path)
        {
            this.name = modname;
            this.path = path;

            this.languages = Language.Load(path + "/languages");
        }

        private static Dictionary<string, Mod> modDict = new Dictionary<string, Mod>();

    }
}
