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
    protected override void SetHoveredAttackColor(ChunkData chunkData)
    {
        Node2D character = chunkData.GetCurrentCharacter();
        HighlightTile tileHighlight = chunkData.GetTileHighlight();

        if (character != null && IsAllegianceSame(chunkData))
        {
            tileHighlight.SetHighlightColor(abilityHoverCharacter);
            EnableDamagePreview(chunkData,"BLOCK");
        }
        else
        {
            tileHighlight.SetHighlightColor(abilityHighlightHover);
        }
    }
    protected override void HighlightGridTile(ChunkData chunkData)
    {
        if(chunkData.GetCurrentCharacter()!=GameTileMap.Tilemap.GetCurrentCharacter())
        {
            chunkData.GetTileHighlight().ActivateColorGridTile(true);
            SetNonHoveredAttackColor(chunkData);
        }
    }
    public override bool CanTileBeClicked(ChunkData chunk)
    {
        return IsAllegianceSame(chunk);
    }
    public override void OnTurnStart()
    {
        if (_characterBeingBlocked != null)
        {
            
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
            PlayerInformation playerInformationLocal = chunk.GetCurrentPlayerInformation();
            if (playerInformationLocal != null)
                // playerInformationLocal.BlockingAlly = GameTileMap.Tilemap.GetCurrentCharacter();
            _characterBeingBlocked = (Player)chunk.GetCurrentCharacter();
            // player.playerInformation.Blocker = true;
            FinishAbility();
        }
    }
   
    
}

