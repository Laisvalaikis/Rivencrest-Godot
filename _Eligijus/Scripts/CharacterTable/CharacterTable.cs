using System;
using System.Collections;
using Godot;
using Godot.Collections;

public partial class CharacterTable : Node
{
	[Export] 
	public View view;
	[Export] 
	private TextureRect tableBoarder;
	[Export]
	private TextureRect characterArt;
	[Export]
	private Label className;
	[Export]
	private Label role;
	[Export]
	private Label maxHP;
	[Export]
	private Label level;
	[Export]
	private Label xpProgress;
	[Export]
	private Label abilityPointCount;
	[Export]
	private Label blessingList;
	[Export]
	private Control leftArrow;
	[Export]
	private Control rightArrow;
	[Export]
	private Button sellButton;
	[Export]
	private Control confirmAbility;
	public string originalName;
	[Export]
	private Array<Button> abilityButtons;
	[Export]
	private Array<TextureRect> abilityButtonBackgroundImages;
	[Export]
	private Array<TextureRect> abilityButtonIconImages;
	[Export]
	private TextureRect roleIcon;
	[Export]
	public TextEdit nameInput;
	[Export]
	public HelpTable helpTable;
	[Export]
	private Control confirmationTable;
	[Export]
	private Label confirmationTableText;
	[Export]
	private TextureRect confirmationTableSprite;
	[Export] 
	private GameUi gameUI;
	[Export]
	private PortraitBar portraitBar;
	[Export]
	public Control recruitmentCenterTable;
	[Export]
	public TownHall townHall;
	private Array<bool> abilityButtonState;
	private int characterIndex;
	private int abilityCount = 0;
	private Data _data;
	private bool pauseEnabled = false;
	
	public override void _Ready()
	{
		base._Ready();
		if (_data == null && Data.Instance != null)
		{
			_data = Data.Instance;
			if (abilityButtonState == null || abilityButtonState.Count == 0)
			{
				abilityButtonState = new Array<bool>();
				for (int i = 0; i < abilityButtons.Count; i++)
				{
					abilityButtonState.Add(false);
				}
			}
		}
	}

	public int GetCurrentCharacterIndex()
	{
		return characterIndex;
	}

	public void DisableAllButtons()
	{
		pauseEnabled = true;
		if (abilityButtonState == null)
		{
			abilityButtonState = new Array<bool>();
		}
		else if (abilityButtonState.Count > 0)
		{
			for (int i = 0; i < abilityButtons.Count; i++)
			{
				abilityButtonState[i] = abilityButtons[i].Disabled;
				abilityButtons[i].Disabled = true;
			}
		}
	}

	public void EnableAllButtons()
	{
		pauseEnabled = false;
		if (abilityButtonState.Count > 0)
		{
			for (int i = 0; i < abilityButtons.Count; i++)
			{
				abilityButtons[i].Disabled = abilityButtonState[i];
			}
		}
	}

	private void UpdateCharacterPointCorner()
	{
		Array<SavedCharacterResource> charactersToUpdate = _data.Characters;
		if (charactersToUpdate[characterIndex].abilityPointCount <= 0)
		{
			portraitBar.DisableAbilityCorner(characterIndex);
		}
		else
		{
			portraitBar.EnableAbilityCorner(characterIndex);
		}

		if (gameUI != null)
		{
			gameUI.UpdateUnspentPointWarnings();
		}
	}

	public void ConfirmSelectedAbilities()
	{
		_data.Characters[characterIndex].toConfirmAbilities = 0;
		Array<UnlockedAbilitiesResource> unlockedAbilityList = _data.Characters[characterIndex].unlockedAbilities;
		for (int i = 0; i < unlockedAbilityList.Count; i++)
		{
			if (!unlockedAbilityList[i].abilityConfirmed && unlockedAbilityList[i].abilityUnlocked)
			{
				_data.Characters[characterIndex].unlockedAbilities[i].abilityConfirmed = true;
			}
		}
		confirmAbility.Hide();
		abilityButtons[0].FocusNeighborTop = abilityButtons[abilityCount - 1].GetPath();
		abilityButtons[abilityCount - 1].FocusNeighborBottom = abilityButtons[0].GetPath();
		abilityButtons[abilityCount - 1].GrabFocus();
		UpdateCharacterPointCorner();
	}

