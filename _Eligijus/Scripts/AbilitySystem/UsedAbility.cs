using System;
using Godot;

public partial class UsedAbility : Resource
{
	public BaseAction Ability { get; set; }
	public ChunkData Chunk { get; set; }
	
	private int _turnsSinceCastAfterResolve = 0;
	private int _turnsSinceCastBeforeStart = 0;

	public UsedAbility(BaseAction ability, ChunkData chunk)
	{
		Ability = ability;
		Chunk = chunk;
	}
	
	public void IncreaseCastCountAfterResolve()
	{
		_turnsSinceCastAfterResolve++;
	}

	public void ResetCastCountAfterResolve()
	{
		_turnsSinceCastAfterResolve = 0;
	}
		
	public int GetCastCountAfterResolve()
	{
		return _turnsSinceCastAfterResolve;
	}

	public void IncreaseCastCountBeforeTurn()
	{
		_turnsSinceCastBeforeStart++;
	}
		
	public void ResetCastCountBeforeTurn()
	{
		_turnsSinceCastBeforeStart = 0;
	}

	public int GetCastCountBeforeStart()
	{
		return _turnsSinceCastBeforeStart;
	}

}
