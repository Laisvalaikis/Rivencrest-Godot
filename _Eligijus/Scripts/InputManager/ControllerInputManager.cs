using Godot;

public partial class ControllerInputManager : InputManager
{
	[Export] private Vector2 deadZone = new Vector2(10f, 10f);
	[Export] private float cameraMovementSpeedController = 10.0f;
	private Vector2 _mousePosition;
	private bool selectIsClicked = false;
	private Vector2 mouseRelativePosition;
	private Vector2 deadzoneBoundsX;
	private Vector2 deadzoneBoundsY;
	private bool inDeadZone = true;
	private bool _tileSelectionClicked = false;
	
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (enabled)
		{

			if (Input.IsActionPressed("NextCharacter"))
			{
				EmitSignal("ChangeNextCharacter");
			}

			if (Input.IsActionPressed("PreviousCharacter"))
			{
				EmitSignal("ChangePreviousCharacter");
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (enabled)
		{
			if (Input.IsActionPressed("ReleaseFocus"))
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

			if (Input.IsActionPressed("NextAbility"))
			{
				EmitSignal("ChangeAbilityNext");
			}

			if (Input.IsActionPressed("PreviousAbility"))
			{
				EmitSignal("ChangeAbilityPrevious");
			}

			if (Input.IsActionPressed("Up"))
			{
				EmitSignal("Up");
			}

			if (Input.IsActionPressed("Down"))
			{
				EmitSignal("Down");
			}

			if (!_tileSelectionClicked && !Input.IsActionJustPressed("CameraMovementRight") &&
			    !Input.IsActionJustPressed("CameraMovementLeft") && (Input.IsActionJustPressed("CameraMovementUp") ||
			                                                         Input.IsActionJustPressed("CameraMovementDown")))
			{
				ControllerMousePosition();
				_tileSelectionClicked = true;
			}
			else if (!_tileSelectionClicked && !Input.IsActionJustPressed("CameraMovementUp") &&
			         !Input.IsActionJustPressed("CameraMovementDown") &&
			         (Input.IsActionJustPressed("CameraMovementRight") ||
			          Input.IsActionJustPressed("CameraMovementLeft")))
			{
				ControllerMousePosition();
				_tileSelectionClicked = true;
			}
			else if (_tileSelectionClicked && Input.IsActionJustReleased("CameraMovementUp") &&
			         Input.IsActionJustReleased("CameraMovementDown") &&
			         Input.IsActionJustReleased("CameraMovementRight") &&
			         Input.IsActionJustReleased("CameraMovementLeft"))
			{
				_tileSelectionClicked = false;
			}


			if (Input.IsActionPressed("Exit"))
			{
				OnExitClick();

			}
		}

	}

	public void ControllerMousePosition()
	{
		Vector2 relativePosition = Input.GetVector( "CameraMovementRight", "CameraMovementLeft", "CameraMovementDown", "CameraMovementUp");
		relativePosition = relativePosition.Normalized();
		if (Mathf.Abs(relativePosition.X) > Mathf.Abs(relativePosition.Y))
		{
			relativePosition.Y = 0.0f;
		}
		else
		{
			relativePosition.X = 0.0f;
		}
		
		if (Mathf.Abs(relativePosition.X) > 0.5f)
		{
			relativePosition.X = 1 * Mathf.Sign(relativePosition.X);
		}
		
		if (Mathf.Abs(relativePosition.Y) > 0.5f)
		{
			relativePosition.Y = 1 * Mathf.Sign(relativePosition.Y);
		}
		EmitSignal("DisableSelector", _mousePosition);
		if (GameTileMap.Tilemap.CheckMouseBounds(_mousePosition - relativePosition.Normalized() * (GameTileMap.Tilemap.currentMap._chunkSize)))
		{
			_mousePosition -= relativePosition.Normalized() * (GameTileMap.Tilemap.currentMap._chunkSize);
		}
		EmitSignal("EnableSelector", _mousePosition);
		OnSelectorMove(_mousePosition);
	}

	public override void SetCurrentCharacterPosition(Vector2 characterPosition)
	{
		if (enabled)
		{
			EmitSignal("DisableSelector", _mousePosition);
			_mousePosition = characterPosition;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (enabled)
		{
			if (Input.IsActionPressed("CameraMovementUp", true) || Input.IsActionPressed("CameraMovementDown", true) ||
			    Input.IsActionPressed("CameraMovementRight", true) || Input.IsActionPressed("CameraMovementLeft", true))
			{
				Vector2 relativePosition = Vector2.Zero;
				relativePosition = Input.GetVector("CameraMovementRight", "CameraMovementLeft",
					"CameraMovementDown", "CameraMovementUp");
				CameraMovement(relativePosition, cameraMovementSpeedController);
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
	
	public override void OnSelectClick()
	{
		EmitSignal("SelectClick", _mousePosition);
	}
	
}