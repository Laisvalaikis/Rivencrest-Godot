using Godot;

public partial class FromTheShadows : BaseAction
{

    public FromTheShadows()
    {
        
    }
    public FromTheShadows(FromTheShadows ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        FromTheShadows ability = new FromTheShadows((FromTheShadows)action);
        return ability;
    }
    
    protected override bool CanAddTile(ChunkData chunk)
    {
        return chunk != null && !chunk.TileIsLocked() && chunk.GetCurrentPlayer() == null;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        if (!chunk.CharacterIsOnTile())
        {
            GameTileMap.Tilemap.MoveSelectedCharacter(chunk);
        }
        DamageAdjacent(chunk);
        FinishAbility();
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

            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && chunks[nx, ny]?.GetCurrentPlayer() != null && !IsAllegianceSame(chunks[nx,ny]))
            {
                DealRandomDamageToTarget(chunks[nx, ny], minAttackDamage, maxAttackDamage);
            }
        }
    }
}
