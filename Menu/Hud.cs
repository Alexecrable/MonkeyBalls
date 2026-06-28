using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void GameOverEventHandler();
	private Label tacosLabel, livesLabel, timeLabel;
	private int tacosCounter, livesCounter, timeCounter;
	private Timer timer;
	private TextureRect tacosTexture;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tacosLabel = GetNode<Label>("Control/TacosContainer/Label");
		livesLabel = GetNode<Label>("Control/LivesContainer/Label");
		timeLabel = GetNode<Label>("Control/TimerLabel");
		tacosTexture = GetNode<TextureRect>("Control/TacosContainer/TextureRect");
		tacosCounter = 0;
		livesCounter = 3;
		timeCounter = 0;
		timer = new Timer();
		timer.Timeout += UpdateTime;
		timer.Autostart = false;
		timer.OneShot = false;
		AddChild(timer);
	}

	public void LaunchTimer()
	{
		timer.Start();
	}

	public void StopTimer()
	{
		timer.Stop();
	}

	public void ResetTacos()
	{
		tacosCounter = 0;
		UpdateTacosLabel();
	}

	public void SetTime(int time)
	{
		timeCounter = time;
		UpdateTimeLabel();
	}

	private void UpdateTime()
	{
		timeCounter--;
		UpdateTimeLabel();
		if(timeCounter == 0)
		{
			EmitSignal(SignalName.GameOver);
		}
	}

	public void UpdateTacos(int tacosGain, Vector2 tacosPos)
	{
		tacosCounter = (tacosGain <= -tacosCounter) ? 0 : (tacosCounter + tacosGain);
		UpdateTacosLabel();
		TextureRect textureRect = new TextureRect();
		textureRect.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
		textureRect.Size = new Vector2(77,77);
		textureRect.Texture = tacosTexture.Texture;
		textureRect.GlobalPosition = tacosPos;
		AddChild(textureRect);
		Tween tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Sine);
		tween.TweenProperty(textureRect, "global_position", tacosTexture.GlobalPosition, 0.5);
		tween.Finished += () => {textureRect.QueueFree();};
	}

	public void GainLife()
	{
		livesCounter++;
		UpdateLifeLabel();
	}
	public void LoseLife()
	{
		livesCounter--;
		UpdateLifeLabel();
		if(livesCounter == 0)
		{
			EmitSignal(SignalName.GameOver);
		}
	}

	private void UpdateLifeLabel()
	{
		livesLabel.Text = ": " + livesCounter;
	}

	private void UpdateTacosLabel()
	{
		tacosLabel.Text = ": " + tacosCounter;
	}

	private void UpdateTimeLabel()
	{
		timeLabel.Text = timeCounter.ToString();
	}
}
