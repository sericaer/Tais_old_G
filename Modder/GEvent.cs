using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;
using Parser.Semantic;
using Parser.Syntax;

namespace Modder
{
    public class GEvent
    {
        public Title title;
        public Desc desc;
        public Option[] options;

        internal string file;

        internal Date date;
        internal Trigger trigger;
        internal Occur occur;

        internal GEventParse parse;

        public bool isValid((int y, int m, int d) date)
        {
            return (this.date.isEqual(date) && trigger.isTrue() && occur.isTrue());
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

        public GEvent(string file)
        {
            this.file = file;

            this.parse = ModElementLoader.Load<GEventParse>(file, File.ReadAllText(file));

            this.title = new Title(parse.title, Path.GetFileNameWithoutExtension(file)); 
            this.desc = new Desc(parse.desc, Path.GetFileNameWithoutExtension(file));  
            this.options = parse.options.Select((v, i) => new Option { semantic = v, index = i + 1, owner = this }).ToArray();
            this.date = new Date(parse.date);
            this.trigger = new Trigger(parse.trigger);
            this.occur = new Occur(parse.occur);
        }


        public class GEventParse
        {
            [SemanticPropertyArray("option")]
            public List<Parser.Semantic.Option> options;

            [SemanticProperty("trigger")]
            public Condition trigger;

            [SemanticProperty("occur")]
            internal int? occur;

            [SemanticProperty("title")]
            internal GroupValue title;

            [SemanticProperty("desc")]
            internal GroupValue desc;

            [SemanticProperty("date")]
            internal Parser.Semantic.Date date;
        }

        public class Date
        {
            internal Date(Parser.Semantic.Date raw)
            {
                this.raw = raw;
            }

            internal bool isEqual((int year, int month, int day) date)
            {
                if(raw == null)
                {
                    return true;
                }
                
                if(raw.year != null && raw.year != date.year)
                {
                    return false;
                }
                if (raw.month != null && raw.month != date.month)
                {
                    return false;
                }
                if (raw.day != null && raw.day != date.day)
                {
                    return false;
                }

                return true;
            }

            Parser.Semantic.Date raw;
        }

        public abstract class TEXT
        {
            public string Format;
            public string[] Params;

            internal TEXT(GroupValue groupValue, string defaultValue)
            {
                if (groupValue == null)
                {
                    Format = defaultValue;
                    return;
                } 

                Format = groupValue.First().ToString();
                Params = groupValue.Skip(1).Select(x => x.ToString()).ToArray();
            }
        }

        public class Title : TEXT
        {
            public Title(GroupValue groupValue, string fileName) : base(groupValue, $"{fileName}_TITLE")
            {

            }
        }

        public class Desc : TEXT
        {
            public Desc(GroupValue groupValue, string fileName) : base(groupValue, $"{fileName}_DESC")
            {

            }
        }

        public class Option
        {
            public Desc desc
            {
                get
                {
                    return new Desc(semantic.desc, Path.GetFileNameWithoutExtension(owner.file), index);
                }
            }

            public void Selected()
            {
                semantic.set.Do();
            }

            public (string op, string left, SingleValue right)[] sets
            {
                get
                {
                    return semantic.set.info;
                }
            }

            internal GEvent owner;
            internal int index;
            internal Parser.Semantic.Option semantic;

            public class Desc : TEXT
            {
                public Desc(GroupValue groupValue, string fileName, int index) : base(groupValue, $"{fileName}_OPTION_{index}_DESC")
                {

                }
            }
        }

        internal class Trigger
        {
            private Condition raw;

            public Trigger(Condition raw)
            {
                if(raw == null)
                {
                    throw new Exception("event must have trigger");
                }

                this.raw = raw;
            }

            internal bool isTrue()
            {
                return raw.Rslt();
            }
        }
    }
}
