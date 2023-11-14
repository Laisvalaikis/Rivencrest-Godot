using Godot;
using System;

public partial class PlayerStun : BaseDeBuff
{
	
	public PlayerStun()
	{
			
	}
    
	public PlayerStun(PlayerStun deBuff): base(deBuff)
	{
        
	}
    
	public override BaseDeBuff CreateNewInstance(BaseDeBuff baseDeBuff)
	{
		PlayerStun blessing = new PlayerStun((PlayerStun)baseDeBuff);
		return blessing;
	}
    
	public override BaseDeBuff CreateNewInstance()
	{
		PlayerStun blessing = new PlayerStun(this);
		return blessing;
	}
	
	public override void Start()
	{

	}
    
	public override void ResolveDeBuff(ChunkData chunkData)
	{
        
	}
    
	public override void OnTurnStart()
	{
			
	}

	public override void OnTurnEnd()
	{
        
	}
	
	public override void PlayerWasAttacked()
	{
			
	}
	
	public override void PlayerDied()
	{
			
	}
}
