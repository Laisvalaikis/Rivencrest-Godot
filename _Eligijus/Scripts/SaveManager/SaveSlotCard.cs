using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class SaveSlotCard : Control
{
	[Export]
	private int _slotIndex;
	[Export] 
	private Button _addButton;
	[Export] 
	private CanvasItem _slotMenu;
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
		bool saveExist = SaveSystem.DoesSaveFileExist(_slotIndex);
		SetActive(_addButton, !saveExist);
		SetActive(_slotMenu, saveExist);
		if(saveExist)
		{
			_slotTitle.Text = SaveSystem.LoadTownData(_slotIndex).slotName;
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

	public void DeleteSlot()
	{
		SetActive(_addButton, true);
		SetActive(_slotMenu, false);
		_saveManager.DeleteSlot(_slotIndex);
	}
}
