using Godot;
using System;

public partial class Tacos : Node3D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area3D area = GetNode<Area3D>("Area3D");
		area.BodyEntered += bodyente;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Rotate(Vector3.Up, (float)delta);
	}

	private void bodyente(Node3D body)
	{
		if(body.Name == "PlayerBody")
		{
			QueueFree();
		}
	}
}
