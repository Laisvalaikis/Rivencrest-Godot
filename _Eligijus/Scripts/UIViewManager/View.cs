using Godot;
using System;

public partial class View : Control
{
	[Export]
	private bool viewCanBeDisabled {get; set;} = true;
	[Export]
	public bool addViewToStack {get; set;} = true;
	private int _viewIndex = -1;
	private bool _disabled = true;
	public override void _Ready()
	{
		OpenView();
	}
	
	public void OpenView()
	{
		if (!IsVisibleInTree())
		{
			Show();
			// gameObject.SetActive(true);
		}
		if (_viewIndex == -1 && addViewToStack)
		{
			_viewIndex = UIStack.Instance.AddView(this);
		}
		_disabled = false;
	// 	openView.Invoke();
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
		// closeView.Invoke();
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
	
}



