using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class FlameKick : BaseAction
{
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
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        ChunkData current = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        if (!IsAllegianceSame(chunk))
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }

        AflameDebuff debuff = new AflameDebuff();
        chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);
        Side side = ChunkSideByCharacter(current, chunk);
        (int x, int y) sideVector = GetSideVector(side);
        sideVector = (sideVector.x, sideVector.y);
        MovePlayerToSide(chunk, sideVector);
        FinishAbility();
    }
}