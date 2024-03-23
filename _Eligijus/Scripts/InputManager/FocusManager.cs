using Godot;

public partial class FocusManager : Node
{
    public static FocusManager Instance;
    private Control currentfocus;
    private View currentView;
    public override void _Ready()
    {
        base._Ready();
        if (Instance == null)
        {
            Instance = this;
        }

        InputManager.Instance.FocusLeft += FocusLeft;
        InputManager.Instance.FocusRight += FocusRight;
        InputManager.Instance.FocusUp += FocusUp;
        InputManager.Instance.FocusDown += FocusDown;
        UIStack.Instance.CloseView += ResetFocus;
    }
    
    private void FocusLeft()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.GetNodeOrNull<Control>(currentfocus.FocusNeighborLeft) != null)
        {
            // Control temp = (Control)GetNode(currentfocus.FocusNeighborLeft);
            Control temp = currentfocus.GetNode<Control>(currentfocus.FocusNeighborLeft);
            if (temp.IsVisibleInTree())
            {
                currentfocus.ReleaseFocus();
                temp.GrabFocus();
                currentfocus = temp;
            }
            GD.Print(temp.IsVisibleInTree());
        }
    }
    
    private void FocusRight()
    {
        GetViewFocus();
        if (currentfocus != null && currentfocus.GetNodeOrNull<Control>(currentfocus.FocusNeighborRight) != null)
        {
            // Control temp = (Control)GetNode(currentfocus.FocusNeighborRight);
            Control temp = currentfocus.GetNode<Control>(currentfocus.FocusNeighborRight);
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
        if (currentfocus != null && currentfocus.GetNodeOrNull<Control>(currentfocus.FocusNeighborTop) != null)
        {
            // Control temp = (Control)GetNode(currentfocus.FocusNeighborTop);
            Control temp = currentfocus.GetNode<Control>(currentfocus.FocusNeighborTop);
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
        if (currentfocus != null && currentfocus.GetNodeOrNull<Control>(currentfocus.FocusNeighborBottom) != null)
        {
            // Control temp = (Control)GetNode(currentfocus.FocusNeighborBottom);
            Control temp = currentfocus.GetNode<Control>(currentfocus.FocusNeighborBottom);
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
        if (tempView != null)
        {

            // currentfocus = tempView.FirstFocused();
            // currentView = tempView;

            // this is for saving button part of it
            if (tempView != currentView)
            {
                currentfocus = tempView.FirstFocused();
                currentView = tempView;
            }
        }
    }

    public void ResetFocus()
    {
        currentfocus = null;
        currentView = null;
    }

    public void SetCurrentFocus(Control focus)
    {
        currentfocus = focus;
    }
}