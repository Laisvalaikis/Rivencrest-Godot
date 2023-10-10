public partial class Enrage : BaseAction
{
	public Enrage()
	{
 		
	}
	public Enrage(Enrage enrage): base(enrage)
	{
	}
 	
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		Enrage enrage = new Enrage((Enrage)action);
		return enrage;
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		FinishAbility();
	}
}
