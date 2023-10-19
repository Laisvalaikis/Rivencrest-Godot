using System.Collections.Generic;
using Godot;

public partial class GroundSlam : BaseAction
{
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
        chunkData.GetTileHighlight().ActivateColorGridTile(true);
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        //transform.Find("CharacterModel").GetComponent<Animator>().SetTrigger("spell3");
        DealDamageToAdjacent();
        FinishAbility();
    }
    private void DealDamageToAdjacent()
    {
        foreach (var chunk in _chunkListCopy)
        {
            SetNonHoveredAttackColor(chunk);
            if (chunk.GetCurrentPlayer() != null && chunk!=GameTileMap.Tilemap.GetChunk(player.GlobalPosition))
            {
                DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            }
        }
    }
    public override bool CanTileBeClicked(ChunkData chunk)
    {
        return chunk.GetCurrentPlayer() != GameTileMap.Tilemap.GetCurrentCharacter(); //Prety sure ground slamas temamateus hittina?
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
            }
        }
    }
}
