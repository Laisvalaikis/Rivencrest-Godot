using Godot;
using System;

public partial class ExitUI : Node
{
	[Export] private View pauseMenuView;
	[Export] private bool openMenuWithExitButton = true;

	public override void _Ready()
	{
		base._Ready();
		if (InputManager.Instance != null)
		{
			InputManager.Instance.Exit += Escape;
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
			if (pauseMenuView != null && openMenuWithExitButton)
			{
				pauseMenuView.OpenView();
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		InputManager.Instance.Exit -= Escape;
		base.Dispose(disposing);
	}
}
