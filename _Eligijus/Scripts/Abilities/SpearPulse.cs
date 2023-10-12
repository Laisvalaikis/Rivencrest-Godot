using Godot;

public partial class SpearPulse : BaseAction
{
    private ChunkData _currentChunk;

    public SpearPulse()
    {
    }

    public SpearPulse(SpearPulse ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        SpearPulse ability = new SpearPulse((SpearPulse)action);
        return ability;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            for (int i = 0; i < _chunkList.Count; i++)
            {
                DealRandomDamageToTarget(_chunkList[i], minAttackDamage, maxAttackDamage);
            }
            base.ResolveAbility(chunk);
            FinishAbility();
        }
    }
    
    public override void CreateGrid()
    {
        if (_currentChunk != null && _currentChunk.CharacterIsOnTile())
        {
            CreateAvailableChunkList(attackRange);
            HighlightAllGridTiles();
        }
    }
    
    public override void OnMoveArrows(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk != _currentChunk)
        {
            _currentChunk = hoveredChunk;
            if (_currentChunk != null && _currentChunk.CharacterIsOnTile())
            {
                ClearGrid();
                GenerateDiamondPattern(_currentChunk, attackRange);
            }
        }
    }
}
