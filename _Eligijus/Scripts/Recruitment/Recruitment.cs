using System;
using System.Collections;
using Godot;
using Godot.Collections;

public partial class Recruitment : Control
{
	[Export]
	public Array<RecruitButton> recruitTableCharacters;
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
	public override void _Draw()
	{
		base._Draw();
		if (_data == null)
		{
			_data = Data.Instance;
			characterIndexList = new Array<int>();
		}
	}

	public void RecruitmentStart()
	{
		ReadString(NamesM, NamesMFile);
		ReadString(NamesW, NamesWFile);
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

	public void ReadString(Array<string> stringList, string namesFile)
	{
		stringList.Clear();
		using (var file = FileAccess.Open(namesFile, FileAccess.ModeFlags.Read))
		{
			string line;
			while ((line = file.GetLine()) != null)
			{
				stringList.Add(line);
			}
		}
	}
	private void CreateCharactersInShop()
	{
		Array<SavedCharacterResource> AllCharactersCopy = new Array<SavedCharacterResource>(_data.AllAvailableCharacters);
		CharactersInShop.Clear();
		characterIndexList.Clear();
		if(NamesW.Count <= 8)
		{
			ReadString(NamesW, NamesWFile);
		}
		if (NamesM.Count <= 8)
		{
			ReadString(NamesM, NamesMFile);
		}
		for (int i = 0; i < AttractedCharactersCount; i++)
		{
			int randomIndex = _random.Next(0, AllCharactersCopy.Count);
			SavedCharacterResource characterToAdd = new SavedCharacterResource(AllCharactersCopy[randomIndex]);
			characterToAdd.level = 1;
			characterToAdd.xP = 0;
			for (int j = AllCharactersCopy.Count; j >= 0; j--)
			{
				if (AllCharactersCopy[i].prefab == characterToAdd.prefab)
				{
					AllCharactersCopy.RemoveAt(i);
				}
			}
			Array<string> NameList;
			if (characterToAdd.playerInformation.ClassName == "ASSASSIN" ||
				characterToAdd.playerInformation.ClassName == "ENCHANTRESS" ||
				characterToAdd.playerInformation.ClassName == "SORCERESS" ||
				characterToAdd.playerInformation.ClassName == "HUNTRESS")
			{
				NameList = NamesW;
			}
			else {
				NameList = NamesM;
			}
			int randomIndex2 = _random.Next(0, NameList.Count);
			characterToAdd.characterName = NameList[randomIndex2].ToUpper();
			NameList.RemoveAt(randomIndex2);
			//
			characterToAdd.abilityPointCount = 1;
			characterToAdd.unlockedAbilities = new Array<UnlockedAbilitiesResource>();
			for (int j = 0; j < 4; j++)
			{
				characterToAdd.unlockedAbilities.Add(new UnlockedAbilitiesResource());
			}
			GD.PrintErr("Need to redo Unlocked Abilities");
			//
			if (CharacterLevelChar == 1)
			{
				characterToAdd.level = 2;
				characterToAdd.abilityPointCount = 2;
			}
			characterIndexList.Add(randomIndex);
			//
			CharactersInShop.Add(characterToAdd);
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
				gameUI.EnableGoldChange("-" + savedCharacter.cost + "g");
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
