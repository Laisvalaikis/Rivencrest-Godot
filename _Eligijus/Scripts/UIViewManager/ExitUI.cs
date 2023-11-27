using Godot;
using System;

public partial class ExitUI : Node
{
	[Export] private View pauseMenuView;

	public override void _Ready()
	{
		base._Ready();
		if (InputManager.Instance != null)
		{
			InputManager.Instance.ExitView += Escape;
		}
	}

	private void Escape()
	{
		if (UIStack.HasAnyViewToQuit())
		{
			UIStack.QuitLast();
		}
		else
		{
			if (pauseMenuView != null)
			{
				pauseMenuView.OpenView();
			}
		}
	}
}
