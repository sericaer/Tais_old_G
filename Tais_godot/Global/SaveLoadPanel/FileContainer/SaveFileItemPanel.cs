using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
	class SaveFileItemPanel : PanelContainer
	{
		internal string fileName;

		[Signal]
		internal delegate void LoadFileSignal(string name);

		public override void _Ready()
		{
			GetNode<Label>("HBoxContainer/Label").Text = fileName;
		}

		private void _on_ButtonDelete_pressed()
		{
			System.IO.File.Delete(GlobalPath.save + fileName + ".save");
			QueueFree();
		}


		private void _on_ButtonLoad_pressed()
		{
			EmitSignal(nameof(LoadFileSignal), fileName);
			// Replace with function body.
		}
	}
}
