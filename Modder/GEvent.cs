using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser.Semantic;

namespace Modder
{
    public class GEvent
    {
        internal static void Load(string mod, string path)
        {
            LoadSub(mod, path + "common/", ref common);
        }

        private static void LoadSub(string mod, string path, ref List<(string mod, List<GEvent> events)> eventGroup)
        {
            List<GEvent> events = new List<GEvent>();
            foreach(var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                events.Add(SemanticParser.ParserFile<GEvent>(file));
            }

            eventGroup.Add((mod, events));
        }

        public class Desc
        {
            public string Format;
            public string[] Params;

            internal Desc()
            {

            }

            internal Desc(GroupValue groupValue)
            {
                Format = groupValue.Take(1).ToString();
                Params = groupValue.Skip(1).Select(x => x.ToString()).ToArray();
            }
        }

        public class Option
        {
            public Desc desc
            {
                get
                {
                    return new Desc(_desc);
                }
            }

            [SemanticProperty("select")]
            public Action Selected;

            [SemanticProperty("desc")]
            internal GroupValue _desc;
        }



        public Desc title
        {
            get
            {
                return new Desc(_title);
            }
        }

        public Desc desc
        {
            get
            {
                return new Desc(_desc);
            }
        }

        [SemanticPropertyArray("option")]
        public Option[] options;

        [SemanticProperty("trigger")]
        internal Condition trigger;

        [SemanticProperty("occur")]
        internal Occur occur;

        [SemanticProperty("titile")]
        internal GroupValue _title;

        [SemanticProperty("desc")]
        internal GroupValue _desc;

        internal static List<(string mod, List<GEvent> events)> common;
        internal static List<(string mod, List<GEvent> events)> depart;
        internal static List<(string mod, List<GEvent> events)> pop;

        internal static IEnumerable<GEvent> Process()
        {
            foreach(var gEvent in common.SelectMany(x=>x.events))
            {
                if(gEvent.trigger.Rslt() && gEvent.occur.Rslt())
                {
                    yield return gEvent;
                }
            }
        }

        internal GEvent()
        {
            trigger = new ConditionDefault(false);
            occur = new Occur(false);
        }
    }
}
