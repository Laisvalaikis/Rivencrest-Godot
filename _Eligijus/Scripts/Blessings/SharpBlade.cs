using Godot;

public partial class SharpBlade : AbilityBlessing
{

    [Export] private int dealDamage = 2;
    
    public SharpBlade()
    {
			
    }
    
    public SharpBlade(SharpBlade blessing): base(blessing)
    {
        dealDamage = blessing.dealDamage;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        SharpBlade blessing = new SharpBlade((SharpBlade)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        SharpBlade blessing = new SharpBlade(this);
        return blessing;
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
       base.ResolveBlessing(ref baseAction, tile);
       if (tile.CharacterIsOnTile())
       {
           tile.GetCurrentPlayerInformation().DealDamage(dealDamage, false, baseAction.GetPlayer());
       }
    }

}