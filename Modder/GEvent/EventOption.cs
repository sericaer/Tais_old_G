using System.IO;

namespace Modder
{
    public class EventOption : Option
    {
        public EventOption(string name, Parser.Semantic.Option semantic, int i)
        {
            this.semantic = semantic;
            this.index = i + 1;
            this.ownerName = name;
        }

        public override void Selected()
        {
            base.Selected();

            if (semantic.process_start != null)
            {
                ModDataVisit.Set("process.start", semantic.process_start);
            }
            if(semantic.process_cancel != null)
            {
                ModDataVisit.Set("process.cancel", semantic.process_cancel);
            }
        }
    }
}