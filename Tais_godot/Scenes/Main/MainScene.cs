using Godot;
using RunData;
using Modder;

namespace TaisGodot.Scripts
{
	public class MainScene : Panel
	{
		public MainScene()
		{
			Mod.showDialogAction = ShowDialog;
		}
		
		private void ShowDialog(Modder.GEvent dialog)
		{
			var dialogNode = (DialogPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DialogPanel/DialogPanel.tscn").Instance();
			dialogNode.gEventObj = dialog;

			AddChild(dialogNode);
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

		private void _on_Button_Cmd_button_up()
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
