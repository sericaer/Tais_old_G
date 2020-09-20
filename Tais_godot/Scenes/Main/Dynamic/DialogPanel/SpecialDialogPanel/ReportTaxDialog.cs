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
			return RunData.Date.inst == (null, 10, 1);
		}

		public override void _Ready()
		{
			labelExpect = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/ReportExpect/Value");
			labelReal = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/ReportReal/Value");

			slider = GetNode<LimitSlider>("CenterContainer/PanelContainer/VBoxContainer/LimitSlider");

			slider.MinValue = 0;
			slider.MaxValue = (float)RunData.Economy.inst.curr.Value;

			if (RunData.Chaoting.inst.expectYearTax.Value < RunData.Economy.inst.curr.Value)
			{
				slider.LimitMinValue = (float)RunData.Chaoting.inst.expectYearTax.Value;
			}
			else
			{
				slider.LimitMinValue = slider.MaxValue;
			}

			slider.Value = slider.LimitMinValue;

			labelExpect.Text = RunData.Chaoting.inst.expectYearTax.Value.ToString();
			labelReal.Text = slider.Value.ToString();

		}

		private void _on_SliderReportCurr_ValueChanged(float value)
		{
			labelReal.Text = value.ToString();
		}

		private void _on_ButtonConfrim_Pressed()
		{
			RunData.Chaoting.inst.realYearTax.Value = slider.Value;
			RunData.Economy.inst.curr.Value -= slider.Value;
			QueueFree();
		}
	}
}
