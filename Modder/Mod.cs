using System;
using System.Collections.Generic;
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
            foreach (var sub in System.IO.Directory.EnumerateDirectories(path))
            {
                var modname = System.IO.Path.GetFileName(sub);
                modDict.Add(modname, new Mod(modname, sub));
            }
        }

        public static IEnumerable<GEvent> Process((int y, int m, int d) date)
        {
            foreach (var eventObj in GEvent.Process(date))
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
            GEvent.Load(modname, path + "/events");
        }

        private static Dictionary<string, Mod> modDict = new Dictionary<string, Mod>();

    }
}
