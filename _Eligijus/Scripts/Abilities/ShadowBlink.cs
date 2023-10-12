using Godot;

public partial class ShadowBlink : BaseAction
{
    public ShadowBlink()
    {
    }

    public ShadowBlink(ShadowBlink ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        ShadowBlink ability = new ShadowBlink((ShadowBlink)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        ChunkData current = GameTileMap.Tilemap.GetChunk(player.GlobalPosition + new Vector2(0, 50f));
        Side side = ChunkSideByCharacter(current, chunk);
        (int x, int y) sideVector = GetSideVector(side);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        sideVector = (sideVector.x * -1, sideVector.y * -1);
        MovePlayerToSide(current, sideVector);
        FinishAbility();
    }
    
}
