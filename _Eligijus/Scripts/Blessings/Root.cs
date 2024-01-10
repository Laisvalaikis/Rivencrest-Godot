using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Root : AbilityBlessing
{
    
    public Root()
    {
			
    }
    
    public Root(Root blessing): base(blessing)
    {
 
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Root blessing = new Root((Root)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Root blessing = new Root(this);
        return blessing;
    }

    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (tile.CharacterIsOnTile())
        {
            Player player = tile.GetCurrentPlayer();
            if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction))
            {
                RootDebuff debuff = new RootDebuff(1);
                tile.GetCurrentPlayer().debuffManager.AddDebuff(debuff, player);
            }
        }
    }
}