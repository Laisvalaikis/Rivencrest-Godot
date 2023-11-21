namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class RootDebuff: BaseDebuff
{

	public RootDebuff()
	{

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
}