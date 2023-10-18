using Godot;
using System;

public partial class HealingWinds : AbilityBlessing
{
	private int minHeal = 3;
	private int maxHeal = 5;
	private Random _random;
	public HealingWinds()
	{
		
	}
	public HealingWinds(HealingWinds blessing): base(blessing)
	{
		
	}
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		HealingWinds blessing = new HealingWinds((HealingWinds)baseBlessing);
		return blessing;
	}
	public override BaseBlessing CreateNewInstance()
	{
		HealingWinds blessing = new HealingWinds(this);
		return blessing;
	}
	public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(ref baseAction);
		PlayerInformation player = tile.GetCurrentPlayerInformation();
		if (IsAllegianceSame(baseAction.GetPlayer().playerInformation, tile, baseAction))
		{
			int randomHeal = _random.Next(minHeal, maxHeal);
			player.Heal(randomHeal);
		}
		
	}
}
