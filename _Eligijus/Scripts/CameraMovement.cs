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
    private float _targetZoom = 1.0f;
    private Tween _tween;

    public override void _Ready()
    {
        base._Ready();
        // _tween = GetTree().CreateTween();
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        double x = Mathf.Lerp(Zoom.X, _targetZoom * Vector2.One.X, _zoomRate * delta);
        double y = Mathf.Lerp(Zoom.Y, _targetZoom * Vector2.One.Y, _zoomRate * delta);
        Zoom = new Vector2((float)x, (float)y);
        SetPhysicsProcess(!Zoom.X.Equals(_targetZoom));
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
        if (@event is InputEventMouseButton)
        {
            if (@event.IsPressed())
            {
                InputEventMouseButton mouseButton = (InputEventMouseButton)@event;
                if (mouseButton.ButtonIndex == MouseButton.WheelUp)
                {
                    ZoomIn();
                }
                if (mouseButton.ButtonIndex == MouseButton.WheelDown)
                {
                    ZoomOut();
                }

                // if (mouseButton.DoubleClick)
                // {
                //     
                // }
            }
        }

        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion input = (InputEventMouseMotion)@event;
            if (input.ButtonMask == MouseButtonMask.Left)
            {
                Position -= input.Relative * Zoom;
            }
        }
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