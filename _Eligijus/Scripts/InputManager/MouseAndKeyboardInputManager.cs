using Godot;

public partial class MouseAndKeyboardInputManager : InputManager
{
	[Export] private Vector2 deadZone = new Vector2(10f, 10f);
	[Export] private float cameraMovementSpeedMouse = 1.0f;
	private Vector2 _mousePosition;
	private bool selectIsClicked = false;
	private Vector2 mouseRelativePosition;
	private Vector2 deadzoneBoundsX;
	private Vector2 deadzoneBoundsY;
	private bool inDeadZone = true;

	public override void _UnhandledInput(InputEvent @event)
	{
		if (enabled)
		{
			if (Input.IsActionJustPressed("ReleaseFocus"))
			{
				EmitSignal("ReleaseFocus");
			}

			if (Input.IsActionJustPressed("Select"))
			{
				selectIsClicked = true;
				deadzoneBoundsX.X = _mousePosition.X - deadZone.X;
				deadzoneBoundsX.Y = _mousePosition.X + deadZone.X;
				deadzoneBoundsY.X = _mousePosition.Y - deadZone.Y;
				deadzoneBoundsY.Y = _mousePosition.Y + deadZone.Y;
				mouseRelativePosition = _mousePosition;
				inDeadZone = true;
			}
			else if (Input.IsActionJustReleased("Select"))
			{
				selectIsClicked = false;
				if (inDeadZone)
				{
					OnSelectClick();
				}
			}

			if (Input.IsActionPressed("Up"))
			{
				EmitSignal("Up", 1.0);
			}

			if (Input.IsActionPressed("Down"))
			{
				EmitSignal("Down", 1.0);
			}


			if (@event is InputEventMouseMotion)
			{
				InputEventMouseMotion input = (InputEventMouseMotion)@event;
				if (input.ButtonMask == MouseButtonMask.Left && selectIsClicked)
				{
					mouseRelativePosition -= input.Relative;
					if (!InDeadZone(mouseRelativePosition))
					{
						inDeadZone = false;
					}

					if (!inDeadZone)
					{
						CameraMovement(input.Relative, cameraMovementSpeedMouse);
					}
				}
				else if (!selectIsClicked)
				{
					TrackMousePosition();
					OnSelectorMove(_mousePosition);
				}
			}

			if (Input.IsActionJustPressed("Exit"))
			{
				OnExitClick();
			}
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (enabled)
		{
			if (Input.IsActionJustPressed("Select"))
			{
				EmitSignal("UnHandledSelectClick", Vector2.Zero);
			}
		}
	}

	private bool InDeadZone(Vector2 relativePosition)
	{
		if (relativePosition.X >= deadzoneBoundsX.X && relativePosition.X <= deadzoneBoundsX.Y && relativePosition.Y >= deadzoneBoundsY.X && relativePosition.Y <= deadzoneBoundsY.Y)
		{
			return true;
		}
		return false;
	}

	private void TrackMousePosition()
	{
		_mousePosition = GetGlobalMousePosition();
	}
	
	public override void OnSelectClick()
	{
		EmitSignal("SelectClick", _mousePosition);
	}
	
}