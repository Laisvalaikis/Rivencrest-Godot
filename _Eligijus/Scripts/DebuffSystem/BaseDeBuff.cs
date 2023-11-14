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
}
