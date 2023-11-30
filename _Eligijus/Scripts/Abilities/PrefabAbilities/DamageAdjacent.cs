using System;
using Godot;

public partial class DamageAdjacent : BaseAction
{
    public DamageAdjacent()
    {
        
    }

    public DamageAdjacent(DamageAdjacent ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        DamageAdjacent ability = new DamageAdjacent((DamageAdjacent)action);
        return ability;
    }
    
    public override void OnTurnStart(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
            DamageArea(chunk);
        }
        _object.Death();
    }
    
    private void DamageArea(ChunkData centerChunk)
    {
        Random random = new Random();
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
            Player player = chunks[nx, ny].GetCurrentPlayer();
            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && player != null)
            {
                DealRandomDamageToTarget(chunks[nx, ny], minAttackDamage, maxAttackDamage);
            }
        }
    }
}