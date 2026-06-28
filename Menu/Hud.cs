using Godot;
using System;

public partial class Hud : CanvasLayer
{
	private Label tacosLabel, livesLabel, timeLabel;
	private int tacosCounter, livesCounter, timeCounter;
	private Timer timer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tacosLabel = GetNode<Label>("Control/TacosContainer/Label");
		livesLabel = GetNode<Label>("Control/LivesContainer/Label");
		timeLabel = GetNode<Label>("Control/TimerLabel");

		tacosCounter = 0;
		livesCounter = 0;
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
	}

	public void UpdateTacos(int tacosGain)
	{
		tacosCounter = (tacosGain <= -tacosCounter) ? 0 : (tacosCounter + tacosGain);
		UpdateTacosLabel();
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
		timeLabel.Text = ": " + timeCounter;
	}
}
