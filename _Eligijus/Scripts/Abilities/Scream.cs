using Godot;

public partial class Scream : BaseAction
{
    
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
        ChunkData current = GameTileMap.Tilemap.GetChunk(player.GlobalPosition + new Vector2(0, 50f));
        Side side = ChunkSideByCharacter(current, chunk);
        (int x, int y) sideVector = GetSideVector(side);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        sideVector = (sideVector.x * -1, sideVector.y * -1);
        MovePlayerToSide(current, sideVector);
        FinishAbility();
    }
}