	public void ChangeCharacterName()
	{
		nameInput.Text = nameInput.Text.ToUpper();
		_data.Characters[characterIndex].characterName = nameInput.Text;
	}

	public void PreventEmptyName()
	{
		if (nameInput.Text.Contains(' '))
		{
			_data.Characters[characterIndex].characterName = originalName;
			nameInput.Text = originalName;
		}
		if (nameInput.Text == "")
		{
			_data.Characters[characterIndex].characterName = originalName;
			nameInput.Text = originalName;
		}
	}
	
	public void UpdateConfirmationTable()
	{
		var character = _data.Characters[characterIndex];
		Color color = character.playerInformation.classColor; // fix this shit
		confirmationTableText.Text = $"Are you sure you want to sell " + character.characterName + " " + (character.cost / 2) + " gold?";
		AtlasTexture atlasTexture = NewAtlasTexture(character.playerInformation.CharacterPortraitSprite, Colors.White);
		confirmationTableSprite.Texture = atlasTexture;
	}
	
	public void UpdateAllAbilities()
	{
		var character = _data.Characters[characterIndex];
		for (int i = 0; i < abilityButtons.Count; i++)
		{
			if (abilityCount > i)
			{
				abilityButtons[i].Show();
				if (character.abilityPointCount > 0 ||
				    !_data.Characters[characterIndex].unlockedAbilities[i].abilityConfirmed &&
				    _data.Characters[characterIndex].unlockedAbilities[i].abilityUnlocked)
				{
					abilityButtons[i].Disabled = false;
				}
				else
				{
					abilityButtons[i].Disabled = true;
				}

				if (!character.unlockedAbilities[i].abilityUnlocked)
				{
					Color color = abilityButtonBackgroundImages[i].SelfModulate - new Color(0.2f, 0.2f, 0.2f, 0f);
					abilityButtonBackgroundImages[i].SelfModulate = color;
				}
				else
				{
					abilityButtonBackgroundImages[i].SelfModulate = character.playerInformation.backgroundColor;
				}
			}
			else
			{
				abilityButtons[i].Hide();
			}
		}
	}


	public void DisplayCharacterTable(int index)
	{
		view.OpenView();
		int tempIndex = characterIndex;
		characterIndex = index;
		abilityCount = _data.Characters[tempIndex].playerInformation.abilities.Count;
		if (index != tempIndex)
		{
			helpTable.helpTableView.ExitView();
			UndoAbilitySelection(tempIndex);
			UpdateTable();
			UpdateAllAbilities();
		}

		if (recruitmentCenterTable != null && townHall != null)
		{
			recruitmentCenterTable.Hide();
			townHall.CloseTownHall();
		}
		else
		{
			GD.Print("TownHall is null");
		}
		
		
	}
	
	public void DisplayCharacterTableButton(int index, Button button)
	{
		// view.OpenView();
		view.OpenViewCurrentButton(button.GetPath());
		int tempIndex = characterIndex;
		characterIndex = index;
		abilityCount = _data.Characters[tempIndex].playerInformation.abilities.Count;
		
		if (index != tempIndex)
		{
			helpTable.helpTableView.ExitView();
			UndoAbilitySelection(tempIndex);
			UpdateTable();
			UpdateAllAbilities();
		}

		if (recruitmentCenterTable != null && townHall != null)
		{
			recruitmentCenterTable.Hide();
			townHall.CloseTownHall();
		}
		else
		{
			GD.Print("TownHall is null");
		}
		
		
	}

	public void EnableDisableHelpTable(int index)
	{
		if (!pauseEnabled)
		{
			Vector2 currentPosition = helpTable.helpTableView.GlobalPosition;
			float y = abilityButtons[index].GlobalPosition.Y;
			y = y + (abilityButtons[index].Size.Y / 2f);
			y = y - (helpTable.helpTableView.Size.Y / 2f);
			Vector2 position = new Vector2(currentPosition.X, y);
			helpTable.helpTableView.SetGlobalPosition(position);
			helpTable.EnableTableForBoughtCharacters(index, characterIndex);
		}
	}
	
