using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
	class PopContainer : PanelContainer
	{
		internal void SetPops(IEnumerable<Pop> pops)
		{
			GD.Print("PopContainer"+ pops.Count());

			var Grid = GetNode<GridContainer>("GridContainer");
			foreach (var pop in pops)
			{
				var popNode = (PopPanel)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Dynamic/DepartPanel/PopPanel/PopPanel.tscn").Instance();
				popNode.gmObj = pop;

				Grid.AddChild(popNode);
			}
		}
	}
}
