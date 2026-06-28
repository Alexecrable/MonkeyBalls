using Godot;
using System;

public partial class Level : Node3D
{
	[Export]
	public int Time {get; set;}
	private Vector3 spawnPoint;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node3D spawnNode = GetNode<Node3D>("SpawnPoint");
		spawnPoint = spawnNode.Position;
		spawnNode.QueueFree();

		GetNode<Area3D>("DeathFloor").BodyEntered += DeathFloorEntered;

	}

	private void DeathFloorEntered(Node3D body)
	{
		if(body.Name == "PlayerBody")
		{
			body.GetParent<Player>().Death();

		}
	}
	public Vector3 GetSpawnPoint()
	{
		GD.Print(spawnPoint);
		return spawnPoint;
	}
}
