using Godot;
using System;

public partial class BaseDebuff : Resource
{
	
	[Export]
	protected int lifetime = 1;
	protected int lifetimeCount = 0;
	protected Player _player;
	protected Player playerWhoCreatedDebuff;
	public BaseDebuff()
	{
			
	}

	public BaseDebuff(BaseDebuff blessing)
	{
		lifetime = blessing.lifetime;
	}

	public virtual BaseDebuff CreateNewInstance(BaseDebuff baseBlessing)
	{
		throw new NotImplementedException();
	}

	public virtual BaseDebuff CreateNewInstance()
	{
		throw new NotImplementedException();
	}
	
	public virtual void Start()
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

	public void SetLifetime(int lifetimeTurns)
	{
		lifetime = lifetimeTurns;
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

	public void SetPlayerWhoCreatedDebuff(Player player)
	{
		playerWhoCreatedDebuff = player;
	}

	public override bool Equals(object obj)
	{
		return GetType().Equals(obj);
	}
	
	public bool EqualsType(Type obj)
	{
		return GetType() == obj;
	}

	public override int GetHashCode()
	{
		return GetHashCode();
	}
}
