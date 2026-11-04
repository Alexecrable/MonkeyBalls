using Godot;
using Godot.NativeInterop;
using System;

public partial class LevelButton : Button
{


	private String levelName;
	// Called when the node enters the scene tree for the first time.
	
	public void InitButton(String _levelName, CompressedTexture2D _image)
	{
		
		levelName = _levelName;
		GetNode<TextureRect>("VBoxContainer/TextureRect").Texture = _image;
		Test();
	}

	private void Test()
	{
		uint a = 0b_0100_01111;
		for(int i = 0; i < 8; i++)
		{
			if((a & (1 << i)) != 0)
			{
				GD.Print("Byte located at pos " + i);
				//Append this 1
			}
			else
			{
				GD.Print("nothing at pos " + i);
				//append the 0
			}
			
		}
		
		
		
	}
}
