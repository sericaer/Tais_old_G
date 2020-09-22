using Godot;
using RunData;
using System;

namespace TaisGodot.Scripts
{
	class Task : PanelContainer
	{
		internal void Refresh(Process process)
		{
			GetNode<Label>("VBoxContainer/Label").Text = process.name;
			GetNode<ProgressBar>("VBoxContainer/ProgressBar").Value = process.percent.Value;

		}
	}
}
