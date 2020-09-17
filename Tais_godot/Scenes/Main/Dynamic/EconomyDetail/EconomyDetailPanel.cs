using Godot;
using System;
using RunData;

namespace TaisGodot.Scripts
{
	public class EconomyDetailPanel : Panel
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		RunData.Economy.Memento memento;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			SpeedContrl.Pause();

			memento = RunData.Economy.inst.CreateMemento();

			foreach (var income in RunData.Economy.inst.EnumerateInCome())
			{
				var incomPanel = (IncomePanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/EconomyDetail/IncomePanel.tscn").Instance();
				incomPanel.gmObj = income;

				GetNode<VBoxContainer>("CenterContainer/EconomyDetail/VBoxContainer/HBoxContainer/Income/VBoxContainer/VBoxContainer").AddChild(incomPanel);
			}

			foreach (var output in RunData.Economy.inst.EnumerateOutput())
			{
				var outputPanel = (OutputPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/EconomyDetail/OutputPanel.tscn").Instance();
				outputPanel.gmObj = output;

				GetNode<VBoxContainer>("CenterContainer/EconomyDetail/VBoxContainer/HBoxContainer/Output/VBoxContainer/VBoxContainer").AddChild(outputPanel);
			}

			GetNode<ReactiveLabel>("CenterContainer/EconomyDetail/VBoxContainer/Bottom/Surplus/Value").Assoc(RunData.Economy.inst.monthSurplus);

			//UpDateTempOutputStatus();
		}

		public override void _ExitTree()
		{
			memento = null;
			SpeedContrl.UnPause();
		}

		private void _on_Button_Confirm_pressed()
		{
			QueueFree();
		}

		private void _on_Button_Cancel_pressed()
		{
			RunData.Economy.inst.LoadMemento(memento);
			QueueFree();
		}

		private void _onButtonFullFillCountryTax()
		{
			//var need = RunData.Chaoting.inst.expectYearTax.Value- RunData.Chaoting.inst.realYearTax.Value;

			//RunData.Chaoting.inst.expectYearTax.Value = RunData.Chaoting.inst.realYearTax.Value;
			//RunData.Economy.inst.curr.Value -= need;
		}

		private void UpDateTempOutputStatus()
		{
			UpdateFullFillCountryTax();
		}

		private void UpdateFullFillCountryTax()
		{
			var btn = GetNode<Button>("ButtonFullFillCountryTax");

			var lackCountryTax = RunData.Chaoting.inst.expectYearTax.Value - RunData.Chaoting.inst.realYearTax.Value;
			if (lackCountryTax >= 0)
			{
				btn.Disabled = false;
				btn.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_NOT_LACK");
			}
			else if (RunData.Economy.inst.curr.Value < lackCountryTax)
			{
				btn.Disabled = false;
				btn.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_LACK_AND_ECONOMY_NOT_SUFFICENT",
					lackCountryTax.ToString(),
					RunData.Economy.inst.curr.Value.ToString());
			}
			else
			{
				btn.Disabled = true;
				btn.HintTooltip = TranslateServerEx.Translate("STATIC_COUNTRY_TAX_LACK_AND_ECONOMY_SUFFICENT",
					lackCountryTax.ToString(),
					RunData.Economy.inst.curr.Value.ToString());
			}
		}

		//  // Called every frame. 'delta' is the elapsed time since the previous frame.
		//  public override void _Process(float delta)
		//  {
		//      
		//  }
	}
}