	public void UndoAbilitySelection(int selectedCharacterIndex)
	{
		Array<UnlockedAbilitiesResource> unlockedAbilityList = _data.Characters[selectedCharacterIndex].unlockedAbilities;
		for (int i = 0; i < unlockedAbilityList.Count; i++)
		{
			if (!unlockedAbilityList[i].abilityConfirmed && unlockedAbilityList[i].abilityUnlocked)
			{
				RemoveAbility(i, selectedCharacterIndex);
			}
		}
		UpdateTable();
		UpdateAllAbilities();
	}
	
	public void ExitTable()
	{
		UndoAbilitySelection(characterIndex);
		portraitBar.ToggleDeSelectButton(characterIndex);
		view.ExitView();
	}
	
	public void OnLeftArrowClick()
	{
		UndoAbilitySelection(characterIndex);
		portraitBar.ToggleDeSelectButton(characterIndex);
		int newCharacterIndex = Mathf.Clamp(characterIndex - 1, 0, _data.Characters.Count - 1);
		DisplayCharacterTable(newCharacterIndex);
		UpdateTable();
		UpdateAllAbilities();
		portraitBar.ToggleSelectButton(newCharacterIndex);
		portraitBar.ScrollDownByCharacterIndex(newCharacterIndex);
		view.ViewFocus(view.GetPathTo(portraitBar.GetPortraitButtonByIndex(newCharacterIndex)));
	}

	public void OnRightArrowClick()
	{
		UndoAbilitySelection(characterIndex);
		portraitBar.ToggleDeSelectButton(characterIndex);
		int newCharacterIndex = Mathf.Clamp(characterIndex + 1, 0, _data.Characters.Count - 1);
		DisplayCharacterTable(newCharacterIndex);
		UpdateTable();
		UpdateAllAbilities();
		portraitBar.ToggleSelectButton(newCharacterIndex);
		portraitBar.ScrollUpByCharacterIndex(newCharacterIndex);
		view.ViewFocus(view.GetPathTo(portraitBar.GetPortraitButtonByIndex(newCharacterIndex)));
	}

	// until this everything is fixed
	
	public void SellCharacter()
	{
		int cost = _data.Characters[characterIndex].cost;
		_data.townData.townGold += cost / 2;
		gameUI.UpdateTownCost();
		_data.Characters.RemoveAt(characterIndex);
		portraitBar.RemoveCharacter(characterIndex);
		view.ExitView();
		UpdateTable();
		if (characterIndex > _data.Characters.Count - 1)
		{
			characterIndex = _data.Characters.Count - 1;
		}
	}


	public void UpdateTable()
	{
		if (_data.Characters.Count > 0 && characterIndex < _data.Characters.Count && characterIndex >= 0)
		{
			UpdateTableInformation();
			UpdateConfirmationTable();
		}
		confirmationTable.Hide();
		if (_data.Characters[characterIndex].toConfirmAbilities > 0)
		{
			confirmAbility.Show();
			abilityButtons[0].FocusNeighborTop = confirmAbility.GetPath();
			abilityButtons[abilityCount - 1].FocusNeighborBottom = confirmAbility.GetPath();
			confirmAbility.FocusNeighborBottom = abilityButtons[0].GetPath();
			confirmAbility.FocusNeighborTop = abilityButtons[abilityCount - 1].GetPath();
		}
		else
		{
			confirmAbility.Hide();
			abilityButtons[0].FocusNeighborTop = abilityButtons[abilityCount - 1].GetPath();
			abilityButtons[abilityCount - 1].FocusNeighborBottom = abilityButtons[0].GetPath();
		}

		if (characterIndex > 0)
		{
			leftArrow.Show();
		}
		else
		{
			leftArrow.Hide();
		}

		if (characterIndex < _data.Characters.Count - 1)
		{
			rightArrow.Show();
		}
		else
		{
			rightArrow.Hide();
		}
		
		if (_data.Characters.Count > 3)
		{
			sellButton.Show();
		}
		else
		{
			sellButton.Hide();
		}
	}

