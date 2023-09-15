using Godot;
using System;

public partial class ExitUI : Node
{
	[Export] private View pauseMenuView;
	
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Escape)
			{
				Escape();
			}
		}
	}
	
	// public void OnEscape(InputAction.CallbackContext context)
	// {
	// 	if (context.performed)
	// 		Escape();
	// }
   
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
