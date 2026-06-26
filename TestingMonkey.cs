using Godot;
using System;

public partial class TestingMonkey : Node3D
{
	[Export]
	private float speed, camSpeed, jumpForce;
	private bool canJump, bufferJump;
	private Timer timer;

	private Node3D camera, refer, refere;
	private Node3D trueCAm;
	private RigidBody3D body;
	private StaticBody3D ground;
	private Vector3 posGround, camRotSav;
	private RayCast3D vect;
	private Quaternion quat;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector3 test = Vector3.Right;
		GD.Print(test);
		test = test.Rotated(Vector3.Up, 5);
		GD.Print(test);

		camera = GetNode<Node3D>("Node3D");
		refer = GetNode<Node3D>("ref");
		refere = GetNode<Node3D>("ref/refe");
		body = GetNode<RigidBody3D>("lol");
		ground = GetNode<StaticBody3D>("StaticBody3D2");
		posGround = ground.GlobalPosition;
		trueCAm = GetNode<Node3D>("Node3D2");
		vect = GetNode<RayCast3D>("ref/RayCast3D");
		camRotSav = trueCAm.Rotation;
		quat = new Quaternion();
		canJump = true;
		bufferJump = false;
		body.MaxContactsReported = 1;
		timer = new Timer();
		timer.WaitTime = 0.1;
		timer.Autostart = false;
		timer.Timeout += () =>
		{
			bufferJump = false;
		};
		AddChild(timer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		//ground.GlobalPosition = posGround;
		camera.Position = body.Position;
		trueCAm.Position = body.Position ;
		Vector2 direction = Input.GetVector("left", "right", "forward", "backward");
		Vector3 ballPushVector = new Vector3(direction.X * speed, 0, direction.Y * speed).Rotated(Vector3.Up, camera.Rotation.Y);

		Vector2 cameraMoveDir = Input.GetVector("camLeft", "camRight", "camForward", "camBackward");

		Vector3 camRot = new Vector3(-cameraMoveDir.Y, -cameraMoveDir.X, 0);
		//camera.Rotation += camRot;
		Vector3 GroundRot = new Vector3(direction.Y, 0, -direction.X).Rotated(Vector3.Up, trueCAm.Rotation.Y);

		//float finRotX = Mathf.Lerp(camera.Rotation.X, GroundRot.X, 0.1f * (float)delta);
		//float finRotY  = Mathf.Lerp(camera.Rotation.Z, GroundRot.Z, 0.1f * (float)delta);
		//GD.Print("-----------------");
		//GD.Print(camera.Rotation.X + " | " + camera.Rotation.Z);
		//GD.Print(GroundRot.X + " | " + GroundRot.Z);
		//GD.Print(finRotX + " | " + finRotY);

		//func rotate_around(obj:Spatial, point: Vector3, axis: Vector3, phi: float):
		//# https://godotengine.org/qa/45609/how-do-you-rotate-spatial-node-around-axis-given-point-space?show=45970#a45970
		//obj.global_translate(-point)
		//	




		///get offset
		/// rotate offset
		/// rotate ground
		/// apply new pos
		/// 
		/// 
		
		trueCAm.Rotation += camRot * 0.06f;
		
		Vector3 offset = ground.GlobalPosition - camera.GlobalPosition;
		Vector3 offsetTest = ground.GlobalPosition - camera.GlobalPosition;

		offsetTest = offsetTest.Rotated(Vector3.Back, -direction.X * 0.3f);
		offsetTest = offsetTest.Rotated(Vector3.Right, direction.Y * 0.3f);


		refere.GlobalPosition = refer.GlobalPosition + offset;
		refer.Rotation = camera.Rotation;
		//offset = offset.Rotated(new Vector3(1, 1, 0).Normalized(), 0.3f * cameraMoveDir.X);
		//offset = offset.Rotated(new Vector3(0, 1, 1).Normalized(), 0.3f * cameraMoveDir.Y);
		vect.TargetPosition = offset;
		//Man(delta);
		camera.Rotation = GroundRot *0.3f;
		ground.Rotation = camera.Rotation;
		if(direction.X != 0)
		{
		GD.Print("----------");
		GD.Print(refere.GlobalPosition);
		GD.Print(offsetTest);
			
		}

		ground.GlobalPosition = camera.GlobalPosition + refere.GlobalPosition;
		//ground.GlobalPosition = camera.GlobalPosition + offsetTest;
		//camera.Rota


		//ground.GlobalPosition = posGround;

		//body.ApplyCentralForce(ballPushVector);
		//body.ApplyForce(ballPushVector, new Vector3(0, 0.1f, 0));
		//if(body.GetCollidingBodies().Count > 0 && !bufferJump)
		//{
		//	canJump = true;
		//}
		//if (Input.IsActionPressed("jump") && canJump)
		//{
		//	canJump = false;
		//	bufferJump = true;
		//	timer.Start();
		//	Vector3 jumpVect = new Vector3(0, jumpForce, 0);
		//	body.ApplyCentralImpulse(jumpVect);
		//}

	}

	private void Man(double delt)
	{
		refer.Rotate(Vector3.Up, 1 * (float)delt);
		//GD.Print("--------------");
		//GD.Print(refer.Rotation);
		//GD.Print(refere.GlobalPosition);
		//GD.Print(refere.Position);
	}
}

