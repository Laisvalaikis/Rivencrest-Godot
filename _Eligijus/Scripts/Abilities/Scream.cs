using Godot;

public partial class Scream : BaseAction
{
    public Scream()
    {
    }

    public Scream(Scream ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Scream ability = new Scream((Scream)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        ChunkData current = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        Side side = ChunkSideByCharacter(current, chunk);
        (int x, int y) sideVector = GetSideVector(side);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        sideVector = (sideVector.x, sideVector.y );
        MovePlayerToSide(current, sideVector,chunk);
        if (IsTargetIsolated(chunk))
        {
            //apply silenced
            Player target = chunk.GetCurrentPlayer();
            target.debuffs.SetTurnCounterFromThisTurn(1);
        }
        FinishAbility();
    }
    
    private bool IsTargetIsolated(ChunkData target)
    {
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int x, int y) indexes = target.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && chunks[nx, ny]?.GetCurrentPlayer() != null && 
                chunks[nx, ny].GetCurrentPlayer().playerInformation.CharactersTeam == target.GetCurrentPlayer().playerInformation.CharactersTeam)
            {
                return false;
            }
        }
        return true;
    }
}

