using Godot;

public partial class LeapAndSlam : BaseAction
{
    LeapAndSlam()
    {
        
    }
    public LeapAndSlam(LeapAndSlam ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        LeapAndSlam ability = new LeapAndSlam((LeapAndSlam)action);
        return ability;
    }
    public override bool CanBeUsedOnTile(ChunkData chunkData)
    {
        return !chunkData.CharacterIsOnTile();
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        UpdateAbilityButton();
        if (!chunk.CharacterIsOnTile())
        {
            PlayerAbilityAnimation();
            PlayAnimation("Burgundy3", chunk);
            GameTileMap.Tilemap.MoveSelectedCharacter(chunk);
        }
        DamageAdjacent(chunk);
        FinishAbility();
    }

    protected override bool CanAddTile(ChunkData chunk)
    {
        return chunk != null && !chunk.TileIsLocked() && chunk.GetCurrentPlayer() == null;
    }
    private void DamageAdjacent(ChunkData centerChunk)
    {
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int x, int y) indexes = centerChunk.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && chunks[nx, ny].GetCurrentPlayer() != null)
            {
                DealRandomDamageToTarget(chunks[nx, ny], minAttackDamage, maxAttackDamage);
            }
        }
    }
}
