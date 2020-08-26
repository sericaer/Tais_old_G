using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;
using Parser.Semantic;

namespace Modder
{
    public class GEvent
    {
        internal static void Load(string mod, string path)
        {
            LoadSub(mod, path + "/common", ref common);
            LoadSub(mod, path + "/depart", ref depart);
            LoadSub(mod, path + "/pop", ref pop);
        }

        private static void LoadSub(string mod, string path, ref List<(string mod, List<GEvent> events)> eventGroup)
        {
            if (!Directory.Exists(path))
            {
                return;
            }

            List<GEvent> events = new List<GEvent>();
            foreach(var file in Directory.EnumerateFiles(path, "*.txt"))
            {
                var eventobj = ModElementLoader.Load<GEvent>(file, File.ReadAllText(file));
                eventobj.file = file;
                events.Add(eventobj);
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
                Format = groupValue.First().ToString();
                Params = groupValue.Skip(1).Select(x => x.ToString()).ToArray();
            }
        }

        public class Option
        {
            public Desc desc
            {
                get
                {
                    if(semantic.desc == null)
                    {
                        return new Desc() { Format = $"{Path.GetFileNameWithoutExtension(owner.file)}_OPTION_{index}_DESC" };
                    }

                    return new Desc(semantic.desc);
                }
            }

            public void Selected()
            {
                semantic.set.Do();
            }

            public (string right, string left)[] sets
            {
                get
                {
                    return semantic.set.list.ToArray();
                }
            }

            internal GEvent owner;
            internal int index;
            internal Parser.Semantic.Option semantic;
        }

        public Desc title
        {
            get
            {
                if(_title == null)
                {
                    return new Desc() { Format = $"{Path.GetFileNameWithoutExtension(file)}_TITLE" };
                }

                return new Desc(_title);
            }
        }

        public Desc desc
        {
            get
            {
                if (_desc == null)
                {
                    return new Desc() { Format = $"{Path.GetFileNameWithoutExtension(file)}_DESC" };
                }

                return new Desc(_desc);
            }
        }


        public Option[] options
        {
            get
            {
                var rslt = new List<Option>();
                for(int i=0; i< _options.Count; i++)
                {

                    rslt.Add(new Option() { semantic = _options[i], index = i + 1, owner = this });
                }

                return rslt.ToArray();
            }
        }

        public bool isOccur
        {
            get
            {
                return Tools.GRandom.isOccur(100/+_occur);
            }
        }

        internal string file;

        [SemanticPropertyArray("option")]
        public List<Parser.Semantic.Option> _options;

        [SemanticProperty("trigger")]
        public Condition trigger;

        [SemanticProperty("occur")]
        internal int _occur;

        [SemanticProperty("title")]
        internal GroupValue _title;

        [SemanticProperty("desc")]
        internal GroupValue _desc;

        internal static List<(string mod, List<GEvent> events)> common = new List<(string mod, List<GEvent> events)>();
        internal static List<(string mod, List<GEvent> events)> depart = new List<(string mod, List<GEvent> events)>();
        internal static List<(string mod, List<GEvent> events)> pop = new List<(string mod, List<GEvent> events)>();

        internal static IEnumerable<GEvent> Process()
        {
            foreach (var gEvent in common.SelectMany(x => x.events))
            {
                if (gEvent.trigger.Rslt() && gEvent.isOccur)
                {
                    yield return gEvent;
                }
            }

            //while (ModDataVisit.EnumerateVisit("depart"))
            //{
            //    foreach (var gEvent in depart.SelectMany(x => x.events))
            //    {
            //        if (gEvent.trigger.Rslt() && gEvent.isOccur)
            //        {
            //            yield return gEvent;
            //        }
            //    }
            //}

            //DataVisit.ForechPop(() =>
            //{
            //    foreach (var gEvent in depart.SelectMany(x => x.events))
            //    {
            //        if (gEvent.trigger.Rslt() && gEvent.occur.Rslt())
            //        {
            //            yield return gEvent;
            //        }
            //    }
            //});
        }

        //internal static IEnumerable<GEvent> Process(object rootObj, IEnumerable<object> departObjs, IEnumerable<object> popObjs)
        //{
        //    foreach(var gEvent in common.SelectMany(x=>x.events))
        //    {
        //        if (gEvent.trigger.Rslt() && gEvent.occur.Rslt())
        //        {
        //            yield return gEvent;
        //        }
        //    }

        //    foreach(var departObj in departObjs)
        //    {
        //        //DataVisit.SetObj("depart", departObj);
        //        foreach (var gEvent in depart.SelectMany(x => x.events))
        //        {
        //            if (gEvent.trigger.Rslt() && gEvent.occur.Rslt())
        //            {
        //                yield return gEvent;
        //            }
        //        }
        //    }

        //    foreach (var popObj in popObjs)
        //    {
        //        //DataVisit.SetObj("pop", popObj);
        //        foreach (var gEvent in pop.SelectMany(x => x.events))
        //        {
        //            if (gEvent.trigger.Rslt() && gEvent.occur.Rslt())
        //            {
        //                yield return gEvent;
        //            }
        //        }
        //    }
        //}

        public GEvent()
        {
            trigger = new ConditionDefault(false);
        }
    }
}
