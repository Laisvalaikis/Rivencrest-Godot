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
    
    public override void ResolveAbility(ChunkData chunk)
    {
            base.ResolveAbility(chunk);
            if (!GameTileMap.Tilemap.CharacterIsOnTile(chunk))
            {
                GameTileMap.Tilemap.MoveSelectedCharacter(chunk);
            }
            DamageAdjacent(chunk);
            FinishAbility();
    }
    private void DamageAdjacent(ChunkData centerChunk)
    {
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int y, int x) indexes = centerChunk.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (GameTileMap.Tilemap.CheckBounds(ny, nx) && chunks[ny, nx]?.GetCurrentCharacter() != null && !IsAllegianceSame(chunks[ny,nx]))
            {
                DealRandomDamageToTarget(chunks[nx, ny], minAttackDamage, maxAttackDamage);
            }
        }
    }
}
