using Godot;
using System;
using TaisGodot.Scripts;

public class TaishouDetail : Panel
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	Label Name;
	Label Party;
	ReactiveLabel Age;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Name = GetNode<Label>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Name/Value");
		Party = GetNode<Label>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Party/Value");
		Age = GetNode<ReactiveLabel>("CenterContainer/PanelContainer/HBoxContainer/RightContainer/VBoxContainer/HBoxContainer/VBoxContainer/Age/Value");

		Name.Text = RunData.Taishou.inst.name;
		Party.Text = RunData.Taishou.inst.party.name;
		Age.Assoc(RunData.Taishou.inst.age);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
