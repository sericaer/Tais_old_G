using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Modder;
using RunData;

namespace TaisGodot.Scripts
{
	class WarnItem : Panel
	{
		public override void _Ready()
		{
			GetNode<Label>("Label").Text = Name;
		}

		internal void Refresh(List<Desc> descs)
		{

			var strInfo = String.Format("{0}\n-----------------\n{1}",
										Name+"_TITLE",
										String.Join("\n", descs.Select(x=>TranslateServerEx.Translate(x.Format, x.Params))));

			this.HintTooltip = strInfo;
		}
	}
}



