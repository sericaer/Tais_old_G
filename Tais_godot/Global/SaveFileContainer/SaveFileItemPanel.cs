using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RunData;

namespace TaisGodot.Scripts
{
    class SaveFileItemPanel : HBoxContainer
    {
        internal string filePath;

        public override void _Ready()
        {
            GetNode<Label>("Label").Text = System.IO.Path.GetFileNameWithoutExtension(filePath);
        }

        private void _on_Button_Delete_pressed()
        {
            System.IO.File.Delete(filePath);
            QueueFree();
        }
    }
}
