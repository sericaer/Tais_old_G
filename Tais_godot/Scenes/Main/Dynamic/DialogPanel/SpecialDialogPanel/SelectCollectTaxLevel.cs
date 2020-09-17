using Godot;
using System;
using System.Linq;

namespace TaisGodot.Scripts
{
	internal class SelectCollectTaxLevel : SpecialEventDialog
	{
		RichTextLabel labelDesc;
		RichTextLabel labelEffect;

		Button btnLevel1;
		Button btnConfirm;


		public override bool IsVaild()
		{
			return RunData.Date.inst == (null, 1, 10);
		}

		public override void _Ready()
		{
			labelDesc = GetNode<RichTextLabel>("CenterContainer/PanelContainer/VBoxContainer/Desc");
			labelEffect = GetNode<RichTextLabel>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/Effect");

			btnConfirm = GetNode<Button>("CenterContainer/PanelContainer/VBoxContainer/Button");

			btnLevel1 = GetNode<Button>("CenterContainer/PanelContainer/VBoxContainer/PanelContainer/VBoxContainer/HBoxContainer/1");

			labelDesc.Text = TranslateServerEx.Translate("EVENT_SELECT_COLLECT_TAX_LEVEL_DESC", RunData.Chaoting.inst.expectYearTax.Value.ToString());

			AddToggleButtons(RunData.PopTax.maxTaxLevel);

			btnConfirm.Disabled = true;
		}

		private void AddToggleButtons(int maxTaxLevel)
		{
			for (int i = 2; i <= maxTaxLevel; i++)
			{
				var newBtn = btnLevel1.Duplicate() as Button;
				newBtn.Name = i.ToString();
				newBtn.Text = $"EVENT_SELECT_COLLECT_TAX_LEVEL_OPTION_{i}_DESC";
				btnLevel1.GetParent().AddChild(newBtn);

			}
		}

		private void _on_ButtonToggle_Pressed()
		{
			int level = int.Parse(btnLevel1.Group.GetPressedButton().Name);
			labelEffect.Text = TranslateServerEx.Translate("EVENT_SELECT_COLLECT_TAX_LEVEL_EFFECT_DESC",
									RunData.PopTax.CalcTax(level).ToString());
			btnConfirm.Disabled = false;
		}

		private void _on_ButtonConfrim_Pressed()
		{
			int level = int.Parse(btnLevel1.Group.GetPressedButton().Name);
			RunData.Economy.inst.curr.Value += RunData.PopTax.CalcTax(level);

			QueueFree();
		}
	}
}
