using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Parser.Semantic;

namespace Modder
{
    public class InitSelect
    {
        public bool isFirst;
        public Desc desc;
        public Option[] options;

        private string file;

        public InitSelect(string file)
        {
            var sematic = ModElementLoader.Load<InitSelectSematic>(file, File.ReadAllText(file));
            isFirst = sematic.isFirst != null && sematic.isFirst.Value;

            desc = new Desc(null, Path.GetFileNameWithoutExtension(file));
            options = sematic.options.Select((v, i) => new Option { semantic = v, index = i + 1, ownerName = Path.GetFileNameWithoutExtension(file) }).ToArray();
        }

        public static IEnumerable<(string mod, InitSelect initSelect)> Enumerate()
        {
            foreach(var mod in Mod.modDict)
            {
                foreach(var initSelect in mod.Value.initSelects)
                {
                    yield return (mod.Key, initSelect);
                }
            }
        }

        internal static List<InitSelect> Load(string modname, string path)
        {
            List<InitSelect> rslt = new List<InitSelect>();

            if (!Directory.Exists(path))
            {
                return rslt;
            }

            foreach (var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                rslt.Add(new InitSelect(file));
            }

            return rslt;
        }
    }

    public class InitSelectSematic
    {
        [SemanticProperty("is_first")]
        public bool? isFirst;

        [SemanticPropertyArray("option")]
        public List<Parser.Semantic.Option> options;
    }
}
