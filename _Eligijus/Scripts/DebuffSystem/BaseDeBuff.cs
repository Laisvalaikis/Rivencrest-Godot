using Godot;
using System;

public partial class BaseDeBuff : Resource
{
	public BaseDeBuff()
	{
			
	}

	public BaseDeBuff(BaseDeBuff blessing)
	{
	}

	public virtual BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		throw new NotImplementedException();
	}

	public virtual BaseBlessing CreateNewInstance()
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
}
