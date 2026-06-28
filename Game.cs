using Godot;
using GodotPlugins.Game;
using System;

public partial class Game : Node3D
{
	private Level currentLevel;
	private Hud hud;
	private MainMenu mainMenu;
	private Player player;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hud = GetNode<Hud>("Hud");
		mainMenu = GetNode<MainMenu>("MainMenu");
		mainMenu.LoadLevel += LoadLevel;
		player = ResourceLoader.Load<PackedScene>("res://Player/player.tscn").Instantiate<Player>();
		player.TacosCollect += hud.UpdateTacos;
		player.DeathSignal += hud.LoseLife;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void LoadLevel(String lvlName)
	{
		//TODO
		
		GD.Print("LevelLoad :" + lvlName + ".tscn");
		currentLevel = ResourceLoader.Load<PackedScene>("res://Levels/" + lvlName + ".tscn").Instantiate<Level>();
		hud.SetTime(currentLevel.Time);
		LaunchLevel();
	}

	private void LaunchLevel()
	{
		RemoveChild(mainMenu);
		AddChild(currentLevel);
		player.Position = currentLevel.GetSpawnPoint();
		RemoveChild(hud);
		AddChild(hud);
		AddChild(player);
		hud.ResetTacos();
		hud.LaunchTimer();
		hud.Visible = true;
	}
}
