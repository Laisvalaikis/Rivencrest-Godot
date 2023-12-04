using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class VenomousTouch : AbilityBlessing
{
	[Export] private int poisonCounut = 1;
	[Export] private int poisonTurns = 2;
	[Export] private int poisonDamage = 2;
	
	public VenomousTouch()
	{
			
	}
	
	public VenomousTouch(VenomousTouch blessing): base(blessing)
	{
		poisonCounut = blessing.poisonCounut;
		poisonTurns = blessing.poisonTurns;
		poisonDamage = blessing.poisonDamage;
	}
	
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		VenomousTouch blessing = new VenomousTouch((VenomousTouch)baseBlessing);
		return blessing;
	}
	
	public override BaseBlessing CreateNewInstance()
	{
		VenomousTouch blessing = new VenomousTouch(this);
		return blessing;
	}

	public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(baseAction);
		Player player = new Player();
		PoisonDebuff debuff = new PoisonDebuff(2,2);
		tile.GetCurrentPlayer().debuffManager.AddDebuff(debuff,player);
		debuff.SetPoisonDebuffDamage(poisonDamage);
		debuff.SetLifetime(poisonTurns);
		tile.GetCurrentPlayer().debuffManager.AddDebuff(debuff, baseAction.GetPlayer());
	}
}
