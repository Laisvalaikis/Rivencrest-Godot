using Godot;

public partial class InputManager: Node2D
{
	public static InputManager Instance;
	[Signal] public delegate void UpEventHandler();
	[Signal] public delegate void DownEventHandler();
	[Signal] public delegate void SelectClickEventHandler(Vector2 mouseClick);
	[Signal] public delegate void LeftMouseDoubleClickEventHandler(Vector2 mousePosition);
	[Signal] public delegate void ExitEventHandler();
	[Signal] public delegate void ChangeAbilityNextEventHandler();
	[Signal] public delegate void ChangeAbilityPreviousEventHandler();
	[Signal] public delegate void ChangeNextCharacterEventHandler();
	[Signal] public delegate void ChangePreviousCharacterEventHandler();
	[Signal] public delegate void MoveSelectorEventHandler(Vector2 position);
	[Signal] public delegate void DisableSelectorEventHandler(Vector2 position);
	[Signal] public delegate void EnableSelectorEventHandler(Vector2 position);
	[Signal] public delegate void ReleaseFocusEventHandler();
	[Signal] public delegate void CameraControlEventHandler(Vector2 relativePosition, float movementSpeed);

	protected bool enabled = false;
	public override void _EnterTree()
	{
		base._EnterTree();
	}
	
	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		// if (@event.IsPressed())
		// {
		// 	GD.Print(@event.AsText());
		// }
		
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		base._UnhandledInput(@event);
	}

	public virtual void SetCurrentCharacterPosition(Vector2 characterPosition)
	{
		
	}
	
	public virtual void OnSelectClick()
	{
		
	}

	public virtual void OnExitClick()
	{
		if (enabled)
		{
			EmitSignal("Exit");
		}
	}

	public virtual void OnSelectorMove(Vector2 mousePosition)
	{
		if (enabled)
		{
			EmitSignal("MoveSelector", mousePosition);
		}
	}

	protected virtual void CameraMovement(Vector2 relativePosition, float cameraMovementSpeed)
	{
		if (enabled)
		{
			EmitSignal("CameraControl", relativePosition, cameraMovementSpeed);
		}
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (enabled)
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}
	}

	public virtual void EnableInputManager()
	{
		enabled = true;
		Instance = this;
	}
	
	public virtual void DisableInputManager()
	{
		enabled = false;
		Instance = null;
	}
}

