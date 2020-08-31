using System;
using Godot;
using RunData;

namespace TaisGodot.Scripts
{
	public class OutputPanel : HBoxContainer
	{
		internal Output gmObj;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GetNode<Label>("Label").Text = gmObj.name;
			GetNode<HSlider>("HSlider").Value = gmObj.percent.value;
			GetNode<ReactiveLabel>("Value").Assoc(gmObj.currValue);
		}

		private void _on_HSlider_value_changed(float value)
		{
			gmObj.percent.value = value;
		}
	}
}
