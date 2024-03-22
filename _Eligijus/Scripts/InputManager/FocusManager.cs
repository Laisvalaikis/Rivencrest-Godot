using Godot;

public partial class FocusManager : Node
{
    private Control currentfocus;
    private View currentView;
    public override void _Ready()
    {
        base._Ready();
        InputManager.Instance.FocusLeft += FocusLeft;
        InputManager.Instance.FocusRight += FocusRight;
        InputManager.Instance.FocusUp += FocusUp;
        InputManager.Instance.FocusDown += FocusDown;
    }
    
    private void FocusLeft()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.FocusNeighborLeft != null)
        {
            currentfocus.ReleaseFocus();
            Control temp = (Control)GetNode(currentfocus.FocusNeighborLeft);
            temp.GrabFocus();
            currentfocus = temp;
        }
    }
    
    private void FocusRight()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.FocusNeighborRight != null)
        {
            currentfocus.ReleaseFocus();
            Control temp = (Control)GetNode(currentfocus.FocusNeighborRight);
            temp.GrabFocus();
            currentfocus = temp;
        }
    }
    
    private void FocusUp()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.FocusNeighborTop != null)
        {
            currentfocus.ReleaseFocus();
            Control temp = (Control)GetNode(currentfocus.FocusNeighborTop);
            temp.GrabFocus();
            currentfocus = temp;
        }
    }
    
    private void FocusDown()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.FocusNeighborBottom != null)
        {
            currentfocus.ReleaseFocus();
            Control temp = (Control)GetNode(currentfocus.FocusNeighborBottom);
            temp.GrabFocus();
            currentfocus = temp;
        }
    }

    private void GetViewFocus()
    {
        View tempView = UIStack.Instance.GetCurrentView();
        if (tempView != currentView)
        {
            currentfocus = tempView.FirstFocused();
            currentView = tempView;
        }
    }
}