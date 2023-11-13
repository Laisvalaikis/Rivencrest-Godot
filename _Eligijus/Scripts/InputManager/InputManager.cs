using Godot;

public partial class InputManager: Node2D
{
	public static InputManager Instance;
	
	private string mouseClick = "MouseClickLeft";
	[Signal]
	public delegate void LeftMouseClickEventHandler();
	[Signal]
	public delegate void MouseMoveEventHandler(Vector2 mousePosition);

	private Vector2 mousePosition;

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

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed(mouseClick))
		{
			OnLeftMouseClick();
		}
		else if (@event is InputEventMouseMotion eventMouseMotion)
		{
			TrackMousePosition();
			OnMouseMove();
		}
		
	}

	public void OnLeftMouseClick()
	{
		
		EmitSignal("LeftMouseClick");
	}

	private void TrackMousePosition()
	{
		mousePosition = GetGlobalMousePosition();
	}

	public void OnMouseMove()
	{
		EmitSignal("MouseMove", mousePosition);
		
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

