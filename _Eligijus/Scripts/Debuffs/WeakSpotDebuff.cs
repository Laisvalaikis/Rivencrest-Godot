namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class WeakSpotDebuff : BaseDebuff
{
	public WeakSpotDebuff()
	{
			
	}
	public WeakSpotDebuff(int lifetime)
	{
		this.lifetime = lifetime;
		debuffAnimation = "PinkWeakSpot";
	}
	public WeakSpotDebuff(WeakSpotDebuff debuff): base(debuff)
	{
		
	}
	
	public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
	{
		WeakSpotDebuff debuff = new WeakSpotDebuff((WeakSpotDebuff)baseDebuff);
		return debuff;
	}
	
	public override BaseDebuff CreateNewInstance()
	{
		WeakSpotDebuff debuff = new WeakSpotDebuff(this);
		return debuff;
	}

	public override void PlayerWasAttacked()
	{
		_player.DealDamageUnnotified(10,playerWhoCreatedDebuff);
	}

}
