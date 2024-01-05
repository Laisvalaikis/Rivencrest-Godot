using Godot;
using System;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

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
		Player player = tile.GetCurrentPlayer();
		if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction))
		{
			SilenceDebuff debuff = new SilenceDebuff(2);
			player.debuffManager.AddDebuff(debuff, player);
		}
		
	}
}
