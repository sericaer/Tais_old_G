using Godot;
using RunData;
using Modder;

namespace TaisGodot.Scripts
{
	public class MainScene : Panel
	{
		private async void _on_DaysInc()
		{
			RunData.Root.DaysInc();

			foreach (var eventobj in Modder.Mod.EventProcess(RunData.Date.Value))
			{
				var dialog = ShowDialog(eventobj);

				await ToSignal(dialog, "tree_exited");
			}

			if(Root.inst.isEnd)
			{
				Root.Exit();
				GetTree().ChangeScene("res://Scenes/End/EndScene.tscn");
			}

			//GetNode<WarnContainer>("").Refresh(Modder.Mod.WarnProcess());
		}

		private Node ShowDialog(Modder.GEvent eventobj)
		{
			GD.Print(eventobj.title.Format);
			GD.Print(eventobj.desc.Format);
			foreach (var op in eventobj.options)
			{
				GD.Print(op.desc.Format);
			}

			var dialogNode = (DialogPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DialogPanel/DialogPanel.tscn").Instance();
			dialogNode.gEventObj = eventobj;

			AddChild(dialogNode);

			return dialogNode;
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

		private void _on_Button_Sys_pressed()
		{
			var SysPanel = ResourceLoader.Load<PackedScene>("res://Scenes/Main/SysPanel/SysPanel.tscn").Instance();
			AddChild(SysPanel);
		}

		private void _on_Button_Economy_pressed()
		{
			var SysPanel = ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/EconomyDetail/EconomyDetailPanel.tscn").Instance();
			AddChild(SysPanel);
		}
		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
