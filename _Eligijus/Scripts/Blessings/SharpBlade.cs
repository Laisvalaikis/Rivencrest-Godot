using Godot;

public partial class SharpBlade : AbilityBlessing
{
    public SharpBlade()
    {
			
    }
    
    public SharpBlade(SharpBlade blessing): base(blessing)
    {


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
           tile.GetCurrentPlayerInformation().DealDamage(2, false, baseAction.GetPlayer());
       }
    }

}