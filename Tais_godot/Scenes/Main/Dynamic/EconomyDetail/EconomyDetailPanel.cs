using Godot;
using System;

namespace TaisGodot.Scripts
{
	public class EconomyDetailPanel : Panel
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			SpeedContrl.Pause();
		}

		public override void _ExitTree()
		{
			SpeedContrl.UnPause();
		}

		private void _on_Button_Confirm_pressed()
		{
			QueueFree();
		}

		private void _on_Button_Cancel_pressed()
		{
			QueueFree();
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}

