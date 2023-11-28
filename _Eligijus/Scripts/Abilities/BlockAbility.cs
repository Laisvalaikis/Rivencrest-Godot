using System;
using Godot;

public partial class BlockAbility : BaseAction
{
    private Player _characterBeingBlocked;
    private Random _random;

    public BlockAbility()
    {
        
    }
    public BlockAbility(BlockAbility ability): base(ability)
    {
        _random = new Random();
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        BlockAbility ability = new BlockAbility((BlockAbility)action);
        return ability;
    }

    public override void Start()
    {
        base.Start();
        _random = new Random();
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
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (_characterBeingBlocked != null)
        {
            _characterBeingBlocked.objectInformation.GetPlayerInformation().RemoveBarrier();
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk)) //currently paspaudus ant abiličio iš kart bando executint ir čia gaunas blogai
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            if (chunk.GetCurrentPlayer() != null)
            {
                _characterBeingBlocked = chunk.GetCurrentPlayer();
                _characterBeingBlocked.objectInformation.GetPlayerInformation().AddBarrier();
            }
            FinishAbility();
        }
    }

    public override void Die()
    {
        base.Die();
        if (_characterBeingBlocked != null)
        {
            _characterBeingBlocked.objectInformation.GetPlayerInformation().RemoveBarrier();
        }
    }
}

