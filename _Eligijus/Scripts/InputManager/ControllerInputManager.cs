using Godot;

public partial class ControllerInputManager : InputManager
{
	[Export] private Vector2 deadZone = new Vector2(10f, 10f);
	[Export] private float cameraMovementSpeedController = 10.0f;
	[Export(PropertyHint.Range, "0,1,")] private float zoomStrength = 0.7f;
	[Export] private double timeTrigerMapMovement = 0.2;
	private Vector2 _mousePosition;
	private bool selectIsClicked = false;
	private Vector2 mouseRelativePosition;
	private Vector2 deadzoneBoundsX;
	private Vector2 deadzoneBoundsY;
	private bool inDeadZone = true;
	private bool _tileSelectionClicked = false;
	private bool upIsPressed = false;
	private bool downIsPressed = false;
	private bool isFocused = false;
	private double timer = 0;
	private Vector2 relativePositionInput = Vector2.Zero;
	private bool encounterIsStarted = false;
	
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);

		if (enabled)
		{
			if (Input.IsActionJustPressed("Select"))
			{
				EmitSignal("UnHandledSelectClick", Vector2.Zero);
			}
			
			if (Input.IsActionJustPressed("NextCharacter"))
			{
				EmitSignal("ChangeNextCharacter");
			}

			if (Input.IsActionJustPressed("PreviousCharacter"))
			{
				EmitSignal("ChangePreviousCharacter");
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (enabled)
		{
			
			if (Input.IsActionJustPressed("ReleaseFocus") && !isFocused)
			{
				EmitSignal("ReleaseFocusWhenNotFocused");
				EmitSignal("EnableSelector", _mousePosition);
			}
			
			if (Input.IsActionJustPressed("ReleaseFocus") && isFocused)
			{
				EmitSignal("ReleaseFocus");
				EmitSignal("EnableSelector", _mousePosition);
			}
			
			if (!isFocused && !inGameFocus)
			{

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
			}

			if (Input.IsActionJustPressed("NextAbility"))
			{
				EmitSignal("ChangeAbilityNext");
			}

			if (Input.IsActionJustPressed("PreviousAbility"))
			{
				EmitSignal("ChangeAbilityPrevious");
			}

			if (Input.IsActionJustPressed("Pause"))
			{
				EmitSignal("PauseMenu");
			}

			if (Input.IsActionJustPressed("EndTurn"))
			{
				EmitSignal("EndTurn");
			}
			
			if (Input.IsActionJustPressed("Undo"))
			{
				EmitSignal("Undo");
			}

			if (!isFocused && !inGameFocus && GameTileMap.Tilemap != null)
			{
				
				if (!_tileSelectionClicked && !Input.IsActionJustPressed("CameraMovementRight") &&
				    !Input.IsActionJustPressed("CameraMovementLeft") &&
				    (Input.IsActionJustPressed("CameraMovementUp") ||
				     Input.IsActionJustPressed("CameraMovementDown")))
				{
					relativePositionInput = Input.GetVector( "CameraMovementRight", "CameraMovementLeft", "CameraMovementDown", "CameraMovementUp");
					_tileSelectionClicked = true;
				}
				else if (!_tileSelectionClicked && !Input.IsActionJustPressed("CameraMovementUp") &&
				         !Input.IsActionJustPressed("CameraMovementDown") &&
				         (Input.IsActionJustPressed("CameraMovementRight") ||
				          Input.IsActionJustPressed("CameraMovementLeft")))
				{
					relativePositionInput = Input.GetVector( "CameraMovementRight", "CameraMovementLeft", "CameraMovementDown", "CameraMovementUp");
					_tileSelectionClicked = true;
				}
				if (_tileSelectionClicked && !Input.IsActionPressed("CameraMovementUp") && // this is not working
				    !Input.IsActionPressed("CameraMovementDown") &&
				    !Input.IsActionPressed("CameraMovementRight") &&
				    !Input.IsActionPressed("CameraMovementLeft"))
				{
					if (timer < timeTrigerMapMovement)
					{
						ControllerMousePosition(relativePositionInput);
						relativePositionInput = Vector2.Zero;
					}
					timer = 0;
					_tileSelectionClicked = false;
				}
			}

			if (Input.IsActionJustPressed("CharacterFocusInGame"))
			{
				EmitSignal("CharacterFocusInGame");
				inGameFocus = true;
				EmitSignal("DisableSelector", _mousePosition);
			}

			if (Input.IsActionJustPressed("Exit"))
			{
				OnExitClick();

			}
		}

	}

	public void ControllerMousePosition(Vector2 position)
	{
		Vector2 relativePosition = position;
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

		if (enabled && !isFocused && !inGameFocus && GameTileMap.Tilemap != null)
		{
			if (Input.IsActionPressed("CameraMovementUp", true) || Input.IsActionPressed("CameraMovementDown", true) ||
			    Input.IsActionPressed("CameraMovementRight", true) || Input.IsActionPressed("CameraMovementLeft", true))
			{
				if (timer >= timeTrigerMapMovement)
				{
					Vector2 relativePosition = Vector2.Zero;
					relativePosition = Input.GetVector("CameraMovementRight", "CameraMovementLeft",
						"CameraMovementDown", "CameraMovementUp");
					CameraMovement(relativePosition, cameraMovementSpeedController);
				}
				timer += delta;
			}

			if (Input.IsActionPressed("Up", true))
			{
				float value = Input.GetActionStrength("Up");
				EmitSignal("Up",value * zoomStrength);
			}
			
			if ( Input.IsActionPressed("Down", true))
			{
				float value = Input.GetActionStrength("Down");
				EmitSignal("Down",value * zoomStrength);
			}
		}
		
		if (enabled && !isFocused)
		{

			if (!isInGame)
			{
				if (Input.IsActionJustPressed("FocusLeft", true))
				{
					EmitSignal("FocusLeft");
				}

				if (Input.IsActionJustPressed("FocusRight", true))
				{
					EmitSignal("FocusRight");
				}

				if (Input.IsActionJustPressed("FocusUp", true))
				{
					EmitSignal("FocusUp");
				}

				if (Input.IsActionJustPressed("FocusDown", true))
				{
					EmitSignal("FocusDown");
				}
			}
			else if(isInGame && inGameFocus)
			{
				if (Input.IsActionJustPressed("FocusLeft", true))
				{
					EmitSignal("FocusLeft");
				}

				if (Input.IsActionJustPressed("FocusRight", true))
				{
					EmitSignal("FocusRight");
				}

				if (Input.IsActionJustPressed("FocusUp", true))
				{
					EmitSignal("FocusUp");
				}

				if (Input.IsActionJustPressed("FocusDown", true))
				{
					EmitSignal("FocusDown");
				}
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

	public override void FocusExit()
	{
		base.FocusExit();
		isFocused = false;
	}
	
	public override void FocusEnter()
	{
		base.FocusEnter();
		isFocused = true;
	}

}