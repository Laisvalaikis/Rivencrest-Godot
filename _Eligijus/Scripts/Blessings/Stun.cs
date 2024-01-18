using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Stun : AbilityBlessing
{
    public Stun()
    {
			
    }
    
    public Stun(Stun blessing): base(blessing)
    {
 
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Stun blessing = new Stun((Stun)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Stun blessing = new Stun(this);
        return blessing;
    }

    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        PlayerStun debuff = new PlayerStun();
        tile.GetCurrentPlayer().debuffManager.AddDebuff(debuff, baseAction.GetPlayer());
        
    }
}