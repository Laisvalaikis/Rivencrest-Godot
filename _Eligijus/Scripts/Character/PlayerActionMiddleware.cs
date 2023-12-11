using Rivencrestgodot._Eligijus.Scripts.BuffSystem;

namespace Rivencrestgodot._Eligijus.Scripts.Character;
using System;

public class PlayerActionMiddleware
{
    public Player _player;
    

    public void AddDebuff(ref BaseDebuff debuff, Player playerWhoCreatedDebuff)
    {
        LinkedList<BaseBuff> buffList = _player.buffManager.GetPlayerBuffs();
        for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
        {
            element.Value.ModifyDebuff(ref debuff);
        }
    }
    public void AddMovementPoints(ref int points)
    {
        LinkedList<BaseBuff> buffList = _player.buffManager.GetPlayerBuffs();
        for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
        {
            element.Value.ModifyMovement(ref points);
        }
    }

    public void DealDamage(ref int damage, ref Player playerWhoTakesDamage)
    {
        LinkedList<BaseBuff> buffList = _player.buffManager.GetPlayerBuffs();
        for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
        {
            element.Value.ModifyDamage(ref damage,ref playerWhoTakesDamage);
        }
    }
    public void AddBuff(ref BaseBuff buff, Player playerWhoCreatedDebuff)
    {
        LinkedList<BaseBuff> buffList = _player.buffManager.GetPlayerBuffs();
        for (LinkedListNode<BaseBuff> element = buffList.First; element != null; element = element.Next)
        {
            element.Value.ModifyBuff(ref buff);
        }
    }
}