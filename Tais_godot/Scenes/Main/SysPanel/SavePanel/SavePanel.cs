using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
    class SavePanel : Panel
    {
        private void _on_ButtonSave_button_up(string name)
        {
            var filePath = GlobalPath.save + GetNode<TextEdit>("").Text + ".save";
            if(System.IO.File.Exists(filePath))
            {
                var MsgboxPanel = (MsgboxPanel)ResourceLoader.Load<PackedScene>("res://Global/MsgboxPanel/MsgboxPanel.tscn").Instance();
                MsgboxPanel.desc = "STATIC_SAVE_FILE_EXIT";
                MsgboxPanel.action = () =>
                {
                    SaveFile(filePath);
                    QueueFree();
                };

                AddChild(MsgboxPanel);
                return;
            }

            SaveFile(filePath);
            QueueFree();
        }

        private void _on_ButtonCancel_button_up(string name)
        {
            QueueFree();
        }

        private void SaveFile(string path)
        {
            var content = Root.Serialize();
            System.IO.File.WriteAllText(path, content);
        }
    }
}
