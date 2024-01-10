using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class FreezingTouch : AbilityBlessing
{
	//[Export] private int slowEnemyBy;
	
	public FreezingTouch()
	{
			
	}
	
	public FreezingTouch(FreezingTouch blessing): base(blessing)
	{
		
	}
	
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		FreezingTouch blessing = new FreezingTouch((FreezingTouch)baseBlessing);
		return blessing;
	}
	
	public override BaseBlessing CreateNewInstance()
	{
		FreezingTouch blessing = new FreezingTouch(this);
		return blessing;
	}
	
	// public override void OnTurnStart(BaseAction baseAction)
	// {
	// 	base.OnTurnStart(baseAction);
	// 	Player player = chunkData.GetCurrentPlayer();
	// 	SlowDebuff debuff = new SlowDebuff(2, 2);
	// 	player.debuffManager.AddDebuff(debuff,player);
	// }
	public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(baseAction);
		Player player = tile.GetCurrentPlayer();
		SlowDebuff debuff = new SlowDebuff(2, 2);
		player.debuffManager.AddDebuff(debuff,player);
	}
	
}
