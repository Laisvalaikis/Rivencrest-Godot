using System;
using Godot;

public partial class UsedAbility : Resource
{
	public BaseAction ability { get; }
	public ChunkData chunk { get; }
	public Player abilityPLayer { get; }
	private int _turnLifetime ;
	private int _turnsSinceCast = 0;
	
	public UsedAbility(BaseAction ability, ChunkData chunk, Player abilityPLayer)
	{
		this.ability = ability;
		this.chunk = chunk;
		this.abilityPLayer = abilityPLayer;
		_turnLifetime = 1;
		
	}

	public UsedAbility(BaseAction ability, ChunkData chunk, Player abilityPLayer, int turnLifetime)
	{
		this.ability = ability;
		this.chunk = chunk;
		this.abilityPLayer = abilityPLayer;
		_turnLifetime = turnLifetime;
	}

	public UsedAbility(UsedAbility usedAbility)
	{
		ability = usedAbility.ability;
		chunk = usedAbility.chunk;
		_turnLifetime = usedAbility._turnLifetime;
		_turnsSinceCast = usedAbility._turnsSinceCast;
	}
	
	public UsedAbility(UsedAbility usedAbility, int turnLifetime)
	{
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
