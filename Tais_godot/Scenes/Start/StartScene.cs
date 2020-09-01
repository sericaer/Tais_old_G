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
			Root.logger = (value) => GD.Print(value);
			Mod.logger = (value) => GD.Print(value);

			ModDataVisit.InitVisitMap(typeof(Root));

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
			var def = new Define()
			{
				departs = new Dictionary<string, Define.DepartDef>()
				{
					{ "JIXIAN", new Define.DepartDef(){color = (63, 72, 204),
													   pop_init= new (string name, int num)[]{ ("haoqiang", 3000), ("minhu", 60000), ("yinhu", 20000) } } }
				},

				pops = new Dictionary<string, Define.PopDef>()
				{
					{ "haoqiang", new Define.PopDef(){ is_collect_tax = true} },
					{ "minhu", new Define.PopDef(){ is_collect_tax = true} },
					{ "yinhu", new Define.PopDef(){ is_collect_tax = false} },
				},

				economy = new Define.EconomyDef()
				{
					curr = 456,
				}

			};

			ModDataVisit.InitVisitData(Root.Init(def));
			//DataVisit.Init(Root.GetData());

			//Mod.InitVisit(Root.inst, Root.GetReflectionDict());

			GetTree().ChangeScene("res://Scenes/Main/MainScene.tscn");
			// Replace with function body.
		}

		private void _on_Button_Load_button_up()
		{
			GetNode<SavePanel>("LoadPanel").Visible = true;
		}

		private void _on_Button_Quit_pressed()
		{
			GetTree().Quit();
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
