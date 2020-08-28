using Godot;
using System;
namespace TaisGodot.Scripts
{
    public class Economy : PanelContainer
    {
        public override void _Ready()
        {
            GetNode<ReactiveLabel>("").Assoc(RunData.Economy.inst.curr);
        }
    }
}
