using Godot;

public partial class EndTurnButton : Button
{
    [Export] private TurnManager _turnManager;

    public override void _Ready()
    {
        base._Ready();
        InputManager.Instance.EndTurn += _Pressed;
    }

    public override void _Pressed()
    {
        base._Pressed();
        _turnManager.EndTurn();
    }
    
    protected override void Dispose(bool disposing)
    {
        InputManager.Instance.EndTurn -= _Pressed;
        base.Dispose(disposing);
    }
}