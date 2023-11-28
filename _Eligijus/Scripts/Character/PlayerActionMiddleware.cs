namespace Rivencrestgodot._Eligijus.Scripts.Character;
using System;

public class PlayerActionMiddleware
{
    public Player _player;
    
    public void Death()
    {
        _player.Death();    
    }

    public void PlayerWasDamaged()
    {
        _player.PlayerWasDamaged();
    }

    public int GetMovementPoints()
    {
        return _player.GetMovementPoints();
    }

    public int GetPreviousMovementPoints()
    {
        return _player.GetPreviousMovementPoints();
    }

    public void AddMovementPoints(int points)
    {
        _player.AddMovementPoints(points);
    }

    public void SetMovementPoints(int points)
    {
        _player.SetMovementPoints(points);
    }
    public void SetPlayerTeam(int currentCharacterTeam)
    {
        _player.SetPlayerTeam(currentCharacterTeam);
    }

    public int GetPlayerTeam()
    {
        return _player.GetPlayerTeam();
    }

    public void OnTurnStart()
    {
        _player.OnTurnStart();
    }

    public void OnAfterResolve()
    {
        _player.OnAfterResolve();
    }

    public void OnBeforeStart()
    {
        _player.OnBeforeStart();
    }

    public  void OnTurnEnd()
    {
        _player.OnTurnEnd();
    }

    public void AddBarrier()
    {
        _player.AddBarrier();
    }

    public void RemoveBarrier()
    {
        _player.RemoveBarrier();
    }
    //--------------------Player information methods--------------------

    public void DealDamage(int damage, Player damageDealer)
    {
        _player.objectInformation.GetPlayerInformation().DealDamage(damage, damageDealer);
    }

    public void DeathStart(Player damageDealer)
    {
        _player.objectInformation.GetPlayerInformation().DeathStart(damageDealer);
    }

    public void AddKillXP()
    {
        _player.objectInformation.GetPlayerInformation().AddKillXP();
    }

    public int GainXP()
    {
        return _player.objectInformation.GetPlayerInformation().GainXP();
    }

    public Type GetInformationType()
    {
        return _player.objectInformation.GetPlayerInformation().GetInformationType();
    }

    public void SetInformationType(Type informationType)
    {
        _player.objectInformation.GetPlayerInformation().SetInformationType(informationType);
    }

    public float GetHealthPercentage()
    {
        return _player.objectInformation.GetPlayerInformation().GetHealthPercentage();
    }
}