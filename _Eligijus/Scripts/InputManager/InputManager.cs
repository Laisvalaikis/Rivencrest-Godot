using Godot;

public partial class InputManager: Node2D
{
	public static InputManager Instance;
	
	[Signal] public delegate void LeftMouseClickEventHandler();
	[Signal] public delegate void LeftMouseDoubleClickEventHandler(Vector2 mousePosition);
	[Signal] public delegate void ExitViewEventHandler();
	[Signal] public delegate void MouseMoveEventHandler(Vector2 relativeValues);
	[Signal] public delegate void MouseMoveOutDeadZoneEventHandler(Vector2 mousePosition);

	[Export] private Vector2 deadZone = new Vector2(10f, 10f);
	
	private string mouseClick = "MouseClickLeft";
	private Vector2 mousePosition;
	private bool selectIsClicked = false;
	private Vector2 mouseRelativePosition;
	private Vector2 deadzoneBoundsX;
	private Vector2 deadzoneBoundsY;
	private bool inDeadZone = true;

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
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		// base._UnhandledInput(@event);

		if (@event is InputEventMouseButton)
		{
			InputEventMouseButton input = (InputEventMouseButton)@event;
			if (input.IsPressed())
			{
				selectIsClicked = true;
				Vector2 mouseClickPosition = GetLocalMousePosition();
				deadzoneBoundsX.X = mouseClickPosition.X - deadZone.X;
				deadzoneBoundsX.Y = mouseClickPosition.X + deadZone.X;
				deadzoneBoundsY.X = mouseClickPosition.Y - deadZone.Y;
				deadzoneBoundsY.Y = mouseClickPosition.Y + deadZone.Y;
				mouseRelativePosition = mouseClickPosition;
				inDeadZone = true;
			}
			else if (input.IsReleased())
			{
				
				selectIsClicked = false;
				if (inDeadZone)
				{
					OnLeftMouseClick();
				}
			}
			if (input.DoubleClick)
			{
				EmitSignal("LeftMouseDoubleClick", GetGlobalMousePosition());
			}

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
					OnMoveOutsideDeadZone(input.Relative);
				}
			}
			else if(!selectIsClicked)
			{
				TrackMousePosition();
				OnMouseMove();
			}
		}

		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.Escape)
			{
				OnExitViewClick();
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
	
	public void OnLeftMouseClick()
	{
		
		EmitSignal("LeftMouseClick");
	}

	public void OnExitViewClick()
	{
		
		EmitSignal("ExitView");
	}

	private void TrackMousePosition()
	{
		mousePosition = GetGlobalMousePosition();
	}

	public void OnMouseMove()
	{
		EmitSignal("MouseMove", mousePosition);
	}

	private void OnMoveOutsideDeadZone(Vector2 relativePosition)
	{
		EmitSignal("MouseMoveOutDeadZone", relativePosition);
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

