using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.Buttons;

public partial class UndoButton : FocusButton
{
    public override void _Ready()
    {
        base._Ready();
        InputManager.Instance.Undo += _Pressed;
    }

    public override void _Pressed()
    {
        base._Pressed();
    }
}