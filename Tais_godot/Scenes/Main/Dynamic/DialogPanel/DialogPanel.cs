using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modder;

namespace TaisGodot.Scripts
{
    class DialogPanel : Panel
    {
        public DialogPanel()
        {
            Date.Pause();

            GetNode<Label>("Title").Text = TranslateServerEx.Translate(gEventObj.title.Format, gEventObj.title.Params);
            GetNode<Label>("Desc").Text = TranslateServerEx.Translate(gEventObj.desc.Format, gEventObj.desc.Params);

            var optionGroup = GetNode<OptionGroup>("options");
            optionGroup.gmObj = gEventObj.options;
        }

        private void _on_Selected_Signal()
        {
            QueueFree();
        }

        internal GEvent gEventObj;
    }
}
