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

		public DepartPanel()
		{
			GD.Print("DepartPanel construct");

			
			//GetNode<ReactiveLabel>("PopNum").Set();

			//
		}

		public override void _Ready()
		{
			GD.Print("DepartPanel Ready");

			GetNode<Label>("Object/VBoxContainer/Name").Text = gmObj.name;

			GetNode<PopContainer>("Object/VBoxContainer/PopContainer/Grid").SetPops(gmObj.pops);
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


