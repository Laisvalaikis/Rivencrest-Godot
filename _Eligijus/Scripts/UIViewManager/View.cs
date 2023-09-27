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
	private int buttonIndex = -1;
	public override void _Ready()
	{
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
	}
	
	public void UpdateIndex(int index)
	{
		_viewIndex = index;
	}
	
	public void ExitView()
	{
		if (!_disabled)
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

