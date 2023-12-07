using System;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BaseBuff : Resource
{
	[Export]
	protected int lifetime = 1;
	protected int lifetimeCount = 0;
	protected Player _player;
		
	public BaseBuff()
	{
				
	}
	
	public BaseBuff(BaseBuff buff)
	{ 
		lifetime = buff.lifetime;
	}
	
	public virtual BaseBuff CreateNewInstance(BaseBuff baseBuff)
	{
		throw new NotImplementedException();
	}
	
	public virtual BaseBuff CreateNewInstance()
	{
		throw new NotImplementedException();
	}
		
	public virtual void Start()
	{
	
	}
		
	public virtual void ResolveBuff(ChunkData chunkData)
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
	public void ModifyDamage(ref int damage)
	{
		
	}

	public void ModifyMovement(ref int movementPoints)
	{
		
	}

	public void AddDebuff(BaseDebuff debuff)
	{
		if(debuff.GetType()==typeof(PoisonDebuff))
			debuff.SetLifetime(debuff.GetLifetime()/2);
	}
}
