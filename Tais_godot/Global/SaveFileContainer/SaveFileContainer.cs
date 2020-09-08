using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
	class SaveFileContainer : ScrollContainer
	{
		public bool enableSelect;

		public SaveFileContainer()
		{
			enableSelect = false;
		}

		[Signal]
		public delegate void FileItemSelected(string name);

		public override void _Ready()
		{
			foreach (var filePath in System.IO.Directory.EnumerateFiles(GlobalPath.save, "*.save"))
			{
				var saveItemPanel = (SaveFileItemPanel)ResourceLoader.Load<PackedScene>("res://Global/SaveFileContainer/SaveFileItem.tscn").Instance();
				saveItemPanel.fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
				
				GetNode<VBoxContainer>("VBoxContainer").AddChild(saveItemPanel);

				saveItemPanel.GetNode<CheckBox>("HBoxContainer/CheckBox").Disabled = !enableSelect;

				if(!saveItemPanel.GetNode<CheckBox>("HBoxContainer/CheckBox").Disabled)
					saveItemPanel.GetNode<CheckBox>("HBoxContainer/CheckBox").Connect("toggled", this, nameof(TriggleFileItemSelectedSignal), new Godot.Collections.Array() { filePath });
			}
		}

		private void TriggleFileItemSelectedSignal(string fileName)
		{
			EmitSignal(nameof(FileItemSelected), fileName);
		}
	}
}
