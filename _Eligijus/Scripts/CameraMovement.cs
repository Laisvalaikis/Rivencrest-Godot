using System;
using Godot;

public partial class CameraMovement : Camera2D
{
    [Export]
    private float minZoom = 0.7f;
    [Export]
    private float maxZoom = 2f;
    [Export] 
    private float _zoomRate = 8.0f;
    [Export]
    private float zoomIncrement = 0.1f;
    [Export] 
    private Vector2 xBounds = new Vector2(-200f, 200f);
    private Vector2 yBounds = new Vector2(-200f, 200f);
    private float _targetZoom = 1.0f;
    private Tween _tween;

    public override void _Ready()
    {
        base._Ready();
        if (InputManager.Instance != null)
        {
            InputManager.Instance.MouseMoveOutDeadZone += MoveCamera;
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        double x = Mathf.Lerp(Zoom.X, _targetZoom * Vector2.One.X, _zoomRate * delta);
        double y = Mathf.Lerp(Zoom.Y, _targetZoom * Vector2.One.Y, _zoomRate * delta);
        Zoom = new Vector2((float)x, (float)y);
        SetPhysicsProcess(!Zoom.X.Equals(_targetZoom));
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        // if (@event is InputEventMouseButton)
        // {
        //     if (@event.IsPressed())
        //     {
        //         InputEventMouseButton mouseButton = (InputEventMouseButton)@event;
        //         if (mouseButton.ButtonIndex == MouseButton.WheelUp)
        //         {
        //             ZoomIn();
        //         }
        //         if (mouseButton.ButtonIndex == MouseButton.WheelDown)
        //         {
        //             ZoomOut();
        //         }
        //
        //         // if (mouseButton.DoubleClick)
        //         // {
        //         //     
        //         // }
        //     }
        // }
        
        // if (@event is InputEventMouseMotion)
        // {
        //     InputEventMouseMotion input = (InputEventMouseMotion)@event;
        //     if (input.ButtonMask == MouseButtonMask.Left)
        //     {
        //         if (Position.X <= xBounds.Y && Position.Y <= yBounds.Y && Position.X >= xBounds.X && Position.Y >= yBounds.X)
        //         {
        //             Position -= input.Relative * Zoom;
        //         }
        //         
        //         if(Position.X > xBounds.Y)
        //         {
        //             Position = new Vector2(xBounds.Y, Position.Y);
        //         }
        //         if (Position.Y > yBounds.Y)
        //         {
        //             Position = new Vector2(Position.X, yBounds.Y);
        //         }
        //         if(Position.X < xBounds.X)
        //         {
        //             Position = new Vector2(xBounds.X, Position.Y);
        //         }
        //         if (Position.Y < yBounds.X)
        //         {
        //             Position = new Vector2(Position.X, yBounds.X);
        //         }
        //
        //     }
        // }
    }

    private void MoveCamera(Vector2 relativePosition)
    {
        if (Position.X <= xBounds.Y && Position.Y <= yBounds.Y && Position.X >= xBounds.X && Position.Y >= yBounds.X)
        {
            Position -= relativePosition * Zoom;
        }
        
        if(Position.X > xBounds.Y)
        {
            Position = new Vector2(xBounds.Y, Position.Y);
        }
        if (Position.Y > yBounds.Y)
        {
            Position = new Vector2(Position.X, yBounds.Y);
        }
        if(Position.X < xBounds.X)
        {
            Position = new Vector2(xBounds.X, Position.Y);
        }
        if (Position.Y < yBounds.X)
        {
            Position = new Vector2(Position.X, yBounds.X);
        }
        
    }

    public void SetMovementBounds(Vector2 xMapBounds, Vector2 yMapBounds)
    {
        xBounds = xMapBounds;
        yBounds = yMapBounds;
    }

    public void ZoomIn()
    {
        _targetZoom = Math.Max(_targetZoom - zoomIncrement, minZoom);
        SetPhysicsProcess(true);
    }
    
    public void ZoomOut()
    {
        _targetZoom = Math.Min(_targetZoom + zoomIncrement, maxZoom);
        SetPhysicsProcess(true);
    }

    // public void FocusPoint(Vector2 targetPosition)
    // {
    //     _tween.Stop(this, "position");
    //     // _tween.interpolate_property(self, "position", position, target_position, 0.2, Tween.TRANS_EXPO)
    //     _tween.
    // }
}