using Godot;

public partial class Player : Node3D
{
	[Signal]
	public delegate void TacosCollectEventHandler(int tacosGain, Vector2 pos);
	[Signal]
	public delegate void DeathSignalEventHandler();
	[Signal]
	public delegate void LevelEndEventHandler();
	[Export]
	public float bodySpeed, camSpeed, camDistance, camFOV;
	private bool dying;
	private Vector3 cameraSavePos, cameraSaveRot;

	private RigidBody3D body;
	private AudioStreamPlayer3D musicPlayer;

	private Node3D cameraAnchor;
	private Camera3D camera;
	private Timer respawnTimer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		musicPlayer = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");

		body = GetNode<RigidBody3D>("PlayerBody");
		cameraAnchor = GetNode<Node3D>("CameraAnchor");
		camera = cameraAnchor.GetNode<Camera3D>("Camera3D");
		camera.Fov = camFOV;
		camera.Position = new Vector3(0, 0.5f, camDistance);
		cameraSavePos = camera.Position;
		cameraSaveRot = camera.Rotation;
		dying = false;
		respawnTimer = new Timer
		{
			WaitTime = 3,
			Autostart = false,
			OneShot = true
		};
		respawnTimer.Timeout += Respawn;
		AddChild(respawnTimer);
	}

	public void init()
	{
		Respawn();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
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

		float realBodySpeed = bodySpeed * (float)delta;
		float realCamSpeed = camSpeed * (float)delta;

		Vector2 movementDirection = Input.GetVector("left", "right", "forward", "backward");
		Vector2 cameraMoveDir = Input.GetVector("camLeft", "camRight", "camForward", "camBackward");

		Vector3 ballPushVector = new Vector3(movementDirection.X, 0, movementDirection.Y).Rotated(Vector3.Up, cameraAnchor.GlobalRotation.Y) * realBodySpeed;
		Vector3 camRot = new Vector3(-cameraMoveDir.Y, -cameraMoveDir.X, 0) * realCamSpeed;

		cameraAnchor.Rotation += camRot;
		body.ApplyCentralForce(ballPushVector);
		//body.ApplyForce(ballPushVector, new Vector3(0, 0.1f, 0));

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

	private void Respawn()
	{

		body.LinearVelocity = Vector3.Zero;
		body.AngularVelocity = Vector3.Zero;
		body.Position = Vector3.Zero;
		body.Rotation = Vector3.Zero;
		cameraAnchor.Rotation = Vector3.Zero;
		dying = false;

	}

	public void Death()
	{
		dying = true;
		EmitSignal(SignalName.DeathSignal);
		respawnTimer.Start();
	}
}
