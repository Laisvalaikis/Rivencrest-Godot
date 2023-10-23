using Godot;
using System;

public partial class Concussion : AbilityBlessing
{
	public Concussion()
	{
		
	}
	public Concussion(Concussion blessing): base(blessing)
	{
		
	}
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		Concussion blessing = new Concussion((Concussion)baseBlessing);
		return blessing;
	}
	public override BaseBlessing CreateNewInstance()
	{
		Concussion blessing = new Concussion(this);
		return blessing;
	}

	public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(ref baseAction, tile);
		if (tile.CharacterIsOnTile())
		{
			Player player = tile.GetCurrentPlayer();
			if (!IsAllegianceSame(baseAction.GetPlayer().playerInformation, tile, baseAction))
			{
				// Create can't use abilities for one turn
				GD.PrintErr("can't use abilities for one turn");
			}
		}
	}
}
