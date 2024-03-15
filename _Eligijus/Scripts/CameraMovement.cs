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
    [Export]
    private Vector2 yBounds = new Vector2(-200f, 200f);
    private float _targetZoom = 1.0f;
    private Vector2 orgXBoundsZoomOut;
    private Vector2 orgYBoundsZoomOut;
    private Vector2 orgXBounds;
    private Vector2 orgYBounds;
    private Tween _tween;
    private bool zoomIn = false;
    private Vector2 _tempPosition;

    public override void _Ready()
    {
        base._Ready();
        if (InputManager.Instance != null)
        {
            // InputManager.Instance.CameraControl += MoveCamera;
            InputManager.Instance.CameraControl += MoveCameraTiles;
            InputManager.Instance.LeftMouseDoubleClick += FocusPoint;
            InputManager.Instance.Up += ZoomIn;
            InputManager.Instance.Down += ZoomOut;
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        if (_targetZoom < 0.01f)
        {
            _targetZoom = MathF.Abs(_targetZoom);
        }

        double x = Mathf.Lerp(Zoom.X, _targetZoom * Vector2.One.X, _zoomRate * delta);
        double y = Mathf.Lerp(Zoom.Y, _targetZoom * Vector2.One.Y, _zoomRate * delta);
        Zoom = new Vector2((float)x, (float)y);

        if (_targetZoom > 0.9f)
        {
            xBounds = orgXBounds * (float)x;
            yBounds = orgYBounds * (float)y;
            if (!zoomIn)
            {
                if (!CheckInBounds(Position))
                {
                    Vector2 inBounds = InBounds(Position);
                    FocusPointDuration(inBounds, 0.02f);
                }
                else
                {
                    _tween?.Kill();
                }
            }
        }
        else
        {
            xBounds = orgXBoundsZoomOut;
            yBounds = orgYBoundsZoomOut;
            if (!zoomIn)
            {
                if (!CheckInBounds(Position))
                {
                    Vector2 inBounds = InBounds(Position);
                    FocusPointDuration(inBounds, 0.08f);
                }
                else
                {
                    _tween?.Kill();
                }
            }
        }

       

        SetPhysicsProcess(!Zoom.X.Equals(_targetZoom));
    }

    private void MoveCamera(Vector2 relativePosition, float movementSpeed)
    {
        if (Position.X <= xBounds.Y && Position.Y <= yBounds.Y && Position.X >= xBounds.X && Position.Y >= yBounds.X)
        {
            Position -= relativePosition * Zoom * movementSpeed;
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
    
    private void MoveCameraTiles(Vector2 relativePosition, float movementSpeed)
    {
        if (Position.X <= xBounds.Y && Position.Y <= yBounds.Y && Position.X >= xBounds.X && Position.Y >= yBounds.X)
        {
            Position -= relativePosition * Zoom; //* movementSpeed;
            // _tempPosition -= relativePosition * Zoom;
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
        orgXBounds = xBounds;
        orgYBounds = yBounds;
    }
    
    public void SetMovementBoundsZoomOut(Vector2 xMapBounds, Vector2 yMapBounds)
    {
        orgXBoundsZoomOut = xMapBounds;
        orgYBoundsZoomOut = yMapBounds;
    }

    private void ZoomOut(float increment)
    {
        zoomIn = false;
        _targetZoom = Math.Max(_targetZoom - zoomIncrement * increment, minZoom);
        SetPhysicsProcess(true);
    }
    
    private void ZoomIn(float increment)
    {
        zoomIn = true;
        _targetZoom = Math.Min(_targetZoom + zoomIncrement * increment, maxZoom);
        SetPhysicsProcess(true);
    }

    private void FocusPointDuration(Vector2 targetPosition, float duration = 0.2f)
    {
        if (_tween is not null)
        {
            _tween.Kill();
        }
        _tween = GetTree().CreateTween();
        _tween.TweenProperty(this, "position", InBounds(targetPosition), duration);
        _tempPosition = InBounds(targetPosition);
        _tween.Play();
    }

    public void FocusPoint(Vector2 targetPosition)
    {
        FocusPointDuration(targetPosition);
    }

    private Vector2 InBounds(Vector2 position)
    {
        Vector2 tempPosition = position;
        if(tempPosition.X > xBounds.Y)
        {
            tempPosition = new Vector2(xBounds.Y, tempPosition.Y);
        }
        if (tempPosition.Y > yBounds.Y)
        {
            tempPosition = new Vector2(tempPosition.X, yBounds.Y);
        }
        if(tempPosition.X < xBounds.X)
        {
            tempPosition = new Vector2(xBounds.X, tempPosition.Y);
        }
        if (tempPosition.Y < yBounds.X)
        {
            tempPosition = new Vector2(tempPosition.X, yBounds.X);
        }
        
        return tempPosition;
    }
    
    private bool CheckInBounds(Vector2 position)
    {
        bool inBounds = true;
        if(position.X > xBounds.Y)
        {
            inBounds = false;
        }
        if (position.Y > yBounds.Y)
        {
            inBounds = false;
        }
        if(position.X < xBounds.X)
        {
            inBounds = false;
        }
        if (position.Y < yBounds.X)
        {
            inBounds = false;
        }
        
        return inBounds;
    }
    
    protected override void Dispose(bool disposing)
    {
        InputManager.Instance.CameraControl -= MoveCamera;
        InputManager.Instance.LeftMouseDoubleClick -= FocusPoint;
        InputManager.Instance.Up -= ZoomIn;
        InputManager.Instance.Down -= ZoomOut;
        base.Dispose(disposing);
    }
}