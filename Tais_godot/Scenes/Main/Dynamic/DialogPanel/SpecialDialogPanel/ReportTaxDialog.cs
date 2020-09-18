using Godot;
using System;
using System.Linq;

namespace TaisGodot.Scripts
{
	internal class ReportTaxDialog : SpecialEventDialog
	{
		Label labelExpect;
		Label labelReal;

		LimitSlider slider;


		public ReportTaxDialog()
		{
		}

		public override bool IsVaild()
		{
			return RunData.Date.inst == (null, 9, 1);
		}

		public override void _Ready()
		{
			labelExpect = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/CurrLastReport/Value");

			slider = GetNode<LimitSlider>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/LastReport/HSlider");

			labelExpect.Text = RunData.Chaoting.inst.expectYearTax.Value.ToString();

			slider.MinValue = 0;
			slider.MaxValue = RunData.Economy.inst.curr.Value;

			if(RunData.Chaoting.inst.expectYearTax.Value < RunData.Economy.inst.curr.Value)
            {
				slider.LimitMinValue = RunData.Chaoting.inst.expectYearTax.Value;
			}
		}

		private void _on_SliderReportCurr_ValueChanged(double value)
		{
			labelReal.Text = slider.Value.ToString();
		}

		private void _on_ButtonConfrim_Pressed()
		{
			RunData.Chaoting.inst.realYearTax.Value = slider.Value;
			RunData.Economy.inst.curr.Value -= slider.Value;
			QueueFree();
		}
	}
}
