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
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
       base.ResolveBlessing(baseAction, tile);
       if (tile.CharacterIsOnTile())
       {
           tile.GetCurrentPlayer().playerInformation.DealDamage(dealDamage, false, baseAction.GetPlayer());
       }
    }

}