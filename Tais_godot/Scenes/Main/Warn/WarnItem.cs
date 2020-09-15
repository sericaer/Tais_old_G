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
			GetNode<Label>("Label").Text = "WARN" + Name + "_TITLE";
		}

		internal void Refresh(Warn warn)
		{
			var strItem = warn.datas.Select(x =>
			{
				var split = x.Split("|");
				return string.Format(TranslateServerEx.Translate("WARN_" + Name + "_ITEM"), split);
			});

			var strInfo = TranslateServerEx.Translate("WARN_" + Name + "_DESC")
						  + "\n\n-----------------\n\n"
						  + string.Join("\n", strItem);

			this.HintTooltip = strInfo;
		}
	}
}



