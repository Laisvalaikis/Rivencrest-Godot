using System.Collections.Generic;
using Godot;

public partial class GroundSlam : BaseAction
{
    private bool isAbilityActive;
    private List<ChunkData> _chunkListCopy;

    public GroundSlam()
    {
        
    }
    public GroundSlam(GroundSlam ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        GroundSlam ability = new GroundSlam((GroundSlam)action);
        return ability;
    }

    public override void CreateGrid()
    {
        base.CreateGrid();
        _chunkListCopy = new List<ChunkData>(_chunkList);
    }
    protected override void HighlightGridTile(ChunkData chunkData)
    {
        SetNonHoveredAttackColor(chunkData);
        chunkData.GetTileHighlight().EnableTile(true);
        chunkData.GetTileHighlight().ActivateColorGridTile(true);
    }
    
    protected override void TryAddTile(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked())
        {
            _chunkList.Add(chunk);
        }
    }

    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if(isAbilityActive && player.objectInformation.GetPlayerInformation().GetHealth() > 0)
        {
            DealDamageToAdjacent();
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (_chunkList.Contains(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            DealDamageToAdjacent();
            isAbilityActive = true;
            FinishAbility();
        }
    }
    private void DealDamageToAdjacent()
    {
        foreach (var chunk in _chunkListCopy)
        {
            if (chunk.CharacterIsOnTile() && !IsAllegianceSame(chunk))
            {
                DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            }
        }
    }
    public override bool CanTileBeClicked(ChunkData chunkData)
    {
        return chunkData.GetCurrentPlayer() != GameTileMap.Tilemap.GetCurrentCharacter() && chunkData.GetTileHighlight().isHighlighted; //Prety sure ground slamas temamateus hittina?
    }
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        if ((hoveredChunkHighlight == null && previousChunkHighlight!=null && previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight!=null && !hoveredChunkHighlight.isHighlighted && previousChunkHighlight.isHighlighted))
        {
            foreach (var chunk in _chunkList)
            {
                SetNonHoveredAttackColor(chunk);
                DisableDamagePreview(chunk);
            }
        }
        else if ((hoveredChunkHighlight!=null && previousChunkHighlight!=null && hoveredChunkHighlight.isHighlighted && !previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight==null && hoveredChunkHighlight.isHighlighted))
        {
            foreach (var chunk in _chunkList)
            {
                SetHoveredAttackColor(chunk);
                if (chunk.CharacterIsOnTile() && chunk.GetCurrentPlayer() != player)
                {
                    EnableDamagePreview(chunk);
                }
            }
        }
    }
}
