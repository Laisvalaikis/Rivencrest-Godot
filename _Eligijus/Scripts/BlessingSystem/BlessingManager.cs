using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class BlessingManager : Control
{
    [Export] private Array<BlessingCard> _blessingCards;
    [Export] private SaveData saveData;
    [Export] private ChangeScene changeScene;
    private Array<BlessingData> _playerBlessings;
    private Array<BlessingData> _abilityBlessings;
    private Array<BlessingData> _globalBlessings;
    private Data _data;
    private Array<BlessingData> _generatedBlessings;
    private Random _random;

    public override void _EnterTree()
    {
        base._EnterTree();
        if (Data.Instance != null)
        {
            _data = Data.Instance;
            SetupBlessingLists();
            _random = new Random();
        }
    }

    public override void _Draw()
    {
        base._Draw();
        ShowBlessings();
    }

    public void SetupBlessingLists()
    {
        if (_data.Characters != null)
        {
            _playerBlessings = new Array<BlessingData>();
            _abilityBlessings = new Array<BlessingData>();
            _globalBlessings = new Array<BlessingData>();
            for (int i = 0; i < _data.Characters.Count; i++)
            {
                PlayerInformationData playerInformationData = _data.Characters[i].playerInformation;
                SetupPlayerBlessings(playerInformationData.GetAllBlessings(), _data.Characters[i]);
                SetupAbilityBlessings(playerInformationData.baseAbilities, playerInformationData.abilities, _data.Characters[i]);
                SetupGlobalBlessings(_data.Characters[i]);
            }
        }

        if (_data.townData.deadCharacters != null)
        {
            for (int i = 0; i < _data.townData.deadCharacters.Count; i++)
            {
                SetupGlobalBlessingsWithDeath(_data.townData.deadCharacters[i]);
            }
        }
    }

    private void SetupAbilityBlessings(Array<Ability> baseAbility, Array<Ability> ability, SavedCharacterResource character) // patikrinti ar ability yra unlocked
    {
        Array<Ability> abilities = new Array<Ability>();
        abilities.AddRange(baseAbility);
        abilities.AddRange(ability);
    
            if (_abilityBlessings == null)
            {
                _abilityBlessings = new Array<BlessingData>();
            }

            Array<UnlockedAbilitiesResource> unlockedAbilitiesList = character.unlockedAbilities;
            Array<AbilityBlessingsResource> abilityBlessingsResources = character.abilityBlessings;
            for (int i = 0; i < abilities.Count; i++)
            {
                if (i < baseAbility.Count)
                {
                    if (abilities[i].Action.GetAllBlessings() != null)
                    {
                        Array<AbilityBlessing> abilityBlessings = abilities[i].Action.GetAllBlessings();
                        for (int blessingIndex = 0; blessingIndex < abilityBlessings.Count; blessingIndex++)
                        {
                            if (!abilityBlessingsResources[i].UnlockedBlessingsList[blessingIndex].blessingUnlocked)
                            {
                                BlessingData blessingData =
                                    new BlessingData(character, abilityBlessings[blessingIndex],
                                        abilityBlessingsResources[i].UnlockedBlessingsList[blessingIndex]);
                                blessingData.SetupAbilityBlessing();
                                _abilityBlessings.Add(blessingData);
                            }
                        }
                    }
                }
                else
                {
                    int unlockedAbilityIndex = i - baseAbility.Count;
                    if (abilities[i].Action.GetAllBlessings() != null && unlockedAbilitiesList[unlockedAbilityIndex].abilityUnlocked && unlockedAbilitiesList[unlockedAbilityIndex].abilityConfirmed)
                    {
                        Array<AbilityBlessing> abilityBlessings = abilities[i].Action.GetAllBlessings();
                        for (int blessingIndex = 0; blessingIndex < abilityBlessings.Count; blessingIndex++)
                        {
                            if (!abilityBlessingsResources[i].UnlockedBlessingsList[blessingIndex].blessingUnlocked)
                            {
                                BlessingData blessingData =
                                    new BlessingData(character, abilityBlessings[blessingIndex],
                                        abilityBlessingsResources[i].UnlockedBlessingsList[blessingIndex]);
                                blessingData.SetupAbilityBlessing();
                                _abilityBlessings.Add(blessingData);
                            }
                        }
                    }
                }
                
            }

    }
    
    private void SetupPlayerBlessings(Array<PlayerBlessing> playerBlessings, SavedCharacterResource character)
    {
        if (playerBlessings != null)
        {
            if (_playerBlessings == null)
            {
                _playerBlessings = new Array<BlessingData>();
            }

            Array<UnlockedBlessingsResource> lockUnlockPlayerBlessings = character.characterBlessings;
            for (int i = 0; i < playerBlessings.Count; i++)
            {
                if (!lockUnlockPlayerBlessings[i].blessingUnlocked)
                {
                    BlessingData blessingData =
                        new BlessingData(character, playerBlessings[i], lockUnlockPlayerBlessings[i]);
                    blessingData.SetupPlayerBlessing();
                    _playerBlessings.Add(blessingData);
                }
            }
        }
    }

    private void SetupGlobalBlessings(SavableCharacterResource character)
    {
        if (_globalBlessings == null)
        {
            _globalBlessings = new Array<BlessingData>();
        }

        if (_data.globalBlessings != null)
        {
            Array<GlobalBlessing> globalBlessings = _data.globalBlessings;
            for (int i = 0; i < globalBlessings.Count; i++)
            {
                BlessingData blessingData =
                    new BlessingData(character, globalBlessings[i], null);
                blessingData.SetupGlobalBlessing();
                _globalBlessings.Add(blessingData);
            }
        }

    }
    private void SetupGlobalBlessingsWithDeath(SavableCharacterResource character)
    {
        if (_globalBlessings == null)
        {
            _globalBlessings = new Array<BlessingData>();
        }
        if (_data.deathGlobalBlessings != null)
        {
            Array<GlobalBlessing> globalBlessings = _data.deathGlobalBlessings;
            for (int i = 0; i < globalBlessings.Count; i++)
            {
                BlessingData blessingData =
                    new BlessingData(character, globalBlessings[i], null);
                blessingData.SetupGlobalBlessing();
                _globalBlessings.Add(blessingData);
            }
        }

    }
    

    private void GenerateBlessingList()
    {
        Array<BlessingData> playerBlessingsTemp = new Array<BlessingData>(_playerBlessings);
        Array<BlessingData> abilityBlessingsTemp = new Array<BlessingData>(_abilityBlessings);
        Array<BlessingData> globalBlessings = new Array<BlessingData>(_globalBlessings);
        if (_generatedBlessings == null)
        {
            _generatedBlessings = new Array<BlessingData>();
        }
        
        for (int i = 0; i < _blessingCards.Count; i++)
        {
            int maxRange = abilityBlessingsTemp.Count + playerBlessingsTemp.Count + globalBlessings.Count;
            int index = _random.Next(0, maxRange);
            
            if (index < playerBlessingsTemp.Count)
            {
                if (!_generatedBlessings.Contains(playerBlessingsTemp[index]))
                {
                    _generatedBlessings.Add(playerBlessingsTemp[index]);
                    playerBlessingsTemp.RemoveAt(index);
                }
            }
            else if (index >= playerBlessingsTemp.Count && index < playerBlessingsTemp.Count + abilityBlessingsTemp.Count)
            {
                int correctIndex = index - playerBlessingsTemp.Count;
                if (!_generatedBlessings.Contains(abilityBlessingsTemp[correctIndex]))
                {
                    _generatedBlessings.Add(abilityBlessingsTemp[correctIndex]);
                    abilityBlessingsTemp.RemoveAt(correctIndex);
                }
            }
            else if(index >= playerBlessingsTemp.Count + abilityBlessingsTemp.Count && index < playerBlessingsTemp.Count + abilityBlessingsTemp.Count + globalBlessings.Count)
            {
                int correctIndex = index - (playerBlessingsTemp.Count + abilityBlessingsTemp.Count);
                if (!_generatedBlessings.Contains(globalBlessings[correctIndex]))
                {
                    _generatedBlessings.Add(globalBlessings[correctIndex]);
                    globalBlessings.RemoveAt(correctIndex);
                }
            }
        }

    }

    private void ShowBlessings()
    {
        if (_abilityBlessings.Count == 0 && _playerBlessings.Count == 0 && _globalBlessings.Count == 0)
        {
            NoBlessingsWasGenerated();
            return;
        }

        GenerateBlessingList();
        for (int i = 0; i < _blessingCards.Count; i++)
        {
            if (i < _generatedBlessings.Count)
            {
                _blessingCards[i].UpdateInformation(_generatedBlessings[i], this);
            }
            else
            {
                _blessingCards[i].Hide();
            }

            
        }
    }
    
    public void BlessingFinished()
    {
        _data.townData.deadCharacters.Clear();
    }

    public void NoBlessingsWasGenerated()
    {
        _data.townData.deadCharacters.Clear();
        saveData.SaveGameData();
        changeScene.SceneTransition();
    }
} 