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
		internal IEnumerable<SaveFileItemPanel> EnumSaveFileItems()
		{
			return GetNode<VBoxContainer>("VBoxContainer").GetChildren<SaveFileItemPanel>();
		}

		public override void _Ready()
		{
			foreach (var filePath in System.IO.Directory.EnumerateFiles(GlobalPath.save, "*.save"))
			{
				var saveItemPanel = (SaveFileItemPanel)ResourceLoader.Load<PackedScene>("res://Global/SaveLoadPanel/FileContainer/SaveFileItem.tscn").Instance();
				saveItemPanel.Name = System.IO.Path.GetFileNameWithoutExtension(filePath);

				GetNode<VBoxContainer>("VBoxContainer").AddChild(saveItemPanel);
			}
		}
	}
}
