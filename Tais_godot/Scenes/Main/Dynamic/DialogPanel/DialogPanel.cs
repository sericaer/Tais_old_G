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
			SpeedContrl.Pause();
		}

		public override void _Ready()
		{
			GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/Title").Text = TranslateServerEx.Translate(gEventObj.title.Format, gEventObj.title.Params);
			GetNode<RichTextLabel>("CenterContainer/PanelContainer/VBoxContainer/Desc").Text = TranslateServerEx.Translate(gEventObj.desc.Format, gEventObj.desc.Params);

			var buttons = GetChildButton();
			if (buttons.Count() < gEventObj.options.Count())
			{
				throw new Exception($"button count max is {buttons.Count()}, but real is {gEventObj.options.Count()}");
			}

			for (int i = 0; i < gEventObj.options.Count(); i++)
			{
				buttons[i].Visible = true;
				buttons[i].Text = TranslateServerEx.Translate(gEventObj.options[i].desc.Format, gEventObj.options[i].desc.Params);
			}

		}
		private void _on_Button_pressed(int index)
		{
			gEventObj.options[index].Selected();

			QueueFree();

			SpeedContrl.UnPause();
		}


		private Button[] GetChildButton()
		{
			var rslt = new List<Button>();
			var buttonContainer = GetNode("CenterContainer/PanelContainer/VBoxContainer/OptionsContainer");
			for (int i = 0; i < buttonContainer.GetChildCount(); i++)
			{
				var button = buttonContainer.GetChild<Button>(i);
				if (button == null)
				{
					continue;
				}

				rslt.Add(button);
			}
			return rslt.ToArray();
		}

		internal GEvent gEventObj;
	}
}
