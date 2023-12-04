namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

//player cant move but can use abilities?? cj aurio comment
public partial class RootDebuff: BaseDebuff
{

	public RootDebuff()
	{

	}

	public RootDebuff(int lifetime)
	{
		this.lifetime = lifetime;
	}

	public RootDebuff(RootDebuff debuff) : base(debuff)
	{

	}

	public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
	{
		RootDebuff debuff = new RootDebuff((RootDebuff)baseDebuff);
		return debuff;
	}

	public override BaseDebuff CreateNewInstance()
	{
		RootDebuff debuff = new RootDebuff(this);
		return debuff;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		_player.SetMovementPoints(0);
		lifetimeCount++;
	}
}