	public void UpdateTableInformation()
	{
		SavedCharacterResource character = _data.Characters[characterIndex];
		originalName = character.characterName;
		PlayerInformationDataNew data = character.playerInformation;
		Color color = data.classColor;
		tableBoarder.SelfModulate = data.secondClassColor;
		nameInput.Text = character.characterName;
		className.Text = data.ClassName;
		className.LabelSettings.FontColor = color;
		role.Text = data.role;
		role.LabelSettings.FontColor = color;
		roleIcon.Texture.Set("atlas", data.roleSprite.Get("atlas"));
		level.Text = "LEVEL: " + character.level;
		level.LabelSettings.FontColor = color;
		maxHP.Text = "MAX HP: " + character.playerInformation.maxHealth; // CalculateMaxHP(character);
		maxHP.LabelSettings.FontColor = color;
		xpProgress.Text = (character.level >= GameManager.currentMaxLevel()) ? "MAX LEVEL" : character.xP + "/" + _data.XPToLevelUp[character.level - 1] + " XP";
		xpProgress.LabelSettings.FontColor = color;
		abilityPointCount.Text = character.abilityPointCount.ToString();
		abilityPointCount.LabelSettings.FontColor = color;
		characterArt.Texture.Set("region", data.CharacterSplashArt.Get("region"));
		// blessingList.Text = character.CharacterTableBlessingString();
		
		for (int i = 0; i < abilityButtonBackgroundImages.Count; i++)
		{
			abilityButtons[i].Show();
			abilityButtonIconImages[i].SelfModulate = character.playerInformation.classColor;
			if (character.playerInformation.abilities.Count > i)
			{
				AtlasTexture atlasTexture = NewAtlasTexture(character.playerInformation.abilities[i].AbilityImage,
					Colors.White);
				abilityButtonIconImages[i].Texture = atlasTexture;
			}

			abilityButtonBackgroundImages[i].SelfModulate = character.playerInformation.backgroundColor;
			
		}
	}


	// private int CalculateMaxHP(SavedCharacterResource character)
	// {
	// 	int maxHP = character.playerInformation.MaxHealth; // fix this
	// 	maxHP += (character.level - 1) * 2;
	// 	// for (int i = 0; i < character.blessings.Count; i++)
	// 	// {
	// 	// 	BaseBlessing blessing = character.blessings[i];
	// 	// 	if (blessing.blessingName == "Healthy")
	// 	// 	{
	// 	// 		maxHP += 3;
	// 	// 	}
	// 	// }
	// 	return maxHP;
	// }
	private void UpgradeAbility(int abilityIndex)
	{
		_data.Characters[characterIndex].unlockedAbilities[abilityIndex].abilityUnlocked = true;
		_data.Characters[characterIndex].abilityPointCount--;
		_data.Characters[characterIndex].toConfirmAbilities++;
		UpdateTable();
		UpdateAllAbilities();
	}
	private void RemoveAbility(int abilityIndex, int selectedCharacterIndex)
	{
		_data.Characters[selectedCharacterIndex].unlockedAbilities[abilityIndex].abilityUnlocked = false;
		_data.Characters[selectedCharacterIndex].toConfirmAbilities--;
		_data.Characters[selectedCharacterIndex].abilityPointCount++;
	}

	public void AddRemoveAbility(int index)
	{
		if (!_data.Characters[characterIndex].unlockedAbilities[index].abilityUnlocked)
		{
			UpgradeAbility(index);
		}
		else
		{
			RemoveAbility(index, characterIndex);
			UpdateTable();
			UpdateAllAbilities();
		}
	}
	
	private AtlasTexture NewAtlasTexture(Texture spriteTexture, Color pressedColor)
	{
		AtlasTexture atlas = new AtlasTexture();
		atlas.Region = (Rect2)spriteTexture.Get("region");
		atlas.Atlas = (CompressedTexture2D)spriteTexture.Get("atlas");
		
		return atlas;
	}
	
}

