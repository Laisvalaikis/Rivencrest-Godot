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
        for (int i = 0; i < _chunkList.Count; i++)
        {
            DealRandomDamageToTarget(_chunkList[i], minAttackDamage, maxAttackDamage);
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            for (int i = 0; i < _chunkList.Count; i++)
            {
                ChunkData chunkToHighLight = _chunkList[i];
                if (chunkToHighLight != null)
                {
                    SetHoveredAttackColor(chunkToHighLight);
                    EnableDamagePreview(chunkToHighLight);
                }
            }
        }
        else
        {
            for (int i = 0; i < _chunkList.Count; i++)
            {
                ChunkData chunkToHighLight = _chunkList[i];
                if (chunkToHighLight != null)
                    SetNonHoveredAttackColor(chunkToHighLight);
            }
        }

    }
    
}
