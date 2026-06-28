using Godot;
using System;

public partial class LevelButton : Button
{
	private String levelName;
	// Called when the node enters the scene tree for the first time.
	public LevelButton()
	{
		
	}
	public LevelButton(String _levelName, CompressedTexture2D _image)
	{
		levelName = _levelName;
		Icon = _image;
	}
}
