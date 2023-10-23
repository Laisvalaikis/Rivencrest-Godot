public partial class FlameBlast : BaseAction
{
	//Ability buvo remove'intas. (Player orange abilitis)
	//Assuminu, kad del to jis nebaigtas
	public FlameBlast()
	{
 		
	}
	public FlameBlast(FlameBlast flameBlast): base(flameBlast)
	{
	}
 	
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		FlameBlast flameBlast = new FlameBlast((FlameBlast)action);
		return flameBlast;
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		if (CheckIfSpecificInformationType(chunk, InformationType.Player) && (IsAllegianceSame(chunk) || friendlyFire))
		{
			DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
		}
		FinishAbility();
	}
}
