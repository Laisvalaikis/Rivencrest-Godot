using System;
using System.Collections;
using System.Collections.Generic;
using Godot;
using Godot.Collections;


public partial class SaveData : Control
{
    private Data _data;
    private Array<SavedCharacterResource> _recruitCharacters;
    private bool _allowEnemySelection = false;
    private bool _allowDuplicates = false;

    public override void _Draw()
    {
        base._Draw();
        if (_data == null)
        {
            _data = Data.Instance;
            LoadData();
        }
    }

    //SAVESYSTEM
    public void SaveGameData()
    {
        TownData townData = new TownData(_data.townData);
        _data.selectedEnemies = new Array<int>();
        List<SavedCharacter> savedCharacters = new List<SavedCharacter>();
        for (int i = 0; i < _data.Characters.Count; i++)
        {
            savedCharacters.Add(new SavedCharacter(_data.Characters[i], _data.Characters[i].prefab));
        }
        
        List<SavedCharacter> rcCharacters = new List<SavedCharacter>();
        for (int i = 0; i < _recruitCharacters.Count; i++)
        {
            savedCharacters.Add(new SavedCharacter(_recruitCharacters[i]));
        }

        TownData data = new TownData(townData.difficultyLevel, townData.townGold, townData.day, savedCharacters, new List<int>(_data.CharactersOnLastMission),
            townData.wasLastMissionSuccessful, false, townData.singlePlayer, townData.selectedMission, townData.townHall, rcCharacters,
            new List<int>(_data.selectedEnemies), _allowEnemySelection, _allowDuplicates, SaveSystem.LoadTownData().teamColor,
            townData.slotName, townData.selectedEncounter, townData.pastEncounters, townData.generateNewEncounters, townData.generatedEncounters, townData.gameSettings);
        SaveSystem.SaveTownData(data);
        SaveSystem.SaveStatistics(_data.statistics);
        SaveSystem.SaveStatistics(_data.globalStatistics, true);
    }

    public void SaveTownData(Recruitment recruitment)
    {
        _allowEnemySelection = false;
        _allowDuplicates = false;
        _recruitCharacters = recruitment.CharactersInShop;
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
        _data.Characters.Clear();
        _data.CharactersOnLastMission.Clear();
        List<SavedCharacterResource> savableCharacterResources = new List<SavedCharacterResource>();
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
