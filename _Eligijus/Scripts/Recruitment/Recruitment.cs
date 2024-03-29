using System;
using System.Collections;
using Godot;
using Godot.Collections;

public partial class Recruitment : Node
{
	[Export]
	public Array<RecruitButton> recruitTableCharacters;
	[Export]
	public Array<Button> iconButtons;
	[Export]
	public Array<SavedCharacterResource> CharactersInShop = null;
	[Export]
	public int AttractedCharactersCount;
	[Export]
	public Button reRollButton;
	[Export]
	public string NamesMFile;
	[Export]
	public string NamesWFile;
	[Export] 
	private PortraitBar portraitBar;
	[Export]
	private GameUi gameUI;
	private Data _data;
	private int CharacterLevelChar = 0;
	private Array<string> NamesM = new Array<string>();
	private Array<string> NamesW = new Array<string>();
	private Array<int> characterIndexList;
	private Random _random;
	private int selectedCharacterIndex = -1;
	public override void _Ready()
	{
		base._Ready();
		if (_data == null)
		{
			_data = Data.Instance;
			characterIndexList = new Array<int>();
		}

		_random = new Random();
	}

	public void SetSelectedCharacterIndex(int index)
	{
		selectedCharacterIndex = index;
	}

	public void DeSelectCurrentButton()
	{
		iconButtons[selectedCharacterIndex].ButtonPressed = false;
	}

	public void RecruitmentStart()
	{
		ReadString(ref NamesM, NamesMFile);
		ReadString(ref NamesW, NamesWFile);
		if (_data.townData.day > 1)
		{
			AttractedCharactersCount = 2;
			int townHallChar = _data.townData.townHall.attractedCharactersCount;
			CharacterLevelChar = _data.townData.townHall.attractedCharacterLevel;
			if (townHallChar == 1)
			{
				AttractedCharactersCount = 3;
			}
			else if(townHallChar == 2)
			{
				AttractedCharactersCount = 4;
			}
			else if (townHallChar == 3)
			{
				AttractedCharactersCount = 5;
			}
		}
		if(CharactersInShop == null || CharactersInShop.Count == 0)
		{
			CharactersInShop = new Array<SavedCharacterResource>();
			CreateCharactersInShop();
		}
		UpdateButtons();
	}

	public void ReadString(ref Array<string> stringList, string namesFile)
	{
		stringList = new Array<string>();
		using (var file = FileAccess.Open(namesFile, FileAccess.ModeFlags.Read))
		{
			while (!file.EofReached())
			{
				stringList.Add(file.GetLine());
			}
		}
	}
	private void CreateCharactersInShop()
	{
		GD.PrintErr("Need to refactor this");
		Array<SavedCharacterResource> AllCharactersCopy = new Array<SavedCharacterResource>(_data.AllAvailableCharacters);
		CharactersInShop.Clear();
		characterIndexList.Clear();
		for (int i = 0; i < AttractedCharactersCount && AllCharactersCopy.Count > 0; i++)
		{
			int randomIndex = _random.Next(0, AllCharactersCopy.Count);
			SavedCharacterResource characterToAdd = new SavedCharacterResource(AllCharactersCopy[randomIndex]);
			characterToAdd.level = 1;
			characterToAdd.xP = 0;
			// Generate only unique
			for (int j = AllCharactersCopy.Count-1; j >= 0; j--)
			{
				if (AllCharactersCopy[j].prefab.ResourcePath == characterToAdd.prefab.ResourcePath)
				{
					AllCharactersCopy.RemoveAt(j);
				}
			}
			characterToAdd.characterName = CharacterNameGeneration(characterToAdd.playerInformation.ClassName);;
			characterToAdd.abilityPointCount = 1;
			characterToAdd.unlockedAbilities = new Array<UnlockedAbilitiesResource>();
			GenerateBaseAbilities(characterToAdd.playerInformation.baseAbilities, characterToAdd.abilityBlessings);
			GenerateAbilities(characterToAdd.playerInformation.abilities, characterToAdd.unlockedAbilities,
				characterToAdd.abilityBlessings);
			Array<PlayerBlessing> playerBlessings = characterToAdd.playerInformation.GetAllPlayerBlessings();
			if (characterToAdd.characterBlessings == null)
			{
				characterToAdd.characterBlessings = new Array<UnlockedBlessingsResource>();
			}

			for (int j = 0; j < playerBlessings.Count; j++)
			{
				characterToAdd.characterBlessings.Add(new UnlockedBlessingsResource(playerBlessings[j]));
			}
			if (CharacterLevelChar == 1)
			{
				characterToAdd.level = 2;
				characterToAdd.abilityPointCount = 2;
			}
			characterIndexList.Add(randomIndex);
			CharactersInShop.Add(characterToAdd);
		}
	}

