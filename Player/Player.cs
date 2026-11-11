
using System;
using System.Transactions;
using Godot;

/// FAIRE TOURNER LA BOULE AU LIEU D'APPLIQUER UNE FORCE EXTERIEURE POUR VOIR SI ÇA LA FAIT AVANCER
/// 
public partial class Player : Node3D
{
	[Signal]
	public delegate void TacosCollectEventHandler(int tacosGain, Vector2 pos);
	[Signal]
	public delegate void DeathSignalEventHandler();
	[Signal]
	public delegate void SpawnSignalEventHandler();
	[Signal]
	public delegate void LevelEndEventHandler();
	[Export]
	public float bodySpeed, camSpeed, camDistance, camFOV, mouseSensitivity, jumpForce, dashForce, dashCoolDownTime;
	private bool dying;
	private Vector3 cameraSavePos, cameraSaveRot, mouseInputVector;
	public bool IsMonkeyBall { get; set; }
	private Timer dashCoolDownTimer;

	private RigidBody3D body;
	private AudioStreamPlayer3D musicPlayer;
	private Level levelToMove;

	private Node3D cameraAnchor, skyBox;
	private Camera3D camera;
	private bool canJump, canDash;
	private Timer respawnTimer;
	private bool started;

	private float xCamSave, yCamSave;

	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{

		dashCoolDownTimer = new Timer()
		{
			WaitTime = dashCoolDownTime,
			OneShot = true,
			Autostart = false
		};


		AddChild(dashCoolDownTimer);


		musicPlayer = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");

		body = GetNode<RigidBody3D>("PlayerBody");

		cameraAnchor = GetNode<Node3D>("CameraAnchor");
		xCamSave = cameraAnchor.GlobalRotation.X;
		yCamSave = cameraAnchor.GlobalRotation.Y;

		camera = cameraAnchor.GetNode<Camera3D>("Camera3D");
		camera.Fov = camFOV;
		camera.Position = new Vector3(0, 0.5f, camDistance);
		cameraSavePos = camera.Position;
		cameraSaveRot = camera.Rotation;


		respawnTimer = new Timer
		{
			WaitTime = 3,
			Autostart = false,
			OneShot = true
		};
		respawnTimer.Timeout += Spawn;
		AddChild(respawnTimer);


	}


	//TODOS PR BONNE SEPARATION
	private void Spawn()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		body.BodyEntered += BodyEntered;
		body.GravityScale = 0;
		canJump = false;
		canDash = false;
		dying = false;
		started = false;

		body.LinearVelocity = Vector3.Zero;
		body.AngularVelocity = Vector3.Zero;
		body.Position = Vector3.Zero;
		body.Rotation = Vector3.Zero;
		cameraAnchor.Rotation = Vector3.Zero;
		dying = false;
	
