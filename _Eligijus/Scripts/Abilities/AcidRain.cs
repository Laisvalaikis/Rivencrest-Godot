using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class AcidRain : BaseAction
{
	[Export] private int poisonDamage;
	[Export] private int poisonTurns;
	public AcidRain()
	{
	}
 	public AcidRain(AcidRain acidRain): base(acidRain)
	{
		poisonDamage = acidRain.poisonDamage;
		poisonTurns = acidRain.poisonTurns;
		enemyDisplayText = "POISON";
	}
 	
 	public override BaseAction CreateNewInstance(BaseAction action)
 	{
 		AcidRain acidRain = new AcidRain((AcidRain)action);
 		return acidRain;
 	}

	public override void ResolveAbility(ChunkData chunk)
	{
		UpdateAbilityButton(); 
		foreach (ChunkData tile in _chunkList)
		{
			if (CanBeUsedOnTile(tile))
			{
				Player target = tile.GetCurrentPlayer();
				PoisonDebuff debuff = new PoisonDebuff(2, 2);
				target.debuffManager.AddDebuff(debuff, _player);
			} 
		}
		base.ResolveAbility(chunk); 
		FinishAbility();
	}
}
