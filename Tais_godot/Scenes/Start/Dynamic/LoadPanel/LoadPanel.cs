using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaisGodot.Scripts
{
    class LoadPanel : Panel
    {
        [Signal]
        public delegate void LoadSaveFile(string fileName);

        private void _on_FileItemSelected(string fileName)
        {
            GetNode<Label>("Selected").Text = fileName;
            GetNode<Button>("Confirm").Disabled = false;
        }
        
        private void _on_ButtonConfirm_button_Press()
        {
            EmitSignal(nameof(LoadSaveFile), GetNode<Label>("Selected").Text);
            QueueFree();
        }

        private void _on_ButtonCancel_button_Press()
        {
            QueueFree();
        }
    }
}
