using Godot;
using Godot.Collections;
using System;

public partial class MainMenu : CanvasLayer
{

	[Signal]
	public delegate void LoadLevelEventHandler(String lvlName);
	private Array<String> levelList = [
		"level1",
		"level2",
		"level3",
		"level4",
		"level5",
		"level6"
	];

	private GridContainer levelSelectGrid;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		levelSelectGrid = GetNode<GridContainer>("LevelSelect");
		GetNode<Button>("BaseMenu/Play").Pressed += () => {EmitSignal(SignalName.LoadLevel, "level1");};
		GetNode<Button>("BaseMenu/LevelSelect").Pressed += () =>
		{
			GetNode<VBoxContainer>("BaseMenu").Visible = false;
			levelSelectGrid.Visible = true;
		};
		GetNode<Button>("BaseMenu/Exit").Pressed += () => {GetTree().Quit();};
		
		CreateLevelSelectMenu();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void CreateLevelSelectMenu()
	{
		for(int i = 0; i < levelList.Count; i++)
		{
			String lvlName = levelList[i];
			CompressedTexture2D img = ResourceLoader.Load<CompressedTexture2D>("res://Levels/Previews/" + lvlName + ".png");
			LevelButton levelButton = new LevelButton(levelList[i], img);
			levelButton.Pressed += () =>
			{
				EmitSignal(SignalName.LoadLevel, lvlName);
			};
			levelSelectGrid.AddChild(levelButton);
		}
	}

	
}
