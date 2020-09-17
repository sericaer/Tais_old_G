using Godot;
using System;
using System.Linq;

namespace TaisGodot.Scripts
{
	internal class SelectCollectTaxLevel : SpecialEventDialog
	{
		Label labelDesc;
		Label labelEffect;

		ButtonGroup toggles;
		Button btnConfirm;


		public override bool IsVaild()
		{
			return RunData.Date.inst == (null, 8, 10);
		}

		public override void _Ready()
		{

			labelDesc.Text = TranslateServerEx.Translate("EVENT_SELECT_COLLECT_TAX_LEVEL_DESC", RunData.Chaoting.inst.expectYearTax.Value.ToString());

			AddToggleButtons(RunData.PopTax.maxTaxLevel);

			btnConfirm.Disabled = true;
		}

        private void AddToggleButtons(int maxTaxLevel)
        {
			var button = GetNode<Button>("");

			for (int i = 2; i <= maxTaxLevel; i++)
			{
				var newBtn = button.Duplicate() as Button;
				newBtn.Name = i.ToString();
				newBtn.Text = $"EVENT_SELECT_COLLECT_TAX_LEVEL_OPTION_{i}_DESC";
			}
		}

        private void _on_ButtonToggle_Pressed()
        {
			int level = int.Parse(toggles.GetPressedButton().Name);
			labelEffect.Text = TranslateServerEx.Translate("EVENT_SELECT_COLLECT_TAX_LEVEL_EFFECT_DESC",
									RunData.PopTax.CalcTax(level).ToString());
			btnConfirm.Disabled = false;
		}

		private void _on_ButtonConfrim_Pressed()
		{
			int level = int.Parse(toggles.GetPressedButton().Name);
			RunData.Economy.inst.curr.Value += RunData.PopTax.CalcTax(level);

			QueueFree();
		}
	}
}
