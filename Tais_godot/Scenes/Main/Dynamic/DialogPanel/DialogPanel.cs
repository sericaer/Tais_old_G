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

			//GetNode<Label>("Title").Text = TranslateServerEx.Translate(gEventObj.title.Format, gEventObj.title.Params);
			//GetNode<Label>("Desc").Text = TranslateServerEx.Translate(gEventObj.desc.Format, gEventObj.desc.Params);

			//var optionGroup = GetNode<OptionGroup>("options");
			//optionGroup.gmObj = gEventObj.options;
		}

		private void _on_Selected_Signal()
		{
			
		}

		private void _on_Button_pressed(int extra_arg_0)
		{
			// Replace with function body.
			GD.Print(extra_arg_0);
			QueueFree();

			Date.UnPause();
		}


		internal GEvent gEventObj;
	}
}
