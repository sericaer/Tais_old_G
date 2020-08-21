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

            var optionButton = GetNode<OptionButton>("options");
            for (int i = 0; i < gEventObj.options.Count(); i++)
            {
                var optionObj = gEventObj.options[i];

                optionButton.SetItemText(i, TranslateServerEx.Translate(optionObj.desc.Format, optionObj.desc.Params));
            }

            for (int i = optionButton.GetItemCount() - 1; i >= gEventObj.options.Count(); i++)
            {
                optionButton.RemoveItem(i);
            }
        }

        private void _on_OptionButton_Selected(int idx)
        {
            gEventObj.options[idx].Selected();
            QueueFree();

            Date.UnPause();
        }

        internal GEvent gEventObj;
    }
}
