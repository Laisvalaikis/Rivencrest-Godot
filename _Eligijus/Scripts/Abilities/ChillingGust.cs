using System.Collections.Generic;
using Godot;

public partial class ChillingGust : BaseAction
{
    private List<ChunkData> _additionalDamageTiles = new List<ChunkData>();
    private Player _protectedAlly;

    public ChillingGust()
    {
        
    }
    public ChillingGust(ChillingGust ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        ChillingGust ability = new ChillingGust((ChillingGust)action);
        return ability;
    }

    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (_protectedAlly != null)
        {
            _protectedAlly.playerInformation.characterProtected = false;
            _protectedAlly = null;
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
            base.ResolveAbility(chunk);
            Player target = chunk.GetCurrentPlayer();
            PlayerInformation clickedPlayerInformation = target.playerInformation;
            
            if (IsAllegianceSame(chunk))
            {
                clickedPlayerInformation.characterProtected = true;
                _protectedAlly = target;
            }
            else
            {
                DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            }
            FinishAbility();
    }
    
    protected override void TryAddTile(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked())
        {
            _chunkList.Add(chunk);
        }
    }
    
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
        {
            SetNonHoveredAttackColor(previousChunk);
            DisableDamagePreview(previousChunk);
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted)
        {
            SetHoveredAttackColor(hoveredChunk);
            if (hoveredChunk.GetCurrentPlayer()!=null)
            {
                if (hoveredChunk.GetCurrentPlayer().GetPlayerTeam() == player.GetPlayerTeam())
                {
                    customText = "PROTECT";
                }
                EnableDamagePreview(hoveredChunk);
                customText = null;
            }
        }
        if (previousChunkHighlight != null)
        {
            SetNonHoveredAttackColor(previousChunk);
            DisableDamagePreview(previousChunk);
        }
    }
    
}
