using Godot;

public partial class AffectAllies : AbilityBlessing
{
	[Export] private bool affectAllies = false;
	
	public AffectAllies()
	{
			
	}
	
	public AffectAllies(AffectAllies blessing): base(blessing)
	{
		affectAllies = blessing.affectAllies;
	}
	
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		AffectAllies blessing = new AffectAllies((AffectAllies)baseBlessing);
		return blessing;
	}
	
	public override BaseBlessing CreateNewInstance()
	{
		AffectAllies blessing = new AffectAllies(this);
		return blessing;
	}

	public override void OnTurnStart(BaseAction baseAction)
	{
		base.OnTurnStart(baseAction);
		baseAction.SetFriendlyFire(affectAllies);
	}
}
