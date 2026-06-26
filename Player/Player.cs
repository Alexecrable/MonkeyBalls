using Godot;
using System;

public partial class Player : Node3D
{
	[Export]
	public float bodySpeed, camSpeed, camDistance, camFOV;

	private RigidBody3D body;
	private Node3D cameraAnchor;
	private Camera3D camera;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		body = GetNode<RigidBody3D>("PlayerBody");
		cameraAnchor = GetNode<Node3D>("CameraAnchor");
		camera = cameraAnchor.GetNode<Camera3D>("Camera3D");
		camera.Fov = camFOV;
		camera.Position = new Vector3(0, 0.5f, camDistance);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		cameraAnchor.Position = body.Position;

		float realBodySpeed = bodySpeed * (float)delta;
		float realCamSpeed = camSpeed * (float)delta;

		Vector2 movementDirection = Input.GetVector("left", "right" ,"forward", "backward");
		Vector2 cameraMoveDir = Input.GetVector("camLeft", "camRight" ,"camForward", "camBackward");
		
		Vector3 ballPushVector = new Vector3(movementDirection.X,0,movementDirection.Y).Rotated(Vector3.Up, cameraAnchor.Rotation.Y) * realBodySpeed;
		Vector3 camRot = new Vector3(-cameraMoveDir.Y , -cameraMoveDir.X, 0) * realCamSpeed;
		
		cameraAnchor.Rotation += camRot;
        body.ApplyCentralForce(ballPushVector);
		//body.ApplyForce(ballPushVector, new Vector3(0, 0.1f, 0));
		
	}
}
