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
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        BlockAbility ability = new BlockAbility((BlockAbility)action);
        return ability;
    }

    public override void Start()
    {
        base.Start();
        customText = "BLOCK";
    }

    protected override void SetNonHoveredAttackColor(ChunkData chunkData)
    {
        HighlightTile tileHighlight = chunkData.GetTileHighlight();
        if (chunkData.CharacterIsOnTile() && IsAllegianceSame(chunkData))
        {
            tileHighlight.SetHighlightColor(characterOnGrid);
        }
        else
        {
            tileHighlight.SetHighlightColor(abilityHighlight);
        }
    }
    public override void SetHoveredAttackColor(ChunkData chunkData)
    {
        Node2D character = chunkData.GetCurrentPlayer();
        HighlightTile tileHighlight = chunkData.GetTileHighlight();

        if (character != null && IsAllegianceSame(chunkData))
        {
            tileHighlight.SetHighlightColor(abilityHoverCharacter);
        }
        else
        {
            tileHighlight.SetHighlightColor(abilityHighlightHover);
        }
    }
    public override bool CanTileBeClicked(ChunkData chunkData)
    {
        return chunkData.GetTileHighlight().isHighlighted && IsAllegianceSame(chunkData);
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            if (chunk.GetCurrentPlayer() != null)
            {
                BlockedBuff buff = new BlockedBuff(_player);
                BlockerDebuff debuff = new BlockerDebuff();
                _player.debuffManager.AddDebuff(debuff, _player);
                chunk.GetCurrentPlayer().buffManager.AddBuff(buff);
            }
            FinishAbility();
        }
    }
    
}

