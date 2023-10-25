using System;
using Godot;

public partial class Debuffs : Node
{
    private bool _turnTracking = false;
    private int _turnThreshold = 0;
    private int _turnCount = 0;
    private int _turnsPassed = 0;
    private int _slowDownBy = 0;
    
    private bool _playerIsSlower = false;
    private bool _playerIsSilenced = false;
    private bool _playerIsRooted = false;
    private bool _playerIsStunned = false;
    private bool _playerIsMarked = false;


    public void TurnCounter()
    {
        _turnCount++;
    }

    public void SetTurnCounterFromThisTurn(int turnCount)
    {
        SilencePlayer();
        _turnsPassed = _turnCount;
        _turnTracking = true;
        _turnThreshold = turnCount;
    }
    
    public void RemoveSlowDown(ActionManager actionManager)
    {
        if (_playerIsSlower)
        {
            actionManager.AddAbilityPoints(_slowDownBy);
            _playerIsSlower = false;
        }
    }

    public bool IsMarked()
    {
        return _playerIsMarked;
    }

    public void UnMark()
    {
        _playerIsMarked = false;
    }

    public void Mark()
    {
        _playerIsMarked = true;
    }

    public void SlowDownPlayer(int slowdownBy)
    {
        _slowDownBy = Math.Abs(slowdownBy);
        _playerIsSlower = true;
    }

    public bool IsPlayerSlower()
    {
        return _playerIsSlower;
    }

    public void StunPlayer()
    {
        _playerIsStunned = true;
    }

    public void SilencePlayer()
    {
        _playerIsSilenced = true;
    }
    
    public bool IsPlayerSilenced()
    {
        return _playerIsSilenced;
    }

    public bool IsPlayerStunned()
    {
       return _playerIsStunned;
    }

    public bool IsPalyerRooted()
    {
        return _playerIsRooted;
    }

    public void RootPlayer()
    {
        _playerIsRooted = true;
    }
    
    public bool CanUseAbility()
    {
        bool result = false;
        if (!_playerIsStunned)
        {
            if (_turnTracking && _turnCount - _turnsPassed > _turnThreshold)
            {
                result = true;
            }
            else if (!_turnTracking)
            {
                result = true;
            }
        }

        return result;
    }
    
    public void CheckAbilityTurns()
    {
        if (_turnTracking && _turnCount - _turnsPassed > _turnThreshold)
        {
            _turnTracking = false;
        }
		
    }
    
    public bool CanUseBaseAbility(ActionManager actionManager)
    {
        bool result = false;

        if (!_playerIsStunned)
        {
            if (!_playerIsRooted && actionManager.IsMovementSelected())
            {
                result = true;
            }
            else if (!actionManager.IsMovementSelected())
            {
                result = true;
            }
        }

        return result;
    }
    
    public void CheckBaseAbilityTurns()
    {
        if (_playerIsRooted)
        {
            _playerIsRooted = false;
        }
    }
    
    public void CheckWholeAbilities()
    {
        if (_playerIsStunned)
        {
            _playerIsStunned = false;
        }
    }
}