	private String CharacterNameGeneration(String className)
	{
		Array<string> NameList;
		if (className == "ASSASSIN" || className == "ENCHANTRESS" ||
			className == "SORCERESS" || className == "HUNTRESS")
		{
			NameList = new Array<string>(NamesW);
		}
		else {
			NameList = new Array<string>(NamesM);
		}
		int index = _random.Next(0, NameList.Count);
		return NameList[index].ToUpper();
	}

	private void GenerateBaseAbilities(Array<Ability> baseAbilities, Array<AbilityBlessingsResource> playerAbilityBlessings)
	{
		int abilityIndex = 0;
		for (int j = 0; j < baseAbilities.Count; j++)
		{
			playerAbilityBlessings.Add(new AbilityBlessingsResource());
			Array<AbilityBlessing> abilityBlessings = baseAbilities[j].Action.GetAllBlessings();
			if (abilityBlessings != null)
			{
				for (int k = 0; k < abilityBlessings.Count; k++)
				{
					playerAbilityBlessings[abilityIndex].UnlockedBlessingsList.Add(new UnlockedBlessingsResource(abilityBlessings[k]));
				}
			}
			abilityIndex++;
		}
	}
	
	private void GenerateAbilities(Array<Ability> abilities, Array<UnlockedAbilitiesResource> unlockedAbilities, Array<AbilityBlessingsResource> playerAbilityBlessings)
	{
		int abilityIndex = playerAbilityBlessings.Count;
		for (int j = 0; j < _data.townData.maxAbilityCount; j++)
		{
			unlockedAbilities.Add(new UnlockedAbilitiesResource());
			playerAbilityBlessings.Add(new AbilityBlessingsResource());
			if (abilities != null && abilities.Count > j)
			{
				Array<AbilityBlessing> abilityBlessings = abilities[j].Action.GetAllBlessings();
				if (abilityBlessings != null)
				{
					for (int k = 0; k < abilityBlessings.Count; k++)
					{
						playerAbilityBlessings[abilityIndex].UnlockedBlessingsList
							.Add(new UnlockedBlessingsResource(abilityBlessings[k]));
					}
				}
				abilityIndex++;
			}
		}
	}

	public void UpdateButtons()
	{
		UpdateRerollButton();
		for (int i = 0; i < recruitTableCharacters.Count; i++)
		{
			if (i < CharactersInShop.Count)
			{
				recruitTableCharacters[i].character = CharactersInShop[i];
				recruitTableCharacters[i].Show();
				recruitTableCharacters[i].UpdateRecruitButton();
			}
			else
			{
				recruitTableCharacters[i].character = null;
				recruitTableCharacters[i].Hide();
			}
			
		}
	}
	
	private void GrabButtonFocusIndex(int index)
	{
		
		if (recruitTableCharacters[index].IsVisibleInTree())
		{
			recruitTableCharacters[index].GrabFocusBuyButton();
		}
		else if (recruitTableCharacters[0].IsVisibleInTree())
		{
			recruitTableCharacters[0].GrabFocusBuyButton();
		}
	}
	
	private void UpdateRerollButton()
	{
		if (_data.townData.townHall != null)
		{
			int townHallChar = _data.townData.townHall.characterReRoll;
			if (townHallChar == 1)
			{
				reRollButton.Show();
			}
			else
			{
				reRollButton.Hide();
			}
		}
	}
	
	public void BuyCharacter(int index)
	{
		if (_data.Characters.Count < _data.maxCharacterCount)
		{
			SavedCharacterResource savedCharacter = recruitTableCharacters[index].character;
			if (_data.canButtonsBeClicked)
			{
				
				_data.InsertCharacter(savedCharacter);
				_data.townData.townGold -= savedCharacter.cost;
				gameUI.UpdateTownCost();
				portraitBar.InsertCharacter();
				gameUI.UpdateUnspentPointWarnings();
				gameUI.UpdateBuyRecruitsWarning();
				_data.statistics.charactersBoughtCountByClass[Statistics.GetClassIndex(savedCharacter.playerInformation.ClassName)]++;
				_data.globalStatistics.charactersBoughtCountByClass[Statistics.GetClassIndex(savedCharacter.playerInformation.ClassName)]++;
			
			}

			gameUI.UpdateEmbarkButton();

			CharactersInShop.Remove(savedCharacter);
			characterIndexList.Remove(index);
			UpdateButtons();
			GrabButtonFocusIndex(index);
		}
		else GD.Print("Ziurek ka darai, kvaily!");
	}

	public int GetRealCharacterIndex(int index)
	{
		return characterIndexList[index];
	}

	public void Reroll()
	{
		AttractedCharactersCount = CharactersInShop.Count;
		CreateCharactersInShop();
		UpdateButtons();
	}

	public SavedCharacterResource GetInShopCharacterByIndex(int index)
	{
		return CharactersInShop[index];
	}
	
}

