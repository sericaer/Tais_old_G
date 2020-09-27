using Godot;
using RunData;
using Modder;

namespace TaisGodot.Scripts
{
	public class MainScene : Panel
	{

		internal static MainScene inst;

		public MainScene()
        {
			inst = this;
        }

		private async void _on_DaysInc()
		{
			RunData.Root.DaysInc();

			foreach (var spevent in SpecialEventDialog.Process())
			{
				var dialog = ShowSpecialDialog(spevent);

				await ToSignal(dialog, "tree_exited");
			}

			foreach (var eventobj in Modder.Mod.EventProcess(RunData.Date.Value))
			{
				var dialog = ShowDialog(eventobj);

				await ToSignal(dialog, "tree_exited");
			}

			GetNode<WarnContainer>("VBoxContainer/WinContainer/ImpContainer/WarnContainer").Refresh(RunData.Root.GenerateWarns());
			GetNode<TaskContainer>("VBoxContainer/WinContainer/TaskContainer").Refresh(RunData.Root.GetTask());

			if (Root.inst.isEnd)
			{
				Root.Exit();
				GetTree().ChangeScene("res://Scenes/End/EndScene.tscn");
			}
		}

		internal static Node ShowDialog(Modder.GEvent eventobj)
		{
			GD.Print(eventobj.title.Format);
			GD.Print(eventobj.desc.Format);
			foreach (var op in eventobj.options)
			{
				GD.Print(op.desc.Format);
			}

			var dialogNode = (DialogPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DialogPanel/DialogPanel.tscn").Instance();
			dialogNode.gEventObj = eventobj;

			inst.AddChild(dialogNode);

			return dialogNode;
		}

		internal static Node ShowSpecialDialog(SpecialEventDialog spEvent)
		{
			var dialogNode = (SpecialEventDialog)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DialogPanel/SpecialDialogPanel/" + spEvent.name + ".tscn").Instance();

			inst.AddChild(dialogNode);
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
		
		private void _on_ButtonChaoting_pressed()
		{
			var ChaotingDetail = ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/ChaotingDetail/ChaotingDetail.tscn").Instance();
			AddChild(ChaotingDetail);
		}
		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}
