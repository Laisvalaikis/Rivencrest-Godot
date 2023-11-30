using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class FlameKick : BaseAction
{
    [Export] private AflameDebuff debuff;
    public FlameKick()
    {
 		
    }
    public FlameKick(FlameKick flameBlast): base(flameBlast)
    {
        debuff = flameBlast.debuff;
    }
 	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        FlameKick flameBlast = new FlameKick((FlameKick)action);
        return flameBlast;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        ChunkData current = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        if (!IsAllegianceSame(chunk))
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
        chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff,player);
        Side side = ChunkSideByCharacter(current, chunk);
        (int x, int y) sideVector = GetSideVector(side);
        sideVector = (sideVector.x, sideVector.y);
        MovePlayerToSide(chunk, sideVector);
        FinishAbility();
    }
}