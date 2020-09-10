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
		[Signal]
		internal delegate void LoadSaveFile(string name);

		internal bool enableLoad;

		private NewSaveContainer newSaveContainter;
		private SaveFileContainer saveFileContainer;

		public override void _Ready()
		{
			newSaveContainter = GetNode<NewSaveContainer>("CenterContainer/PanelContainer/VBoxContainer/NewSaveContainer");
			saveFileContainer = GetNode<SaveFileContainer>("CenterContainer/PanelContainer/VBoxContainer/SaveFileContainer");

			newSaveContainter.Visible = !enableLoad;
			newSaveContainter.buttonConfirm.Connect("pressed", this, nameof(onTriggerSave));

			foreach(var fileItem in saveFileContainer.EnumSaveFileItems())
			{
				fileItem.buttonDelete.Connect("pressed", this, nameof(onTriggerDelete), new Godot.Collections.Array() { fileItem });
				fileItem.buttonLoad.Connect("pressed", this, nameof(onTriggerLoad), new Godot.Collections.Array() { fileItem });

				fileItem.buttonLoad.Visible = enableLoad;

			}
		}

		private void _on_ButtonCancel_pressed()
		{
			GD.Print("_on_ButtonCancel_pressed");
			QueueFree();
		}

		private void onTriggerSave()
		{
			var filePath = GlobalPath.save + newSaveContainter.fileNameEdit.Text + ".save";

			if (System.IO.File.Exists(filePath))
			{
				var MsgboxPanel = (MsgboxPanel)ResourceLoader.Load<PackedScene>("res://Global/MsgboxPanel/MsgboxPanel.tscn").Instance();
				MsgboxPanel.desc = "STATIC_SAVE_FILE_EXIT";
				MsgboxPanel.action = () =>
				{
					System.IO.File.WriteAllText(filePath, Root.Serialize());
					QueueFree();
				};

				AddChild(MsgboxPanel);
				return;
			}

			System.IO.File.WriteAllText(filePath, Root.Serialize());
			QueueFree();
		}

		private void onTriggerDelete(SaveFileItemPanel fileItem)
		{
			var filePath = GlobalPath.save + fileItem.Name + ".save";
			System.IO.File.Delete(filePath);

			fileItem.QueueFree();
		}

		private void onTriggerLoad(SaveFileItemPanel fileItem)
		{
			EmitSignal(nameof(LoadSaveFile), fileItem.Name);
			fileItem.QueueFree();
		}
	}
}
