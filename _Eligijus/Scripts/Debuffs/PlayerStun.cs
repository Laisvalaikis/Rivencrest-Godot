using Godot;
using System;

//Player cannot move at all, but can use abilities?
//NEEDS FIXING
public partial class PlayerStun : BaseDebuff
{
	
	public PlayerStun()
	{
			
	}
    
	public PlayerStun(PlayerStun debuff): base(debuff)
	{
        
	}
    
	public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
	{
		PlayerStun debuff = new PlayerStun((PlayerStun)baseDebuff);
		return debuff;
	}
    
	public override BaseDebuff CreateNewInstance()
	{
		PlayerStun debuff = new PlayerStun(this);
		return debuff;
	}
	
	public override void Start()
	{

	}
	
    
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		_player.SetMovementPoints(0);
		_player.actionManager.SetAbilityPoints(0);
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
