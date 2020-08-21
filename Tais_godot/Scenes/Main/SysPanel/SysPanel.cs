using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
	class SysPanel : Panel
	{
		private void _on_ButtonSave_button_up(string fileName)
		{
			Root.Load(GlobalPath.save + fileName);
			GetTree().ChangeScene("res://Scenes/MainScene.tscn");
		}

		private void _on_Button_Quit_pressed()
		{
			GD.Print("Press");
			GetTree().ChangeScene("res://Scenes/Start/StartScene.tscn");
		}

	}
}
