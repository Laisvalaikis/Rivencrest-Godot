using Godot;
using System;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class ReleaseToxins : AbilityBlessing
{
	[Export] private int bonusDamage = 20;
	public ReleaseToxins()
	{
			
	}
    
	public ReleaseToxins(ReleaseToxins blessing): base(blessing)
	{
		bonusDamage = blessing.bonusDamage;
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
	public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(baseAction);
		//Player player = new Player();
		if (tile.GetCurrentPlayer().debuffManager.ContainsDebuff(typeof(PoisonDebuff)))
		{
			DealDamage(tile,baseAction.GetPlayer(),baseAction,bonusDamage);
		}
		//PoisonDebuff debuff = new PoisonDebuff(2,2);
		//tile.GetCurrentPlayer().debuffManager.AddDebuff(debuff,player);
		//if(tile.CharacterIsOnTile() && !IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction) && tile.GetCurrentPlayer().debuffManager.ContainsDebuff((typeof(PoisonDebuff))))
		//{
		//	DealRandomDamageToTarget(baseAction.GetPlayer(), tile, baseAction, bonusDamage, bonusDamage);
		//}
	}
}
