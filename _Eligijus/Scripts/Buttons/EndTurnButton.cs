using Godot;

public partial class EndTurnButton : FocusButton
{
    [Export] private TurnManager _turnManager;
    public override void _Pressed()
    {
        base._Pressed();
        _turnManager.EndTurn();
    }
}