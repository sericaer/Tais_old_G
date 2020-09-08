using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
    class SaveFileContainer : VBoxContainer
    {
        public override void _Ready()
        {
            foreach (var filePath in System.IO.Directory.EnumerateFiles(GlobalPath.save, "*.save"))
            {
                var saveItemPanel = (SaveFileItemPanel)ResourceLoader.Load<PackedScene>("res://Global/Main/SysPanel/SavePanel/SavePanel.tscn").Instance();
                saveItemPanel.filePath = filePath;
                AddChild(saveItemPanel);
            }
        }
    }
}
