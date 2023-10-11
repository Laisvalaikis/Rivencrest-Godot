using Godot;
using Godot.Collections;
using Array = System.Array;

public partial class BlessingManager : Node
{
    private Array<PlayerBlessing> _playerBlessings;
    private Array<AbilityBlessing> _abilityBlessings;
    private Data _data;

    public override void _Ready()
    {
        base._Ready();
        if (Data.Instance != null)
        {
            _data = Data.Instance;
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
}