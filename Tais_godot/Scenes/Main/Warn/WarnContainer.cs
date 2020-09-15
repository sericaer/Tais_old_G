using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using RunData;

namespace TaisGodot.Scripts
{
	class WarnContainer : HBoxContainer
	{
		public override void _Ready()
		{

		}

        internal void Refresh(IEnumerable<RunData.Warn> warns)
        {
			var warnItems = this.GetChildren<WarnItem>().ToList();

			var needRemoves = warnItems.FindAll(x => !warns.Any(y=>y.name == x.Name));
			needRemoves.ForEach(x =>
			{
				warnItems.Remove(x);
				x.QueueFree();
			});

			foreach(var warn in warns)
            {
				var warnItem = warnItems.SingleOrDefault(x => x.Name == warn.name);
				if (warnItem == null)
                {
					warnItem = (WarnItem)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Warn/WarnItem.tscn").Instance();
					warnItem.Name = warn.name;

					AddChild(warnItem);
				}

				warnItem.Refresh(warn);
			}
			
        }
	}
}



