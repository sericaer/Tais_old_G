using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Modder;

namespace TaisGodot.Scripts
{
    public class OptionGroup : VBoxContainer
    {
        [Signal]
        public delegate void SelectedSignal();

        public OptionGroup()
        {
            var buttons = GetChildButton();
            if(buttons.Count() < gmObj.Count())
            {
                throw new Exception($"button count max is {buttons.Count()}, but real is {gmObj.Count()}");
            }

            for(int i=0; i<buttons.Count(); i++)
            {
                buttons[i].Text = TranslateServerEx.Translate(gmObj[i].desc.Format, gmObj[i].desc.Params);
                buttons[i].Connect("press", this, nameof(OnSelected), new Godot.Collections.Array() {i});
            }
        }

        private void OnSelected(int i)
        {
            gmObj[i].Selected();

            EmitSignal(nameof(SelectedSignal));
        }

        private Button[] GetChildButton()
        {
            var rslt = new List<Button>();
            for (int i = 0; i < GetChildCount(); i++)
            {
                var button = GetChild<Button>(i);
                if (button == null)
                {
                    continue;
                }

                rslt.Add(button);
            }
            return rslt.ToArray();
        }

        public GEvent.Option[] gmObj;
    }
}
