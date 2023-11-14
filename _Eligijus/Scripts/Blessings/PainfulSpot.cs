using Godot;

public partial class PainfulSpot : AbilityBlessing
{
    [Export] private int damage = 2;
    
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
            if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction) && player.HaveWeakSpot())
            {
                tile.GetCurrentPlayer().playerInformation.DealDamage(damage, false, baseAction.GetPlayer());
            }
        }
    }
    
}