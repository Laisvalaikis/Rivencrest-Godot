using Godot;
using System;

public partial class BaseDeBuff : Resource
{
	
	[Export]
	protected int lifetime = 1;
	protected int lifetimeCount = 0;
	protected Player _player;
	public BaseDeBuff()
	{
			
	}

	public BaseDeBuff(BaseDeBuff blessing)
	{
	}

	public virtual BaseDeBuff CreateNewInstance(BaseDeBuff baseBlessing)
	{
		throw new NotImplementedException();
	}

	public virtual BaseDeBuff CreateNewInstance()
	{
		throw new NotImplementedException();
	}
	
	public virtual void Start()
	{

	}
    
	public virtual void ResolveDeBuff(ChunkData chunkData)
	{
        
	}
    
	public virtual void OnTurnStart()
	{
			
	}

	public virtual void OnTurnEnd()
	{
		
	}
	
	public virtual void PlayerWasAttacked()
	{
			
	}
	
	public virtual void PlayerDied()
	{
			
	}

	public int GetLifetime()
	{
		return lifetime;
	}

	public int GetLifetimeCounter()
	{
		return lifetimeCount;
	}

	public void IncreaseLifetimeCount(int count)
	{
		lifetimeCount += count;
	}

	public void ResetLifetimeCount()
	{
		lifetimeCount = 0;
	}

	public void SetPLayer(Player player)
	{
		_player = player;
	}
}
