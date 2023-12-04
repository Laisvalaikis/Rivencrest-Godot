using Godot;

public partial class InputManager: Node2D
{
	public static InputManager Instance;
	public static InputScheme CurrentInputScheme = InputScheme.KeyboardAndMouse;
	[Export] private InputScheme controll = InputScheme.KeyboardAndMouse;
	[Signal] public delegate void UpEventHandler();
	[Signal] public delegate void DownEventHandler();
	[Signal] public delegate void SelectClickEventHandler(Vector2 mouseClick);
	[Signal] public delegate void LeftMouseDoubleClickEventHandler(Vector2 mousePosition);
	[Signal] public delegate void ExitEventHandler();
	[Signal] public delegate void ChangeAbilityNextEventHandler();
	[Signal] public delegate void ChangeAbilityPreviousEventHandler();
	[Signal] public delegate void MoveSelectorEventHandler(Vector2 position);
	[Signal] public delegate void DisableSelectorEventHandler(Vector2 position);
	[Signal] public delegate void EnableSelectorEventHandler(Vector2 position);
	[Signal] public delegate void CameraControlEventHandler(Vector2 relativePosition, float movementSpeed);
	[Export] private Vector2 deadZone = new Vector2(10f, 10f);
	[Export] private float cameraMovementSpeedMouse = 1.0f;
	[Export] private float cameraMovementSpeedController = 10.0f;
	[Export] private float selectChunkControllerSpeed = 10.0f;
	private string mouseClick = "MouseClickLeft";
	private Vector2 _mousePosition;
	private bool selectIsClicked = false;
	private Vector2 mouseRelativePosition;
	private Vector2 deadzoneBoundsX;
	private Vector2 deadzoneBoundsY;
	private bool inDeadZone = true;
	private bool _selectedTileWasVisible = true;
	private bool _tileSelectionClicked = false;
	public override void _EnterTree()
	{
		base._EnterTree();
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			QueueFree();
		}
		CurrentInputScheme = controll;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// base._UnhandledInput(@event);

		if (!_tileSelectionClicked && !Input.IsActionJustPressed("CameraMovementRight") && !Input.IsActionJustPressed("CameraMovementLeft") && (Input.IsActionJustPressed("CameraMovementUp") || Input.IsActionJustPressed("CameraMovementDown")))
		{
			ControllerMousePosition();
			_tileSelectionClicked = true;
		}
		else if (!_tileSelectionClicked && !Input.IsActionJustPressed("CameraMovementUp") && !Input.IsActionJustPressed("CameraMovementDown") && (Input.IsActionJustPressed("CameraMovementRight") || Input.IsActionJustPressed("CameraMovementLeft")))
		{
			ControllerMousePosition();
			_tileSelectionClicked = true;
		}
		else if (_tileSelectionClicked && Input.IsActionJustReleased("CameraMovementUp") && Input.IsActionJustReleased("CameraMovementDown") && Input.IsActionJustReleased("CameraMovementRight") && Input.IsActionJustReleased("CameraMovementLeft"))
		{
			_tileSelectionClicked = false;
		}
		
		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton input = (InputEventMouseButton)@event;
			if (input.ButtonIndex == MouseButton.Left)
			{
				if (input.IsPressed())
				{
					
				}
				else if (input.IsReleased())
				{

					
				}

				// if (input.DoubleClick)
				// {
				// 	EmitSignal("LeftMouseDoubleClick", GetGlobalMousePosition());
				// }
			}
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
		else if(Input.IsActionJustReleased("Select"))
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
		
		if(Input.IsActionPressed("PreviousAbility"))
		{
			EmitSignal("ChangeAbilityPrevious");
		}

		if (Input.IsActionPressed("Up"))
		{
			GD.PrintErr(Input.IsActionPressed("Exit"));
			EmitSignal("Up");
		}
			
		if (Input.IsActionPressed("Down"))
		{
			GD.PrintErr(Input.IsActionPressed("Exit"));
			EmitSignal("Down");
		}

		if (CurrentInputScheme == InputScheme.KeyboardAndMouse)
		{
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
		}

		if (Input.IsActionPressed("Exit"))
		{
			OnExitClick();
		
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

	public void SetCurrentCharacterPosition(Vector2 characterPosition)
	{
		_mousePosition = characterPosition;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		if (Input.IsActionPressed("CameraMovementUp", true) || Input.IsActionPressed("CameraMovementDown", true) || Input.IsActionPressed("CameraMovementRight", true) || Input.IsActionPressed("CameraMovementLeft", true))
		{
			Vector2 relativePosition = Vector2.Zero;
			relativePosition = Input.GetVector( "CameraMovementRight", "CameraMovementLeft", "CameraMovementDown", "CameraMovementUp");
			// GD.PrintErr(relativePosition);
			CameraMovement(relativePosition, cameraMovementSpeedController);
		}
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsPressed())
		{
			// GD.Print(@event.AsText());
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
	
	public void OnSelectClick()
	{
		EmitSignal("SelectClick", _mousePosition);
	}

	public void OnExitClick()
	{
		
		EmitSignal("Exit");
	}

	private void TrackMousePosition()
	{
		_mousePosition = GetGlobalMousePosition();
	}

	public void OnSelectorMove(Vector2 mousePosition)
	{
		EmitSignal("MoveSelector", mousePosition);
	}

	private void CameraMovement(Vector2 relativePosition, float cameraMovementSpeed)
	{
		EmitSignal("CameraControl", relativePosition, cameraMovementSpeed);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (Instance == this)
		{
			Instance = null;
		}
	}
}

