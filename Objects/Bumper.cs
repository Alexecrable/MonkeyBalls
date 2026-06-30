using Godot;
using System;

public partial class Bumper : StaticBody3D
{
	MeshInstance3D mesh;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mesh = GetNode<MeshInstance3D>("MeshInstance3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void YEET(RigidBody3D body)
	{
		
		Vector3 offset = body.GlobalPosition - GlobalPosition;
		body.ApplyImpulse(offset.Normalized() * 10 ,GlobalPosition);
		Tween tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Spring);
		tween.TweenProperty(mesh, "scale", new Vector3(1.5f,1.5f,1.5f),0.1);
		tween.TweenProperty(mesh, "scale", new Vector3(1,1,1),0.1);
	}
}
