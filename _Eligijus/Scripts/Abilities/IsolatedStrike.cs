using Godot;

public partial class IsolatedStrike : BaseAction
{
    [Export]
    private int isolationDamage = 7;

    public IsolatedStrike()
    {
        
    }
    public IsolatedStrike(IsolatedStrike ability): base(ability)
    {
        isolationDamage = ability.isolationDamage;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        IsolatedStrike ability = new IsolatedStrike((IsolatedStrike)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        int bonusDamage = 0;
        //Isolation
        if (IsTargetIsolated(chunk))
        {
            bonusDamage += isolationDamage;
        }
        DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
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

            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && chunks[nx, ny].IsStandingOnChunk() && IsAllegianceSame(chunks[nx, ny]))
            {
                return false;
            }
            
        }
        return true;
    }
}
