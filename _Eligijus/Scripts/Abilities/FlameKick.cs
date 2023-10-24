using Godot;

public partial class FlameKick : BaseAction
{
    //Ability buvo remove'intas. (Player orange abilitis)
    //Assuminu, kad del to jis nebaigtas
    public FlameKick()
    {
 		
    }
    public FlameKick(FlameKick flameBlast): base(flameBlast)
    {
    }
 	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        FlameKick flameBlast = new FlameKick((FlameKick)action);
        return flameBlast;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        ChunkData current = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        if (!IsAllegianceSame(chunk))
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
        Side side = ChunkSideByCharacter(current, chunk);
        (int x, int y) sideVector = GetSideVector(side);
        sideVector = (sideVector.x, sideVector.y);
        MovePlayerToSide(chunk, sideVector);
        FinishAbility();
    }
}