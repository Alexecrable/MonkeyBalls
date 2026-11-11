using System;
using Godot;


public partial class Hud : CanvasLayer
{
	[Signal]
	public delegate void GameOverEventHandler();
	[Signal]
	public delegate void CountDownEndEventHandler();
	private Label tacosLabel, livesLabel, timeLabel, countDownLabel;
	private int tacosCounter, livesCounter, timeCounter;


	private double timeElapsed;
	private TextureRect tacosTexture;
	private int countDownStep;
	private Control controlNode;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		controlNode = GetNode<Control>("Control");
		timeElapsed = 0;
		tacosLabel = controlNode.GetNode<Label>("TacosContainer/Label");
		livesLabel = controlNode.GetNode<Label>("LivesContainer/Label");
		countDownLabel = GetNode<Label>("CountDownLabel");
		timeLabel = controlNode.GetNode<Label>("TimerLabel");
		tacosTexture = controlNode.GetNode<TextureRect>("TacosContainer/TextureRect");
		tacosCounter = 0;
		livesCounter = 3;
		controlNode.Visible = false;


		//timer.Timeout += CountDownStep;
	}

	public void SaveScore()
	{

	}

	public void LaunchCountDown()
	{
		countDownStep = 3;
		countDownLabel.Visible = true;
		CountDownStep();

	}

	private void CountDownStep()
	{

		countDownLabel.Text = countDownStep.ToString();

		Tween tween = CreateTween();
		tween.SetParallel();

		tween.TweenProperty(countDownLabel.LabelSettings, "font_size", 20, 1);
		tween.TweenProperty(countDownLabel, "modulate", new Color(1, 1, 1, 0.3f), 1);

		tween.Finished += () =>
		{
			countDownLabel.LabelSettings.FontSize = 62;
			countDownLabel.Modulate = new Color(1, 1, 1, 1);
			countDownStep--;
			if (countDownStep != 0)
			{

				CountDownStep();

			}
			else
			{
				countDownLabel.Visible = false;
				controlNode.Visible = true; //Casse la souris ????
				ResetTacos();
				LaunchTimer();
				EmitSignal(SignalName.CountDownEnd);

			}
		};
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

	public double GetTime()
	{
		return timeElapsed;

	}

	public int GetTacos()
	{
		return tacosCounter;
	}


	public override void _Process(double delta)
	{
		timeElapsed += delta;
		int minutes = (int)timeElapsed / 60;
		int seconds = (int)(timeElapsed - (minutes * 60));
		int ms = (int)((timeElapsed - (minutes * 60) - seconds) * Math.Pow(10, 3));
		timeLabel.Text = minutes + ":" + seconds.ToString("00") + ":" + ms.ToString("000");
	}

	public void UpdateTacos(int tacosGain, Vector2 tacosPos)
	{
		tacosCounter = (tacosGain <= -tacosCounter) ? 0 : (tacosCounter + tacosGain);
		UpdateTacosLabel();
		TextureRect textureRect = new TextureRect();
		textureRect.ExpandMode = TextureRect.ExpandModeEnum.FitWidth;
		textureRect.Size = new Vector2(77, 77);
		textureRect.Texture = tacosTexture.Texture;
		textureRect.GlobalPosition = tacosPos;
		AddChild(textureRect);
		Tween tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Sine);
		tween.TweenProperty(textureRect, "global_position", tacosTexture.GlobalPosition, 0.5);
		tween.Finished += () => { textureRect.QueueFree(); };
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
		countDownLabel.Visible = true;
		controlNode.Visible = false; //Casse la souris ????
		if (livesCounter == 0)
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
