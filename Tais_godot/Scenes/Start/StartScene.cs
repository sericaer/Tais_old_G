using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

using Modder;
using RunData;

namespace TaisGodot.Scripts
{
	public class StartScene : Panel
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		public StartScene()
		{
            Root.logger = (value)=>GD.Print(value);

			Mod.Load(GlobalPath.mod);

			foreach (var mod in Mod.Enumerate())
			{
				TranslateServerEx.AddTranslate(mod.languages);
			}

		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			

		}

		private void _on_Button_Start_button_up()
		{
			Root.Init();
			GetTree().ChangeScene("res://Scenes/Main/MainScene.tscn");
			// Replace with function body.
		}

		private void _on_Button_Load_button_up()
		{
			GetNode<SavePanel>("LoadPanel").Visible = true;
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
