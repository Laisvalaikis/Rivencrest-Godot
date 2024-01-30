using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Rivencrestgodot._Eligijus.Scripts.AISystem;

public class AIEnemy
{
    [Export] private Player player;
    private Array<Ability> _playerAbilities;
    private List<(BaseAction, int)> possibleActions;

    public void Start()
    {
        _playerAbilities = player.actionManager.GetAllAbilities();
    }

    //Checks to see if a character has any abilities that can be used
    //If a character has at least one ability (excluding the first ability, movement)
    //that is unlocked and is not under cooldown, method returns true
    public bool HasAvailableAbilities()
    {
        for (int i = 1; i < _playerAbilities.Count; i++)
        {
            if (_playerAbilities[i].enabled && _playerAbilities[i].Action.AbilityCanBeActivated())
            {
                return true;
            }
        }
        return false;
    }

    //Fills up possibleActions list with abilities that could
    //currently be used on the generated grid without moving the character
    public void GeneratePossibleActions()
    {
        for (int i = 1; i < _playerAbilities.Count; i++)
        {
            if (_playerAbilities[i].enabled && _playerAbilities[i].Action.AbilityCanBeActivated())
            {
                _playerAbilities[i].Action.CreateAvailableChunkList(_playerAbilities[i].Action.GetRange());
                
            }
        }
    }
    
}