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

    public void AddPoison(Poison poison)
    {
        _player.AddPoison(poison);
    }

    public void SetPlayerTeam(int currentCharacterTeam)
    {
        _player.SetPlayerTeam(currentCharacterTeam);
    }

    public int GetPlayerTeam()
    {
        return _player.GetPlayerTeam();
    }

    public void ClearPoison()
    {
        _player.ClearPoison();
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

    public int GetPoisonCount()
    {
        return _player.GetPoisonCount();
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
        _player.playerInformation.DealDamage(damage, damageDealer);
    }

    public void DeathStart(Player damageDealer)
    {
        _player.playerInformation.DeathStart(damageDealer);
    }

    public void AddKillXP()
    {
        _player.playerInformation.AddKillXP();
    }

    public int GainXP()
    {
        return _player.playerInformation.GainXP();
    }

    public Type GetInformationType()
    {
        return _player.playerInformation.GetInformationType();
    }

    public void SetInformationType(Type informationType)
    {
        _player.playerInformation.SetInformationType(informationType);
    }

    public float GetHealthPercentage()
    {
        return _player.playerInformation.GetHealthPercentage();
    }
}