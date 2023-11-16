using Godot;
using System;

public partial class IncreaseHealing : AbilityBlessing
{
	[Export] private int bonusHealing = 3;
	
	public IncreaseHealing()
	{
		
	}
	public IncreaseHealing(IncreaseHealing blessing): base(blessing)
	{
  
	}
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		IncreaseHealing blessing = new IncreaseHealing((IncreaseHealing)baseBlessing);
		return blessing;
	}
	public override BaseBlessing CreateNewInstance()
	{
		IncreaseHealing blessing = new IncreaseHealing(this);
		return blessing;
	}
	
	public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(baseAction, tile);
		if (tile.CharacterIsOnTile())
		{
			Player tempPlayer = tile.GetCurrentPlayer();
			tempPlayer.playerInformation.Heal(bonusHealing);
		}
	}
}
