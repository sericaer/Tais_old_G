using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
	class SaveLoadPanel : Panel
	{
		internal bool enableLoad;

		public override void _Ready()
		{
			GD.Print("bbb");

			GetNode<Control>("CenterContainer/PanelContainer/VBoxContainer/NewSaveContainer").Visible = !enableLoad;
			//GetNode<SaveFileContainer>("CenterContainer/PanelContainer/VBoxContainer/SaveFileContainer").enuProcess = SaveFileContainer.Process.Save;
		}

		private void _on_ButtonConfirm_pressed()
		{
			var filePath = GlobalPath.save + GetNode<TextEdit>("CenterContainer/PanelContainer/VBoxContainer/NewSaveContainer/TextEdit").Text + ".save";
			if(System.IO.File.Exists(filePath))
			{
				var MsgboxPanel = (MsgboxPanel)ResourceLoader.Load<PackedScene>("res://Global/MsgboxPanel/MsgboxPanel.tscn").Instance();
				MsgboxPanel.desc = "STATIC_SAVE_FILE_EXIT";
				MsgboxPanel.action = () =>
				{
					SaveFile(filePath);
					QueueFree();
				};

				AddChild(MsgboxPanel);
				return;
			}

			SaveFile(filePath);
			QueueFree();
		}

		private void _on_ButtonCancel_pressed()
		{
			GD.Print("_on_ButtonCancel_pressed");
			QueueFree();
		}

		private void SaveFile(string path)
		{
			var content = Root.Serialize();
			System.IO.File.WriteAllText(path, content);
		}
	}
}
