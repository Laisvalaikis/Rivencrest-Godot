using Godot;
using System.Collections.Generic;
using System;

public partial class HealingWinds : AbilityBlessing
{
	[Export] private int minHeal = 3;
	[Export] private int maxHeal = 5;
	private Random _random;
	public HealingWinds()
	{
		
	}
	public HealingWinds(HealingWinds blessing): base(blessing)
	{
		minHeal = blessing.minHeal;
		maxHeal = blessing.maxHeal;
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
	
	public override void ResolveBlessing(BaseAction baseAction, ChunkData chunkData)
	{
		base.ResolveBlessing(baseAction);
		List<ChunkData> tiles = baseAction.GetChunkList();
		_random = new Random();
		//foreach (ChunkData tile in tiles)
		//{
			Player player = chunkData.GetCurrentPlayer();
			if (IsAllegianceSame(baseAction.GetPlayer(), chunkData, baseAction))
			{
				int randomHeal = _random.Next(minHeal, maxHeal);
				player.objectInformation.GetPlayerInformation().Heal(randomHeal);
			}
		//}
	}
	
}
