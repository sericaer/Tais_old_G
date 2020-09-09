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
		[Signal]
		public delegate void FileItemSelected(string name);

		public override void _Ready()
		{
			GD.Print("ccc");
			foreach (var filePath in System.IO.Directory.EnumerateFiles(GlobalPath.save, "*.save"))
			{
				var saveItemPanel = (SaveFileItemPanel)ResourceLoader.Load<PackedScene>("res://Global/SaveLoadPanel/FileContainer/SaveFileItem.tscn").Instance();
				saveItemPanel.fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
				saveItemPanel.GetNode<Button>("HBoxContainer/ButtonLoad").Visible = GetNode<SaveLoadPanel>("../../../../").enableLoad;

				GetNode<VBoxContainer>("VBoxContainer").AddChild(saveItemPanel);
			}
		}

		private void TriggleFileItemSelectedSignal(string fileName)
		{
			EmitSignal(nameof(FileItemSelected), fileName);
		}
	}
}