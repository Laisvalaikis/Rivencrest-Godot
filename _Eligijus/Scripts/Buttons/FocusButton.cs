using Godot;

public partial class FocusButton : Button
{
    [Signal] public delegate void HasFocusEventHandler(bool hasFocus);
    public override void _Ready()
    {
        base._Ready();
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ReleaseFocus += ReleaseButtonFocus;
            FocusEntered += InputManager.Instance.FocusEnter;
            FocusExited += InputManager.Instance.FocusExit;
        }
    }
    
    private void ReleaseButtonFocus() 
    {
        ReleaseFocus();
    }

    public override void _Pressed()
    {
        base._Pressed();
        if (InputSelectManager.CurrentInputScheme == InputScheme.KeyboardAndMouse)
        {
            ReleaseButtonFocus();
        }
        EmitSignal("HasFocus", true);
    }
    
    protected override void Dispose(bool disposing)
    {
        InputManager.Instance.ReleaseFocus -= ReleaseButtonFocus;
        FocusEntered -= InputManager.Instance.FocusEnter;
        FocusExited -= InputManager.Instance.FocusExit;
        base.Dispose(disposing);
    }
    

}