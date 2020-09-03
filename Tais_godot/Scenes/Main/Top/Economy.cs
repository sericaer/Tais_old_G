using Godot;
using System;
namespace TaisGodot.Scripts
{
	public class Economy : Button
	{
		public override void _Ready()
		{
			GetNode<ReactiveLabel>("HBoxContainer/Value").Assoc(RunData.Economy.inst.curr);
		}
	}
}
