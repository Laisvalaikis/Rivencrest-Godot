using System;
using Godot;
using Godot.Collections;
using Array = System.Array;

public partial class BlessingManager : Node
{
    
    private Array<PlayerBlessing> _playerBlessings;
    private Array<AbilityBlessing> _abilityBlessings;
    
    private Array<PlayerBlessing> _playerBlessingsTemp;
    private Array<AbilityBlessing> _abilityBlessingsTemp;
    
    private Data _data;
    private Array<BaseBlessing> _generatedBlessings;
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
                SetupPlayerBlessings(playerInformationData.GetAllBlessings());
                SetupAbilityBlessings(playerInformationData.baseAbilities);
                SetupAbilityBlessings(playerInformationData.abilities);
            }
        }
    }

    private void SetupAbilityBlessings(Array<Ability> baseAbility)
    {
        if (baseAbility != null)
        {
            if (_abilityBlessings == null)
            {
                _abilityBlessings = new Array<AbilityBlessing>();
            }
            for (int i = 0; i < baseAbility.Count; i++)
            {
                if (baseAbility[i].Action.GetAllBlessings() != null)
                {
                    Array<AbilityBlessing> abilityBlessings = baseAbility[i].Action.GetAllBlessings();
                    for (int blessingIndex = 0; blessingIndex < abilityBlessings.Count;blessingIndex++)
                    {
                        _abilityBlessings.Add(abilityBlessings[blessingIndex]);
                    }
                }
            }
            
        }
    }
    
    private void SetupPlayerBlessings(Array<PlayerBlessing> playerBlessings)
    {
        if (playerBlessings != null)
        {
            if (_playerBlessings == null)
            {
                _playerBlessings = new Array<PlayerBlessing>();
            }
            for (int i = 0; i < playerBlessings.Count; i++)
            {
                _playerBlessings.Add(playerBlessings[i]);
            }
        }
    }

    private void GenerateBlessingList()
    {
        _playerBlessingsTemp = new Array<PlayerBlessing>(_playerBlessings);
        _abilityBlessingsTemp = new Array<AbilityBlessing>(_abilityBlessings);
        if (_generatedBlessings == null)
        {
            _generatedBlessings = new Array<BaseBlessing>();
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

    private Array<BaseBlessing> GeneratedBlessings()
    {
        return _generatedBlessings;
    }
}