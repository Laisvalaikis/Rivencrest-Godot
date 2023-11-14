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
		base.OnTurnStart();
		_player.SetMovementPoints(0);
	}

	public override void OnTurnEnd()
	{
        base.OnTurnEnd();
	}
	
	public override void PlayerWasAttacked()
	{
		base.PlayerWasAttacked();
	}
	
	public override void PlayerDied()
	{
		base.PlayerDied();
	}
}
