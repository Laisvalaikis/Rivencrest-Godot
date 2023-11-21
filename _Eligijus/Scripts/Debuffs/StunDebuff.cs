using Godot;
using System;

//Player cannot move at all, but can use abilities?
public partial class StunDebuff : BaseDebuff
{
	
	public StunDebuff()
	{
			
	}
    
	public StunDebuff(StunDebuff debuff): base(debuff)
	{
        
	}
    
	public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
	{
		StunDebuff debuff = new StunDebuff((StunDebuff)baseDebuff);
		return debuff;
	}
    
	public override BaseDebuff CreateNewInstance()
	{
		StunDebuff debuff = new StunDebuff(this);
		return debuff;
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
		lifetimeCount++;
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
