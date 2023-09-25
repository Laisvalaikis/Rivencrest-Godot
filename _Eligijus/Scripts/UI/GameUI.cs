using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class GameUi : Node
{
	[Export] 
	private Label townGold;
	[Export] private Label dayNumber;
	[Export] private Button difficulty;
	[Export] private CanvasItem abilityPointWarning;
	[Export] private CanvasItem buyRecruitsWarning;
	[Export] private Button embark;
	private Data _data;

	public override void _Ready()
	{
		base._Ready();
		_data = Data.Instance;
		UpdateDifficultyText();
		UpdateDayNumber();
		UpdateTownCost();
		UpdateEmbarkButton();
	}
	
	public void UpdateEmbarkButton()
	{
		if (_data.Characters.Count >= _data.minCharacterCount)
		{
			embark.Disabled = false;
		}
	}

	public void UpdateTownCost()
	{
		townGold.Text = _data.townData.townGold.ToString() + "g";
	}
	
	public void UpdateDayNumber()
	{
		dayNumber.Text = "Day " + _data.townData.day.ToString();
	}
	
	public void UpdateDifficultyButton()
	{
		if (_data.townData.difficultyLevel == 0)
		{
			_data.townData.difficultyLevel = 1;
			difficulty.Text = "HARD";
		}
		else
		{
			_data.townData.difficultyLevel = 0;
			difficulty.Text = "EASY";
		}
	}
	
	private void UpdateDifficultyText()
	{
		if (_data.townData.difficultyLevel == 0)
		{
			difficulty.Text = "EASY";
		}
		else
		{
			difficulty.Text = "HARD";   
		}
	}
	public void UpdateBuyRecruitsWarning()
	{
		if (_data.Characters.Count < 3)
		{
			buyRecruitsWarning.Show();
		}
		else
		{
			buyRecruitsWarning.Hide();
		}

		
	}

	public void UpdateUnspentPointWarnings()
	{
		bool playerHasUnspentPoints = false;
		if (_data.Characters != null)
		{
			Array<SavedCharacterResource> charactersToUpdate = _data.Characters;
			for (int i = 0; i < charactersToUpdate.Count; i++)
			{
				if (charactersToUpdate[i].abilityPointCount > 0)
				{
					playerHasUnspentPoints = true;
					abilityPointWarning.Show();
					break;
				}
			}

			if (!playerHasUnspentPoints)
			{
				abilityPointWarning.Hide();
			}
		}
	}

}
