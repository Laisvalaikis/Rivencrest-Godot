using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;


public partial class SaveData : Node
{
	private Data _data;
	private Array<SavedCharacterResource> _recruitCharacters;
	private bool _allowEnemySelection = false;
	private bool _allowDuplicates = false;
	[Export]
	private Recruitment _recruitment;
	
	public override void _Ready()
	{
		base._Ready();
		if (_data == null)
		{
			_data = Data.Instance;
			LoadData();
		}
	}

	//SAVESYSTEM
	public void SaveGameData()
	{
		_data.selectedEnemies = new Array<int>();
		List<SavedCharacter> savedCharacters = new List<SavedCharacter>();
		for (int i = 0; i < _data.Characters.Count; i++)
		{
			savedCharacters.Add(new SavedCharacter(_data.Characters[i], _data.Characters[i].prefab, null));
		}
		
		List<SavedCharacter> rcCharacters = new List<SavedCharacter>();
		for (int i = 0; i < _recruitCharacters.Count; i++)
		{
			rcCharacters.Add(new SavedCharacter(_recruitCharacters[i], _recruitCharacters[i].prefab, null));
		}

		TownData data = new TownData(_data.townData);
		data.characters = new List<SavableCharacter>(savedCharacters);
		if(rcCharacters != null)
			data.rcCharacters = new List<SavableCharacter>(rcCharacters);
		data.createNewRCcharacters = rcCharacters == null;
		SaveSystem.SaveTownData(data);
		SaveSystem.SaveStatistics(_data.statistics);
		SaveSystem.SaveStatistics(_data.globalStatistics, true);
	}

	public void SaveTownData()
	{
		_allowEnemySelection = false;
		_allowDuplicates = false;
		_recruitCharacters = _recruitment.CharactersInShop;
		SaveGameData();
	}

	public void SaveSelectedCharacterData()
	{
		SetupRecruitmentCharacters();
		SaveGameData();
	}
	

	public void SetupRecruitmentCharacters()
	{
		if (SaveSystem.DoesSaveFileExist() && !_data.createNewRCcharacters)
		{
			TownDataResource townData = _data.townData;
			for (int i = 0; i < townData.rcCharacters.Count; i++)
			{
				SavableCharacterResource savableCharacter = townData.rcCharacters[i];
				_recruitCharacters.Add(new SavedCharacterResource(savableCharacter, _data.AllAvailableCharacters[savableCharacter.characterIndex].prefab, _data.AllAvailableCharacters[savableCharacter.characterIndex].playerInformation));
			}
		}
		if (_data.createNewRCcharacters)
		{
			_recruitCharacters = null;
		}
	}

	public void OtherSaveData()
	{
		_allowEnemySelection = false;
		_allowDuplicates = false;
		SaveGameData();
	}
	
	public void LoadData()
	{
		_data.townData = new TownDataResource(SaveSystem.LoadTownData());
		if (_data.Characters != null)
		{
			_data.Characters.Clear();
		}
		else
		{
			_data.Characters = new Array<SavedCharacterResource>();
		}
		
		if (_data.CharactersOnLastMission != null)
		{
			_data.CharactersOnLastMission.Clear();
		}
		else
		{
			_data.CharactersOnLastMission = new Array<int>();
		}
		
		for (int i = 0; i < _data.townData.characters.Count; i++)
		{
			SavableCharacterResource savableCharacter = _data.townData.characters[i];
			_data.Characters.Add(new SavedCharacterResource(savableCharacter,
				_data.AllAvailableCharacters[savableCharacter.characterIndex].prefab,
				_data.AllAvailableCharacters[savableCharacter.characterIndex].playerInformation));
		}
		_data.CharactersOnLastMission = new Array<int>(_data.townData.charactersOnLastMission);
		_data.globalStatistics = SaveSystem.LoadStatistics(true);
		_data.statistics = SaveSystem.LoadStatistics();
	}

}
