using Godot;
using System;
using System.Linq;

namespace TaisGodot.Scripts
{
	internal class ReportTaxDialog : SpecialEventDialog
	{
		Label labelExpect;
		Label labelReal;

		LimitSlider sliderReal;


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

			sliderReal = GetNode<LimitSlider>("CenterContainer/PanelContainer/VBoxContainer/LimitSlider");

			sliderReal.MinValue = 0;
			sliderReal.MaxValue = (float)RunData.Economy.inst.curr.Value;

			if (RunData.Chaoting.inst.expectYearTax.Value < RunData.Economy.inst.curr.Value)
			{
				sliderReal.LimitMinValue = (float)RunData.Chaoting.inst.expectYearTax.Value;
			}
			else
			{
				sliderReal.LimitMinValue = sliderReal.MaxValue;
			}

			sliderReal.Value = sliderReal.LimitMinValue;

			labelExpect.Text = RunData.Chaoting.inst.expectYearTax.Value.ToString();
			labelReal.Text = sliderReal.Value.ToString();

		}

		private void _on_SliderReportCurr_ValueChanged(float value)
		{
			labelReal.Text = value.ToString();
		}

		private void _on_ButtonConfrim_Pressed()
		{
			RunData.Chaoting.inst.realYearTax.Value = sliderReal.Value;
			RunData.Economy.inst.curr.Value -= sliderReal.Value;
			QueueFree();
		}
	}
}
