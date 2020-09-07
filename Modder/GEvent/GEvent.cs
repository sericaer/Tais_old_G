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
    public partial class GEvent
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
    }
}