		EmitSignal(SignalName.SpawnSignal);
	}

	private void RESET()
	{

	}

	private void BodyEntered(Node _body)
	{
		if (_body.Name.ToString().Contains("Bumper"))
		{
			GD.Print("yeet");
			((Bumper)_body).YEET(body);
		}
	}
	public void SetLevelToMove(Level level)
	{
		levelToMove = level;
	}

	public override void _UnhandledInput(InputEvent @event)
	{



		if (@event is InputEventMouseMotion)
		{
			InputEventMouseMotion mouseMotion = (InputEventMouseMotion)@event;
			float rotationInput = -mouseMotion.Relative.X * camSpeed;
			float tiltInput = -mouseMotion.Relative.Y * camSpeed;
			mouseInputVector = new Vector3(tiltInput, rotationInput, 0) * mouseSensitivity;
		}

	}


	public void init()
	{
		//Respawn();
		Spawn();
	}

	public void SetSky(Node3D _skyBox)
	{
		skyBox = _skyBox;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		int nbCol = 0;
		foreach (CollisionObject3D _node in body.GetCollidingBodies())
		{
			nbCol++;
		}
		if (nbCol > 0)
		{
			canJump = true;
			canDash = true;
		}
		//else{canJump = false;}
		GD.Print("jump" + nbCol + canJump);
		//if (!started)
		//{
		//	
		//	
		//	return;
		//}
		GD.Print("music" + musicPlayer.GlobalPosition);
		musicPlayer.Position = body.Position;
		if (dying)
		{
			camera.Position = cameraSavePos;
			camera.LookAt(body.Position);
			return;
		}
		cameraAnchor.Position = body.Position;
		GD.Print("camera " + camera.Position + " | " + camera.Rotation);
		camera.Position = cameraSavePos;
		camera.Rotation = cameraSaveRot;
		Vector2 cameraMoveDir = Input.GetVector("camLeft", "camRight", "camForward", "camBackward");
		bool wantsToJump = Input.IsActionPressed("jump");
		bool dashing = Input.IsActionJustPressed("dash");


		Vector3 joyStickVector = new Vector3(-cameraMoveDir.Y, -cameraMoveDir.X, 0) * camSpeed;


		Vector3 camRot = (joyStickVector + mouseInputVector) * (float)delta;
		mouseInputVector = Vector3.Zero;

		float xToClamp = cameraAnchor.GlobalRotation.X + camRot.X;
		GD.Print("clamp1 " + xToClamp);

		cameraAnchor.GlobalRotation = new Vector3((float)Math.Clamp(xToClamp, -0.6, 0.6), camRot.Y + cameraAnchor.GlobalRotation.Y, cameraAnchor.GlobalRotation.Z);
		Vector2 movementDirection = Input.GetVector("left", "right", "forward", "backward");

		if (Input.IsActionJustPressed("scroll-up"))
		{
			camera.Fov -= 10;
		}
		if (Input.IsActionJustPressed("scroll-down"))
		{
			camera.Fov += 10;
		}
		if (canJump && wantsToJump)
		{
			GD.Print("JUMP");
			body.LinearVelocity = new Vector3(body.LinearVelocity.X, 0, body.LinearVelocity.Z);
			body.ApplyCentralImpulse(new Vector3(0, jumpForce, 0));
			canJump = false;

		}
		if (!IsMonkeyBall)
		{


			float realBodySpeed = bodySpeed * (float)delta;


			Vector3 falseBallVector = new Vector3(movementDirection.X, 0, movementDirection.Y).Rotated(Vector3.Up, cameraAnchor.GlobalRotation.Y);
			Vector3 ballPushVector = falseBallVector * realBodySpeed / 2;
			if (!ballPushVector.IsZeroApprox())
			{
				body.ApplyTorque(ballPushVector.Rotated(Vector3.Up, Mathf.DegToRad(90)));
				//body.Rotate(ballPushVector.Normalized(), -0.1f);
				//body.ApplyCentralForce(ballPushVector);
				if (dashing && canDash && dashCoolDownTimer.TimeLeft == 0)
				{
					body.ApplyCentralImpulse(ballPushVector * dashForce);
					canDash = false;
					dashCoolDownTimer.Start();
				}

			}


		}
		else
		{
			Vector3 groundRot = new Vector3(movementDirection.Y, 0, -movementDirection.X).Rotated(Vector3.Up, cameraAnchor.GlobalRotation.Y) * 0.6f;
			Vector3 saveCam = cameraAnchor.GlobalRotation;
			Vector3 saveBody = body.GlobalRotation;
			Node3D parent = GetParent<Node3D>();
			parent.GlobalRotation = parent.GlobalRotation.Lerp(groundRot, 0.5f);
			cameraAnchor.GlobalRotation = new Vector3(saveCam.X, saveCam.Y, 0);
			body.GlobalRotation = saveBody;
			skyBox.GlobalRotation = Vector3.Zero;
			//cameraAnchor.Rotation = -groundRot;
			//body.Rotation = -groundRot;
		}

		//body.ApplyForce(ballPushVector, new Vector3(0, 0.1f, 0));

	}

	public void CountDownEnd()
	{
		started = true;
		body.GravityScale = 1;
	}
	public void EndLevel()
	{
		EmitSignal(SignalName.LevelEnd);
	}
	public void UpdateTacos(int tacosGain, Vector3 tacosGlobPos)
	{

		Vector2 pos = camera.UnprojectPosition(tacosGlobPos);
		EmitSignal(SignalName.TacosCollect, tacosGain, pos);
	}

	//private void Respawn()
	//{
//
	//	body.LinearVelocity = Vector3.Zero;
	//	body.AngularVelocity = Vector3.Zero;
	//	body.Position = Vector3.Zero;
	//	body.Rotation = Vector3.Zero;
	//	cameraAnchor.Rotation = Vector3.Zero;
	//	dying = false;
//
	//}

	public void Death()
	{
		dying = true;
		EmitSignal(SignalName.DeathSignal);
		respawnTimer.Start();
		started = false;
	}
}
