using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class SaveSlotCard : Control
{
	[Export]
	private SaveSlotCard _prievCard;
	[Export]
	private SaveSlotCard _nexCard;
	[Export]
	private int _slotIndex;
	[Export] 
	private Button _addButton;
	[Export] 
	private SaveSlotCardButtons _slotMenu;
	[Export] 
	private SaveManager _saveManager;
	[Export] 
	private Label _slotTitle;
	// Start is called before the first frame update
	public override void _Ready()
	{
	}

	public override void _Draw()
	{
		EnableView();
	}

	public void EnableView()
	{
		bool saveExist = SaveSystem.DoesSaveFileExist(_slotIndex);
		SetActive(_addButton, !saveExist);
		SetActive(_slotMenu, saveExist);
		ManageFocus();
		if(saveExist)
		{
			_slotTitle.Text = SaveSystem.LoadTownData(_slotIndex).teamName;
		}
	}

	public void GrabSlotFocus()
	{
		if (_slotMenu.IsVisibleInTree())
		{
			_slotMenu.first.GrabFocus();
		}
		else
		{
			_addButton.GrabFocus();
		}
	}

	private void SetActive(CanvasItem node, bool active)
	{
		if (active)
		{
			node.Show();
			
		}
		else
		{
			node.Hide();
		}
	}
	
	public SaveSlotCard GetActivatedView()
	{
		return this;
	}
	
	private void ManageFocus()
	{
		if (_slotMenu.IsVisibleInTree())
		{
			FocusSlotMenu();
		}
		else
		{
			FocusAddButton();
		}
		
	}

	private void FocusSlotMenu()
	{
		if (_prievCard._slotMenu.IsVisibleInTree())
		{
			_slotMenu.first.FocusNeighborLeft = _prievCard._slotMenu.last.GetPath();
		}
		else
		{
			_slotMenu.first.FocusNeighborLeft = _prievCard._addButton.GetPath();
		}
		
		if (_nexCard._slotMenu.IsVisibleInTree())
		{
			_slotMenu.last.FocusNeighborRight = _nexCard._slotMenu.first.GetPath();
		}
		else
		{
			_slotMenu.last.FocusNeighborRight = _nexCard._addButton.GetPath();
		}
	}

	private void FocusAddButton()
	{
		if (_prievCard._slotMenu.IsVisibleInTree())
		{
			_addButton.FocusNeighborLeft = _prievCard._slotMenu.last.GetPath();
		}
		else
		{
			_addButton.FocusNeighborLeft = _prievCard._addButton.GetPath();
		}

		if (_nexCard._slotMenu.IsVisibleInTree())
		{
			_addButton.FocusNeighborRight = _nexCard._slotMenu.first.GetPath();
		}
		else
		{
			_addButton.FocusNeighborRight = _nexCard._addButton.GetPath();
		}
	}

	public void DeleteSlot()
	{
		SetActive(_addButton, true);
		SetActive(_slotMenu, false);
		_saveManager.DeleteSlot(_slotIndex);
	}
}
