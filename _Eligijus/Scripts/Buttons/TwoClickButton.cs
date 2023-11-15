using Godot;

public partial class TwoClickButton : Button
{
    [Signal]
    public delegate void PressedFirstTimeEventHandler();
    [Signal]
    public delegate void PressedSecondTimeEventHandler();
    
    private int timesPressed = 0;
    public override void _Pressed()
    {
        base._Pressed();
        if (timesPressed < 1)
        {
            EmitSignal("PressedFirstTime");
            timesPressed++;
        }
        else
        {
            EmitSignal("PressedSecondTime");
            timesPressed = 0;
        }
    }
}