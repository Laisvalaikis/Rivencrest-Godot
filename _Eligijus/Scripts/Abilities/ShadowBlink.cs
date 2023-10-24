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
        ChunkData current = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        Side side = ChunkSideByCharacter(current, chunk);
        (int x, int y) sideVector = GetSideVector(side);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        sideVector = (sideVector.x, sideVector.y);
        MovePlayerToSide(current, sideVector, chunk);
        FinishAbility();
    }
    
}
