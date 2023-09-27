using Godot;
using System;

public partial class Pause : Node
{
	public void PauseGame()
	{
		GetTree().Paused = true;
	}

	public void UnPauseGame()
	{
		GetTree().Paused = false;
	}
}
