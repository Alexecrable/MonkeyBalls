using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Level : Node3D
{
	[Export]
	public int Time { get; set; }
	private Node3D baseMonkeyBall, targetMonkeyBall, skyBox;
	private Vector3 spawnPointPos, spawnPointRot;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node3D spawnNode = GetNode<Node3D>("SpawnPoint");
		spawnPointPos = spawnNode.Position;
		spawnPointRot = spawnNode.Rotation;
		spawnNode.QueueFree();
		baseMonkeyBall = GetNode<Node3D>("BaseMonkey");
		targetMonkeyBall = GetNode<Node3D>("BaseMonkey/TargetMonkey");
		skyBox = GetNode<Node3D>("SkyBox");
		GetNode<Area3D>("DeathFloor").BodyEntered += DeathFloorEntered;

	}

	public Node3D GetSky()
	{
		return skyBox;
	}	
	

	private void DeathFloorEntered(Node3D body)
	{
		if (body.Name == "PlayerBody")
		{
			body.GetParent<Player>().Death();

		}
	}
	public Vector3 GetSpawnPointPos()
	{
		return spawnPointPos;
	}
	public Vector3 GetSpawnPointRot()
	{
		return spawnPointRot;
	}
}
