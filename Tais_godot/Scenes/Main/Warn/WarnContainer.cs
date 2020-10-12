using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Godot;
using Modder;
using RunData;

namespace TaisGodot.Scripts
{
	class WarnContainer : HBoxContainer
	{
		public override void _Ready()
		{

		}

        internal void Refresh(IEnumerable<(string title, List<Desc> descs)> warns)
        {
			var warnItems = this.GetChildren<WarnItem>().ToList();

			var needRemoves = warnItems.FindAll(x => !warns.Any(y=>y.title == x.Name));
			needRemoves.ForEach(x =>
			{
				warnItems.Remove(x);
				x.QueueFree();
			});

			foreach(var warn in warns)
            {
				var warnItem = warnItems.SingleOrDefault(x => x.Name == warn.title);
				if (warnItem == null)
                {
					warnItem = (WarnItem)ResourceLoader.Load<PackedScene>("res://Scenes/Main/Warn/WarnItem.tscn").Instance();
					warnItem.Name = warn.title;

					AddChild(warnItem);
				}

				warnItem.Refresh(warn.descs);
			}
			
        }
	}
}



