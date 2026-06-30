using Godot;
using System;

public partial class Attractor : Area3D
{
	private RigidBody3D body;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(body != null)
		{
			GD.Print("tenteative de meurtre");
			Vector3 offset = GlobalPosition - body.GlobalPosition;
			body.ApplyForce(offset * 500 * (float)delta);
		}
	}

	private void OnBodyEntered(Node node)
	{
		if(node.Name == "PlayerBody")
		{
			body = (RigidBody3D)node;
		}
	}
	private void OnBodyExited(Node node)
	{
		if(node.Name == "PlayerBody")
		{
			body = null;
		}
	}
}
