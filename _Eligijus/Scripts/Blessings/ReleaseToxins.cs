using Godot;
using System;

public partial class ReleaseToxins : AbilityBlessing
{
	[Export] private int bonusDamage = 3;
	public ReleaseToxins()
	{
			
	}
    
	public ReleaseToxins(ReleaseToxins blessing): base(blessing)
	{
		
	}
    
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		ReleaseToxins blessing = new ReleaseToxins((ReleaseToxins)baseBlessing);
		return blessing;
	}
    
	public override BaseBlessing CreateNewInstance()
	{
		ReleaseToxins blessing = new ReleaseToxins(this);
		return blessing;
	}
	public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(ref baseAction);
		if(tile.CharacterIsOnTile() && !IsAllegianceSame(baseAction.GetPlayer().playerInformation, tile, baseAction) && tile.GetCurrentPlayer().GetPoisonCount() > 0)
		{
			DealRandomDamageToTarget(baseAction.GetPlayer().playerInformation, tile, baseAction, bonusDamage, bonusDamage);
		}
	}
}
