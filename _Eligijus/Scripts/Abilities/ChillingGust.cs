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

    public override void OnTurnStart()
    {
        if (_protectedAlly != null)
        {
            _protectedAlly.playerInformation.Protected = false;
            _protectedAlly = null;
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
            base.ResolveAbility(chunk);
            Player target = chunk.GetCurrentPlayer();
            PlayerInformation clickedPlayerInformation = target.playerInformation;
            
            if (IsAllegianceSame(chunk))
            {
                clickedPlayerInformation.Protected = true;
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
    
    public override void SetHoveredAttackColor(ChunkData chunkData)
    {
        Node2D character = chunkData.GetCurrentPlayer();
        HighlightTile tileHighlight = chunkData.GetTileHighlight();

        if (character != null && IsAllegianceSame(chunkData))
        {
            tileHighlight.SetHighlightColor(abilityHoverCharacter);
            EnableDamagePreview(chunkData,"PROTECT");
        }
        else
        {
            tileHighlight.SetHighlightColor(abilityHighlightHover);
        }
    }
    
}
