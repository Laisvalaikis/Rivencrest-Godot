using Godot;
using System;

public partial class View : Control
{
	[Signal]
	public delegate void OpenViewSignalEventHandler();
	[Signal]
	public delegate void CloseViewSignalEventHandler();
	[Export] 
	private bool viewEnabledOnStart { get; set; } = false;
	[Export]
	private bool viewCanBeDisabled {get; set;} = true;
	[Export]
	public bool addViewToStack {get; set;} = true;
	private int _viewIndex = -1;
	private bool _disabled = true;
	public override void _Ready()
	{
		if (this.GetThemeStylebox("panel") != null)
		{
			GD.PrintErr("Not Null");
		}
		if (viewEnabledOnStart)
		{
			OpenView();
		}
	}
	
	public void OpenView()
	{
		if (!IsVisibleInTree())
		{
			Show();
		}
		if (_viewIndex == -1 && addViewToStack)
		{
			_viewIndex = UIStack.Instance.AddView(this);
		}
		_disabled = false;
		EmitSignal("OpenViewSignal");
		GD.Print(Name);
	}
	
	public void UpdateIndex(int index)
	{
		_viewIndex = index;
	}
	
	public void ExitView()
	{
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
	
	public void ExitViewWithoutRemoveFromStack()
	{
		if (viewCanBeDisabled)
		{
			Hide();
		}

		_disabled = true;
	}
	
	public void ButtonPressed()
	{
		OpenView();
	}

}
