using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class PainfulSpot : AbilityBlessing
{
    [Export] private int damage = 10;
    
    public PainfulSpot()
    {
		
    }
    public PainfulSpot(PainfulSpot blessing): base(blessing)
    {
        damage = blessing.damage;
    }
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        PainfulSpot blessing = new PainfulSpot((PainfulSpot)baseBlessing);
        return blessing;
    }
    public override BaseBlessing CreateNewInstance()
    {
        PainfulSpot blessing = new PainfulSpot(this);
        return blessing;
    }
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (tile.CharacterIsOnTile())
        {
            Player player = tile.GetCurrentPlayer();
            WeakSpotDebuff debuff = new WeakSpotDebuff(2);
            tile.GetCurrentPlayer().debuffManager.AddDebuff(debuff, player);
            if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction) && player.debuffManager.ContainsDebuff(typeof(WeakSpotDebuff)))
            {
                tile.GetCurrentPlayer().objectInformation.GetPlayerInformation().DealDamage(damage, baseAction.GetPlayer());
            }
        }
    }
    
}