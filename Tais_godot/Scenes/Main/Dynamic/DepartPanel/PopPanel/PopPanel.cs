using Godot;
using RunData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaisGodot.Scripts
{
	class PopPanel : Panel
	{
		internal Pop gmObj;

		public override void _Ready()
		{
			GetNode<Label>("Type").Text = gmObj.name;

			GetNode<ReactiveLabel>("Num").Assoc(gmObj.num);
		}
	}
}
