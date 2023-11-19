using Godot;

public partial class GlobalBlessing : BaseBlessing
{
	public GlobalBlessing()
	{
			
	}
	
	public GlobalBlessing(GlobalBlessing blessing): base(blessing)
	{
		
	}
	
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		GlobalBlessing blessing = new GlobalBlessing((GlobalBlessing)baseBlessing);
		return blessing;
	}

	public virtual void OnTurnStart(Player player)
	{
		
	}

	public override void ResolveBlessing(BaseAction baseAction)
	{
		base.ResolveBlessing(baseAction);
	}

	public virtual void Start(SavableCharacterResource playerInformation)
	{
		
	}
}
