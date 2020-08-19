using Godot;
using RunData;
using System;

namespace TaisGodot.Scripts
{
	public class MainScene : Panel
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		private void _on_MapRect_MapClickSignal(int r, int g, int b)
		{
			GD.Print($"{r}, {g}, {b}");

			var depart = Depart.GetByColor(r, g, b);
			if (depart == null)
			{
				return;
			}

			GD.Print($"select depart:{depart.name}");

			var departNode = (DepartPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DepartPanel/DepartPanel.tscn").Instance();
			departNode.gmObj = depart;

			AddChild(departNode);
		}

		public void _on_Button_Cmd_button_up()
		{
			GetNode<Panel>("SysPanel").Visible = true;
		}

		private void OnEscSignal()
		{
			Root.Exit();
			GetTree().ChangeScene("res://Scenes/StartScene.tscn");
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
