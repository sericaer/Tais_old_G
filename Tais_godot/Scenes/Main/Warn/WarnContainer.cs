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

        internal void Refresh(IEnumerable<(string key, List<string> datas)> freshItems)
        {
			var warnItems = this.GetChildren<WarnItem>().ToList();

			var needRemoves = warnItems.FindAll(x => !freshItems.Any(y=>y.key == x.Name));
			needRemoves.ForEach(x =>
			{
				warnItems.Remove(x);
				x.QueueFree();
			});

			foreach(var freshItem in freshItems)
            {
				var warnItem = warnItems.SingleOrDefault(x => x.Name == freshItem.key);
				if (warnItem == null)
                {
					warnItem = (WarnItem)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Warn/WarnItem.tscn").Instance();
					warnItem.Name = freshItem.key;

					AddChild(warnItem);
				}

				warnItem.Refresh(freshItem.datas);
			}
			
        }
	}
}



