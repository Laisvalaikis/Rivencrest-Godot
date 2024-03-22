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
            Control temp = (Control)GetNode(currentfocus.FocusNeighborBottom);
            if (temp.IsVisibleInTree())
            {
                currentfocus.ReleaseFocus();
                temp.GrabFocus();
                currentfocus = temp;
            }
        }
    }
    
    private void FocusRight()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.FocusNeighborRight != null)
        {
            Control temp = (Control)GetNode(currentfocus.FocusNeighborBottom);
            if (temp.IsVisibleInTree())
            {
                currentfocus.ReleaseFocus();
                temp.GrabFocus();
                currentfocus = temp;
            }
        }
    }
    
    private void FocusUp()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.FocusNeighborTop != null)
        {
            Control temp = (Control)GetNode(currentfocus.FocusNeighborBottom);
            if (temp.IsVisibleInTree())
            {
                currentfocus.ReleaseFocus();
                temp.GrabFocus();
                currentfocus = temp;
            }
        }
    }
    
    private void FocusDown()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.FocusNeighborBottom != null)
        {
            Control temp = (Control)GetNode(currentfocus.FocusNeighborBottom);
            if (temp.IsVisibleInTree())
            {
                currentfocus.ReleaseFocus();
                temp.GrabFocus();
                currentfocus = temp;
            }
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