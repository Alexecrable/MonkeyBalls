using Godot;
using System;
using System.ComponentModel;

public partial class Tacos : Node3D
{
	private AudioStreamPlayer3D sound;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area3D area = GetNode<Area3D>("Area3D");
		area.BodyEntered += bodyente;
		sound = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
		sound.Finished += () => {sound.QueueFree();};
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
			body.GetParent<Player>().UpdateTacos(1, GlobalPosition);
			sound.Reparent(body);
			sound.Play();
			QueueFree();

		}
	}
}
