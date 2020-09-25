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

			Root.def = new Define()
			{
				departs = new Dictionary<string, Define.DepartDef>()
				{
					{ "JIXIAN", new Define.DepartDef() { color = (63, 72, 204),
						pop_init = new (string name, int num)[] { ("haoqiang", 3000), ("minhu", 60000), ("yinhu", 20000) } } }
				},

				pops = new Dictionary<string, Define.PopDef>()
				{
					{ "haoqiang", new Define.PopDef() { is_collect_tax = true } },
					{ "minhu", new Define.PopDef() { is_collect_tax = true, consume = 100} },
					{ "yinhu", new Define.PopDef() { is_collect_tax = false } },
				},

				partys = new Dictionary<string, Define.PartyDef>()
				{
					{ "shizu", new Define.PartyDef() { name = "shizu"} },
					{ "huanguan", new Define.PartyDef() { name = "huanguan"} }
				},

				economy = new Define.EconomyDef()
				{
					curr = 456,
					pop_tax_percent = 30,
					expend_depart_admin = 100,
				},

				chaoting = new Define.ChaotingDef()
				{
					reportPopPercent = 130,
					taxPercent = 20,
					powerParty = "huanguan"
				},

				crop = new Define.CropDef()
				{
					growSpeed = 0.4,
					growStartDay = (null, 2, 1),
					harvestDay = (null, 9, 1),
				},

				pop_tax = new List<Define.TaxEffect>()
				{
					new Define.TaxEffect(){name = "level1", per_tax = 0.001, consume_effect = -10},
					new Define.TaxEffect(){name = "level2", per_tax = 0.002, consume_effect = -20},
					new Define.TaxEffect(){name = "level3", per_tax = 0.003, consume_effect = -30 },
					new Define.TaxEffect(){name = "level4", per_tax = 0.0035, consume_effect = -40 },
					new Define.TaxEffect(){name = "level5", per_tax = 0.004, consume_effect = -50 }
				}
			};

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
