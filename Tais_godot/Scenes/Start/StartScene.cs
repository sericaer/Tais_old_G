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

		static StartScene()
		{
			System.IO.Directory.CreateDirectory(GlobalPath.save);

			Root.logger = (value) => GD.Print(value);
			Mod.logger = (value) => GD.Print(value);

			ModDataVisit.InitVisitMap(typeof(Root));

			Mod.Load(GlobalPath.mod);

			Root.def = Mod.def;

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

			var initData = new InitData()
			{
				common = new InitData.Common()
				{
					name = "TEST1",
					age = 34,
					party = "shizu"
				}
			};

			Root.Init(initData);

			ModDataVisit.InitVisitData(Root.inst);

			GetTree().ChangeScene("res://Scenes/Main/MainScene.tscn");
		}

		private void _on_Button_Load_pressed()
		{
			var loadPanel = (SaveLoadPanel)ResourceLoader.Load<PackedScene>("res://Global/SaveLoadPanel/SaveLoadPanel.tscn").Instance();
			loadPanel.enableLoad = true;

			AddChild(loadPanel);

			loadPanel.Connect("LoadSaveFile", this, nameof(_on_LoadSaveFile_Signed));
		}

		private void _on_Button_Quit_pressed()
		{
			GetTree().Quit();
		}

		private void _on_LoadSaveFile_Signed(string fileName)
		{
			var content = System.IO.File.ReadAllText(GlobalPath.save + fileName + ".save");
			Root.Deserialize(content);

			ModDataVisit.InitVisitData(Root.inst);
			GetTree().ChangeScene("res://Scenes/Main/MainScene.tscn");
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
