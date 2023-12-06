using System;
using Godot;

public partial class UsedAbility : Resource
{
	public BaseAction ability { get; }
	public ChunkData chunk { get; }
	private int _turnLifetime ;
	private int _turnsSinceCast = 0;

	public UsedAbility(BaseAction ability)
	{
		this.ability = ability;
		ResourceName = ability.ResourceName;
	}

	public UsedAbility(BaseAction ability, ChunkData chunk)
	{
		ResourceName = ability.ResourceName;
		this.ability = ability;
		this.chunk = chunk;
		_turnLifetime = 1;
		
	}
	
	public UsedAbility(UsedAbility usedAbility, int turnLifetime)
	{
		ResourceName = usedAbility.ResourceName;
		ability = usedAbility.ability;
		chunk = usedAbility.chunk;
		_turnLifetime = turnLifetime;
		_turnsSinceCast = usedAbility._turnsSinceCast;
	}

	public void IncreaseCastCount()
	{
		_turnsSinceCast++;
	}

	public void ResetCastCount()
	{
		_turnsSinceCast = 0;
	}
		
	public int GetCastCount()
	{
		return _turnsSinceCast;
	}
	
	public int GetTurnLifetime()
	{
		return _turnLifetime;
	}

	public override bool Equals(object obj)
	{
		if (obj is not null)
		{
			UsedAbility usedAbility = (UsedAbility)obj;
			return ability.Equals(usedAbility.ability);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return ability.GetHashCode();
	}
}
