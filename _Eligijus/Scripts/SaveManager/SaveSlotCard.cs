using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class SaveSlotCard : Control
{
	[Export]
	private int slotIndex;
	[Export] 
	private CanvasItem addButton;
	[Export] 
	private CanvasItem slotMenu;
	[Export] 
	private Label slotTitle;
	// Start is called before the first frame update
	public override void _Ready()
	{
		
	}

	public override void _Draw()
	{
		bool saveExist = SaveSystem.DoesSaveFileExist(slotIndex);
		SetActive(addButton, !saveExist);
		SetActive(slotMenu, saveExist);
		if(saveExist)
		{
			slotTitle.Text = SaveSystem.LoadTownData(slotIndex).slotName;
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
		SetActive(addButton, true);
		SetActive(slotMenu, false);
	}
}
