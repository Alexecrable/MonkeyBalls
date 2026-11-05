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
		"level3"
	];

	private GridContainer levelSelectGrid;
	private VBoxContainer baseMenu;
	private Button playButton, playStyleButton;
	private PackedScene levelButtonScene;
	private bool isMonkeyBall;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		levelButtonScene = ResourceLoader.Load<PackedScene>("res://Menu/level_button.tscn");
		isMonkeyBall = false;
		levelSelectGrid = GetNode<GridContainer>("LevelSelect");
		playButton = GetNode<Button>("BaseMenu/Play");
		playButton.GrabFocus();
		GetNode<Button>("BaseMenu/Play").Pressed += () => {EmitSignal(SignalName.LoadLevel, "level2");};
		baseMenu = GetNode<VBoxContainer>("BaseMenu");
		baseMenu.GetNode<Button>("LevelSelect").Pressed += () =>
		{
			baseMenu.Visible = false;
			levelSelectGrid.Visible = true;

			levelSelectGrid.GetChild<LevelButton>(0).GrabFocus();
		};
		playStyleButton = baseMenu.GetNode<Button>("PlayStyle");
		playStyleButton.Pressed += () =>
		{
			isMonkeyBall = !isMonkeyBall;
			String playStyleString = (isMonkeyBall) ? "MonkeyBall" : "Marble It Up";
			playStyleButton.Text = "PlayStyle : " + playStyleString;
		};
		GetNode<Button>("BaseMenu/Exit").Pressed += () => {GetTree().Quit();};
		
		CreateLevelSelectMenu();
	}

	public bool IsMonkeyBall()
	{
		return isMonkeyBall;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void BackToMainMenu()
	{
		baseMenu.Visible = true;
		levelSelectGrid.Visible = false;
		playButton.GrabFocus();
	}
	private void CreateLevelSelectMenu()
	{
		
		for(int i = 0; i < levelList.Count; i++)
		{
			String lvlName = levelList[i];
			CompressedTexture2D img = ResourceLoader.Load<CompressedTexture2D>("res://Levels/Previews/" + lvlName + ".png");
			LevelButton levelButton = levelButtonScene.Instantiate<LevelButton>();
			levelButton.InitButton(lvlName, img);
			
			
			levelButton.Pressed += () =>
			{
				EmitSignal(SignalName.LoadLevel, lvlName);
			};
			levelSelectGrid.AddChild(levelButton);
			
		}
	}

	
}
