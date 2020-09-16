using Godot;
using System;
using System.Linq;

namespace TaisGodot.Scripts
{
	internal class ReportPopNumDialog : SpecialEventDialog
	{
		Label labelReal;
		Label labelReportLast;
		Label labelReportCurr;

		Slider sliderReportLast;
		Slider sliderReportCurr;

		public ReportPopNumDialog()
		{
		}

		public override bool IsVaild()
		{
			return RunData.Date.inst == (null, 8, 1);
		}

		public override void _Ready()
		{
			labelReal = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/RealPopNum/Value");
			labelReportLast = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/LastReport/Value");
			labelReportCurr = GetNode<Label>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/CurrLastReport/Value");

			sliderReportLast = GetNode<Slider>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/LastReport/HSlider");
			sliderReportCurr = GetNode<Slider>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/CurrLastReport/HSlider");

			labelReal.Text = RunData.Root.inst.departs.Sum(x => x.popNum.Value).ToString();
			labelReportLast.Text = RunData.Chaoting.inst.reportPopNum.Value.ToString();
			labelReportCurr.Text = labelReportLast.Text;

			sliderReportLast.MinValue = 0;
			sliderReportLast.MaxValue = 10;
			sliderReportLast.Value = 5;
			sliderReportLast.Rounded = true;

			sliderReportCurr.MinValue = 0;
			sliderReportCurr.MaxValue = 10;
			sliderReportCurr.Value = 5;
			sliderReportCurr.Rounded = true;

		}

		private void _on_SliderReportCurr_ValueChanged(double value)
		{
			var newReport = RunData.Chaoting.inst.reportPopNum.Value + RunData.Chaoting.inst.reportPopNum.Value * (sliderReportCurr.Value - 5) / 100;
			labelReportCurr.Text = ((int)newReport).ToString();
		}

		private void _on_ButtonConfrim_Pressed()
		{
			RunData.Chaoting.inst.reportPopNum.Value = int.Parse(labelReportCurr.Text);
			QueueFree();
		}
	}
}
