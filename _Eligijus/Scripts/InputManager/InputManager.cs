using Godot;

public partial class InputManager: Node2D
{
	public static InputManager Instance;
	[Signal] public delegate void UpEventHandler(float increment);
	[Signal] public delegate void DownEventHandler(float increment);
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
	[Signal] public delegate void CharacterFocusInGameEventHandler();
	[Signal] public delegate void CameraControlEventHandler(Vector2 relativePosition, float movementSpeed);
	[Signal] public delegate void PauseMenuEventHandler();
	[Signal] public delegate void EndTurnEventHandler();
	
	[Signal] public delegate void UndoEventHandler();
	
	protected bool enabled = false;

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
	
	public virtual void FocusEnter()
	{
		
	}
	
	public virtual void FocusExit()
	{
		
	}
}

