using Godot;
using System;

public partial class Goal : StaticBody3D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area3D area = GetNode<Area3D>("Area3D");
		area.BodyEntered += bodyente;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private void bodyente(Node3D body)
	{
		if(body.Name == "PlayerBody")
		{
			GD.Print("fini");
			body.GetParent<Player>().EndLevel();
		}
	}
}
