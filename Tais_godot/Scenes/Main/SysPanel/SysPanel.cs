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
		public override void _EnterTree()
		{
			SpeedContrl.Pause();
		}

		public override void _ExitTree()
		{
			SpeedContrl.UnPause();
		}

		private void _on_Button_Quit_pressed()
		{
			GetTree().ChangeScene("res://Scenes/Start/StartScene.tscn");
		}

		private void _on_Button_Save_pressed()
		{
			var savePanel = ResourceLoader.Load<PackedScene>("res://Scenes/Main/SysPanel/SavePanel/SavePanel.tscn").Instance();
			AddChild(savePanel);
		}
		
		private void _on_Button_Cancel_pressed()
		{
			QueueFree();
		}

	}
}
