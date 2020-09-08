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
        [Signal]
        public delegate void FileItemSelected(string name);

        public override void _Ready()
        {
            foreach (var filePath in System.IO.Directory.EnumerateFiles(GlobalPath.save, "*.save"))
            {
                var saveItemPanel = (SaveFileItemPanel)ResourceLoader.Load<PackedScene>("res://Global/Main/SysPanel/SavePanel/SavePanel.tscn").Instance();
                saveItemPanel.fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
                
                AddChild(saveItemPanel);

                saveItemPanel.GetNode<Button>("SelectFile").Connect("press", this, nameof(TriggleFileItemSelectedSignal), new Godot.Collections.Array() { filePath });
            }
        }

        private void TriggleFileItemSelectedSignal(string fileName)
        {
            EmitSignal(nameof(FileItemSelected), fileName);
        }
    }
}
