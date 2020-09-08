using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
	class MsgboxPanel : Panel
	{
		public string desc;
		public Action action;

		public override void _Ready()
		{
			GetNode<RichTextLabel>("CenterContainer/PanelContainer/VBoxContainer/Desc").Text = desc;
		}

		private void _on_ButtonConfirm_pressed()
		{
			action?.Invoke();
			QueueFree();
		}

		private void _on_ButtonCancel_pressed()
		{
			QueueFree();
		}
	}
}


