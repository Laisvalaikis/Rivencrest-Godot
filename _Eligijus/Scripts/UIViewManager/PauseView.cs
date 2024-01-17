namespace Rivencrestgodot._Eligijus.Scripts.UIViewManager;

public partial class PauseView : View
{
    public override void _Ready()
    {
        base._Ready();
        InputManager.Instance.PauseMenu += OpenCloseView;
    }
}