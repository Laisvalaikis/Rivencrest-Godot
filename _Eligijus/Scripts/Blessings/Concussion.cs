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

	public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(baseAction, tile);
		if (tile.CharacterIsOnTile())
		{
			Player player = tile.GetCurrentPlayer();
			if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction))
			{
				player.debuffs.SetTurnCounterFromThisTurn(1);
				// GD.PrintErr("can't use abilities for one turn");
			}
		}
	}
}
