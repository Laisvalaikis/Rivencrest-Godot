using System;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.BuffSystem;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class BlockAbility : BaseAction
{
    public BlockAbility()
    {
        
    }
    public BlockAbility(BlockAbility ability): base(ability)
    {
        teamDisplayText = "BLOCK";
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        BlockAbility ability = new BlockAbility((BlockAbility)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            BlockedBuff buff = new BlockedBuff(_player);
            BlockerDebuff debuff = new BlockerDebuff();
            _player.debuffManager.AddDebuff(debuff, _player);
            chunk.GetCurrentPlayer().buffManager.AddBuff(buff);
            FinishAbility();
        }
    }
    
}

