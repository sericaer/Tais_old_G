using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using RunData;

namespace TaisGodot.Scripts
{
	class DepartPanel : Panel
	{
		public Depart gmObj;


		public override void _Ready()
		{

			GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/Name").Text = gmObj.name;

			GetNode<ReactiveLabel>("CenterContainer/PanelContainer/VBoxContainer/StatisticContainer/GridContainer/PopNum/Value").Assoc(gmObj.popNum);

			GetNode<PopContainer>("CenterContainer/PanelContainer/VBoxContainer/PopContainer").SetPops(gmObj.pops);
		}

		public override void _EnterTree()
		{
			GD.Print("DepartPanel _EnterTree");
		}

		public override void _ExitTree()
		{
			GD.Print("DepartPanel _ExitTree");
		}


		private void _on_Button_button_up()
		{
			QueueFree();
		}

	}
}


