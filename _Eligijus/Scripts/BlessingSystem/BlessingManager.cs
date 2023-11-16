using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Array = System.Array;

public partial class BlessingManager : Node
{
    [Export] private Array<BlessingCard> _blessingCards;
    private Array<BlessingData> _playerBlessings;
    private Array<BlessingData> _abilityBlessings;
    
    private Array<BlessingData> _playerBlessingsTemp;
    private Array<BlessingData> _abilityBlessingsTemp;
    
    private Data _data;
    private Array<BlessingData> _generatedBlessings;
    private Random _random;
    public override void _Ready()
    {
        base._Ready();
        if (Data.Instance != null)
        {
            _data = Data.Instance;
            SetupBlessingLists();
            _random = new Random();
        }
    }

    public void SetupBlessingLists()
    {
        if (_data.Characters != null)
        {
            for (int i = 0; i < _data.Characters.Count; i++)
            {
                PlayerInformationData playerInformationData = _data.Characters[i].playerInformation;
                SetupPlayerBlessings(playerInformationData.GetAllBlessings(), _data.Characters[i]);
                SetupBaseAbilityBlessings(playerInformationData.baseAbilities, _data.Characters[i]);
                SetupAbilityBlessings(playerInformationData.abilities, _data.Characters[i]);
            }
        }
    }

    private void SetupAbilityBlessings(Array<Ability> ability, SavedCharacterResource character) // patikrinti ar ability yra unlocked
    {
        if (ability != null)
        {
            if (_abilityBlessings == null)
            {
                _abilityBlessings = new Array<BlessingData>();
            }

            Array<UnlockedAbilitiesResource> unlockedAbilitiesList = character.unlockedAbilities;
            for (int i = 0; i < ability.Count; i++)
            {
                if (ability[i].Action.GetAllBlessings() != null && unlockedAbilitiesList[i].abilityUnlocked && unlockedAbilitiesList[i].abilityConfirmed)
                {
                    Array<AbilityBlessing> abilityBlessings = ability[i].Action.GetAllBlessings();
                    for (int blessingIndex = 0; blessingIndex < abilityBlessings.Count; blessingIndex++)
                    {
                        BlessingData blessingData =
                            new BlessingData(character, abilityBlessings[blessingIndex], blessingIndex);
                        blessingData.SetupAbilityBlessing();
                        _abilityBlessings.Add(blessingData);
                    }
                }
            }
            
        }
    }
    
    private void SetupBaseAbilityBlessings(Array<Ability> baseAbility, SavedCharacterResource character) // patikrinti ar ability yra unlocked
    {
        if (baseAbility != null)
        {
            if (_abilityBlessings == null)
            {
                _abilityBlessings = new Array<BlessingData>();
            }
            for (int i = 0; i < baseAbility.Count; i++)
            {
                if (baseAbility[i].Action.GetAllBlessings() != null)
                {
                    Array<AbilityBlessing> abilityBlessings = baseAbility[i].Action.GetAllBlessings();
                    for (int blessingIndex = 0; blessingIndex < abilityBlessings.Count; blessingIndex++)
                    {
                        BlessingData blessingData =
                            new BlessingData(character, abilityBlessings[blessingIndex], blessingIndex);
                        blessingData.SetupBaseAbilityBlessing();
                        _abilityBlessings.Add(new BlessingData(character, abilityBlessings[blessingIndex], blessingIndex));
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
            for (int i = 0; i < playerBlessings.Count; i++)
            {
                BlessingData blessingData =
                    new BlessingData(character, playerBlessings[i], i);
                blessingData.SetupPlayerBlessing();
                _playerBlessings.Add(blessingData);
            }
        }
    }

    private void GenerateBlessingList()
    {
        _playerBlessingsTemp = new Array<BlessingData>(_playerBlessings);
        _abilityBlessingsTemp = new Array<BlessingData>(_abilityBlessings);
        if (_generatedBlessings == null)
        {
            _generatedBlessings = new Array<BlessingData>();
        }

        for (int i = 0; i < 4; i++)
        {

            int maxRange = _abilityBlessingsTemp.Count + _playerBlessingsTemp.Count - 1;
            int index = _random.Next(0, maxRange);

            if (index < _playerBlessingsTemp.Count)
            {
                if (!_generatedBlessings.Contains(_playerBlessingsTemp[index]))
                {
                    _generatedBlessings.Add(_playerBlessingsTemp[index]);
                    _playerBlessingsTemp.RemoveAt(index);
                }
            }
            else if (index >= _playerBlessingsTemp.Count)
            {
                int correctIndex = _playerBlessings.Count - index;
                if (!_generatedBlessings.Contains(_abilityBlessingsTemp[correctIndex]))
                {
                    _generatedBlessings.Add(_abilityBlessingsTemp[correctIndex]);
                    _abilityBlessingsTemp.RemoveAt(correctIndex);
                }
            }
        }

    }

    private Array<BlessingData> GeneratedBlessings()
    {
        return _generatedBlessings;
    }
}