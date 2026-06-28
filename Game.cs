using Godot;
using GodotPlugins.Game;
using System;

public partial class Game : Node3D
{
	private Level currentLevel;
	//private PackedScene loadedLevel;
	private Hud hud;
	private PauseMenu pauseMenu;
	private MainMenu mainMenu;
	private Player player;
	private bool inGame;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hud = GetNode<Hud>("Hud");
		hud.GameOver += () => {
			inGame = false;
			CallDeferred("remove_child", currentLevel);
			CallDeferred("remove_child", player);
			AddChild(mainMenu);
			mainMenu.BackToMainMenu();
		};
		
		mainMenu = GetNode<MainMenu>("MainMenu");
		mainMenu.LoadLevel += LoadLevel;
		player = ResourceLoader.Load<PackedScene>("res://Player/player.tscn").Instantiate<Player>();
		player.LevelEnd += () =>
		{
			inGame = false;
			CallDeferred("remove_child", currentLevel);
			CallDeferred("remove_child", player);
			AddChild(mainMenu);
			mainMenu.BackToMainMenu();
		};
		pauseMenu = GetNode<PauseMenu>("PauseMenu");

		pauseMenu.ResumeButton.Pressed += PauseMenu;
		pauseMenu.ExitButton.Pressed += ExitLevel;

		player.TacosCollect += hud.UpdateTacos;
		inGame = false;
		player.DeathSignal += hud.LoseLife;
		//player.DeathSignal += Respawn;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		bool pause = Input.IsActionPressed("pause");
		GD.Print("pause " + pause);
		if (pause)
		{
			if (inGame)
			{
				PauseMenu();
			}
			else
			{
				mainMenu.BackToMainMenu();
			}

		}
	}


	private void ExitLevel()
	{
		inGame = false;
		PauseMenu();
		RemoveChild(currentLevel);
		RemoveChild(player);
		AddChild(mainMenu);
		mainMenu.BackToMainMenu();
	}

	private void PauseMenu()
	{
		pauseMenu.Visible = !pauseMenu.Visible;
		pauseMenu.ResumeButton.GrabFocus();
		GetTree().Paused = !GetTree().Paused;
	}

	//private void Respawn()
	//{
	//	GD.Print("respawn " + player.Position);
	//	player.Position = currentLevel.GetSpawnPoint();
	//	GD.Print("respawn2 " + player.Position);
	//
	//}

	private void LoadLevel(String lvlName)
	{
		//TODO

		//loadedLevel = ResourceLoader.Load<PackedScene>("res://Levels/" + lvlName + ".tscn");
		currentLevel = ResourceLoader.Load<PackedScene>("res://Levels/" + lvlName + ".tscn").Instantiate<Level>();

		hud.SetTime(currentLevel.Time);
		LaunchLevel();
	}

	private void LaunchLevel()
	{
		RemoveChild(mainMenu);
		AddChild(currentLevel);
		player.Position = currentLevel.GetSpawnPointPos();
		player.Rotation = currentLevel.GetSpawnPointRot();

		AddChild(player);
		player.init();
		hud.ResetTacos();
		hud.LaunchTimer();
		hud.Visible = true;
		inGame = true;
	}
}
