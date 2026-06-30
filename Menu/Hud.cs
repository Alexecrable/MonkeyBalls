using Godot;
using System;

public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void GameOverEventHandler();
	private Label tacosLabel, livesLabel, timeLabel;
	private int tacosCounter, livesCounter, timeCounter;
	
	private double timeElapsed;
	private TextureRect tacosTexture;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timeElapsed = 0;
		tacosLabel = GetNode<Label>("Control/TacosContainer/Label");
		livesLabel = GetNode<Label>("Control/LivesContainer/Label");
		timeLabel = GetNode<Label>("Control/TimerLabel");
		tacosTexture = GetNode<TextureRect>("Control/TacosContainer/TextureRect");
		tacosCounter = 0;
		livesCounter = 3;
		
	}

	public void LaunchTimer()
	{
		timeElapsed = 0;
	}


	public void ResetTacos()
	{
		tacosCounter = 0;
		UpdateTacosLabel();
	}

	public override void _Process(double delta)
    {
        timeElapsed += delta;
		int minutes = (int)timeElapsed / 60;
		int seconds = (int)(timeElapsed - (minutes * 60));
		int ms = (int)((timeElapsed - (minutes * 60) - seconds) * Math.Pow(10,3));
		timeLabel.Text = minutes + ":" + seconds.ToString("00") + ":" + ms.ToString("000");
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
