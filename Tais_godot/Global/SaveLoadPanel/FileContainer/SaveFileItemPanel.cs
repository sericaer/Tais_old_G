using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
	class SaveFileItemPanel : PanelContainer
	{
		internal bool enableLoad
		{
			set
			{
				buttonLoad.Visible = value;
			}
		}

		internal Button buttonLoad
		{
			get
			{
				return GetNode<Button>("HBoxContainer/ButtonLoad");
			}
		}

		internal Button buttonDelete
		{
			get
			{
				return GetNode<Button>("HBoxContainer/ButtonDelete");
			}
		}

		public override void _Ready()
		{
			GetNode<Label>("HBoxContainer/Label").Text = Name;
		}
	}
}
