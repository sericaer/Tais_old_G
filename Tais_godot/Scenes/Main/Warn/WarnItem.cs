using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using RunData;

namespace TaisGodot.Scripts
{
	class WarnItem : Panel
	{
		public override void _Ready()
		{
            GetNode<Label>("Label").Text = Name;
		}

        internal void Refresh(List<string> datas)
        {
            GetNode<Label>("Label").HintTooltip = string.Format("\n", datas);
        }
    }
}



