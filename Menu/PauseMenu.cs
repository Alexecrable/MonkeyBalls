using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	public Button ResumeButton {get; set;} 
	public Button ExitButton {get; set;}
	public override void _Ready()
	{
		ResumeButton = GetNode<Button>("VBoxContainer/Resume");
		ExitButton = GetNode<Button>("VBoxContainer/Exit");
		//ResumeButton.FocusNext = ExitButton.GetPath();
		
	}

	
}
