using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

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

    public void AddDebuff(BaseDebuff debuff, Player playerWhoCreatedDebuff)
    {
        LinkedList<BaseBuff> buffList = _player.buffManager.GetPlayerBuffs();
        for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
        {
            element.Value.ModifyDebuff(ref debuff);
        }
        _player.debuffManager.AddDebuff(debuff,playerWhoCreatedDebuff);
    }
    public void AddMovementPoints(int points)
    {
        LinkedList<BaseBuff> buffList = _player.buffManager.GetPlayerBuffs();
        for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
        {
            element.Value.ModifyMovement(ref points);
        }
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
        LinkedList<BaseBuff> buffList = _player.buffManager.GetPlayerBuffs();
        for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
        {
            element.Value.ModifyDamage(ref damage);
        }
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