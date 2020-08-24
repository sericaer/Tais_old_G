using Godot;
using System;

namespace TaisGodot.Scripts
{
	public class Date : HBoxContainer
	{
		public override void _Ready()
		{
			GetNode<ReactiveLabel>("Value").Assoc(RunData.Date.inst.desc, 
												 (desc)=> TranslateServerEx.Translate("STATIC_DATE_VALUE", desc.Split('-')));
		}
	}

}
