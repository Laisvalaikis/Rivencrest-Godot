using Godot;

public partial class FocusButton : Button
{
    public override void _Ready()
    {
        base._Ready();
        if (InputManager.Instance != null)
        {
            InputManager.Instance.ReleaseFocus += ReleaseButtonFocus;
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
    }
}