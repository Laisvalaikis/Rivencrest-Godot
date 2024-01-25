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
    
    protected override void ModifyBonusDamage(ChunkData chunk)
    {
        if (IsTargetIsolated(chunk))
        {
            bonusDamage += isolationDamage;
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
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

            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && chunks[nx, ny].CharacterIsOnTile() && chunks[nx, ny].GetCurrentPlayer().GetPlayerTeam()==target.GetCurrentPlayer().GetPlayerTeam())
            {
                return false;
            }
        }
        return true;
    }
}
