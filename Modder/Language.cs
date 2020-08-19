using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modder
{
    public class Language
    {
        public string locale;
        public Dictionary<string, string> dict;

        internal static List<Language> Load(string path)
        {
            List<Language> rslt = new List<Language>();

            if (System.IO.Directory.Exists(path))
            {
                foreach (var sub in System.IO.Directory.EnumerateDirectories(path))
                {
                    rslt.Add(new Language(sub));
                }
            }

            return rslt;
        }

        private Language(string path)
        {
            locale = System.IO.Path.GetFileNameWithoutExtension(path);

            var paths = System.IO.Directory.EnumerateFiles(path);
            if(!paths.Any())
            {
                dict = new Dictionary<string, string>();
                return;
            }

            foreach (var sub in paths)
            {
                dict = LoadLanguageElement(path);
            }
        }

        private Dictionary<string, string> LoadLanguageElement(string path)
        {
            Dictionary<string, string> rslt = new Dictionary<string, string>();

            foreach (var file in System.IO.Directory.EnumerateFiles(path, "*.txt"))
            {
                var header = System.IO.Path.GetFileNameWithoutExtension(file).ToUpper();

                var lines = System.IO.File.ReadAllLines(file);
                for (int i = 0; i < lines.Count(); i++)
                {
                    if (lines[i].Length == 0)
                    {
                        continue;
                    }

                    var splits = lines[i].Split(':');
                    if (splits.Count() != 2)
                    {
                        throw new Exception($"parse file error! must be XXX:XXX mode in {file}:{i}");
                    }

                    rslt.Add(header + "_" + splits[0], splits[1]);
                }
            }

            return rslt;
        }
    }
}
