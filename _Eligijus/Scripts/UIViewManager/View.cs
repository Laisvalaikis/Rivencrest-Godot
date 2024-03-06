using Godot;
using System;
using Rivencrestgodot._Eligijus.Scripts.Buttons;

public partial class View : Control
{
	[Signal]
	public delegate void OpenViewSignalEventHandler();
	[Signal]
	public delegate void CloseViewSignalEventHandler();
	[Export] private Control openFocus;
	[Export] private Control openFocusCurrent;
	[Export] private bool grabFocusOnOpen { get; set; } = false;
	[Export] private bool viewEnabledOnStart { get; set; } = false;
	[Export] private bool viewCanBeDisabled {get; set;} = true;
	[Export] public bool removeViewFromStack {get; set;} = true;
	[Export] public bool addViewToStack {get; set;} = true;
	private int _viewIndex = -1;
	private bool _disabled = true;
	private int buttonIndex = -1;
	private View previousView;
	public override void _Ready()
	{
		if (viewEnabledOnStart)
		{
			OpenView();
		}
		if (IsVisibleInTree() && grabFocusOnOpen)
		{
			GrabOpenFocus();
		}
	}
	
	private void GrabOpenFocus()
	{
		if (openFocusCurrent != null)
		{
			openFocusCurrent.GrabFocus();
		}
		else
		{
			openFocus?.GrabFocus();
		}
	}

	public void OpenViewCurrentButton(NodePath pressedButton)
	{
		if (previousView is null)
		{
			previousView = UIStack.Instance.GetCurrentView();
			previousView.openFocusCurrent = (Button)GetNode(pressedButton);
		}
		else
		{
			previousView.openFocusCurrent = (Button)GetNode(pressedButton);
		}
		
		
		if (!IsVisibleInTree())
		{
			Show();
			GrabOpenFocus();
		}
		
		if (_viewIndex == -1 && addViewToStack)
		{
			_viewIndex = UIStack.Instance.AddView(this);
		}
		_disabled = false;
		EmitSignal("OpenViewSignal");
	}

	public void OpenView()
	{
		if (!IsVisibleInTree())
		{
			Show();
			GrabOpenFocus();
		}
		
		if (_viewIndex == -1 && addViewToStack)
		{
			_viewIndex = UIStack.Instance.AddView(this);
		}
		_disabled = false;
		EmitSignal("OpenViewSignal");
	}

	public void GrabFocusStack()
	{
		if (IsVisibleInTree() && grabFocusOnOpen)
		{
			GrabOpenFocus();
		}
	}
	
	public void UpdateIndex(int index)
	{
		_viewIndex = index;
	}
	
	public void ExitView()
	{
		if (!_disabled)
		{
			openFocusCurrent = null;
			previousView = null;
			if (viewCanBeDisabled)
			{
				Hide();
			}

			if (addViewToStack)
			{
				UIStack.Quit(_viewIndex);
			}

			_disabled = true;
			EmitSignal("CloseViewSignal");
			_viewIndex = -1;
		}
	}
	
	public void ToggleView(bool toggle)
	{
		if (toggle)
		{
			OpenView();
		}
		else
		{
			ExitView();
		}
		GD.Print(toggle);
	}
	
	public void OpenCloseView()
	{
		if (_disabled)
		{
			OpenView();
		}
		else
		{
			ExitView();
		}
	}
	
	public void OpenCloseView(int buttonIndex)
	{
		if (this.buttonIndex == -1)
		{
			this.buttonIndex = buttonIndex;
		}

		if (this.buttonIndex == buttonIndex)
		{
			if (_disabled)
			{
				OpenView();
			}
			else
			{
				ExitView();
			}
		}
		else
		{
			OpenView();
			this.buttonIndex = buttonIndex;
		}
	}
	
	public void ExitViewWithoutRemoveFromStack()
	{
		if (viewCanBeDisabled)
		{
			Hide();
		}

		_disabled = true;
	}

}
