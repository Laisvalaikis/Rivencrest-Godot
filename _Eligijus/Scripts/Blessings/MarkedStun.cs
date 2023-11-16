using Godot;

public partial class MarkedStun : AbilityBlessing
{
	public MarkedStun()
	{
			
	}
	
	public MarkedStun(MarkedStun blessing): base(blessing)
	{
 
	}
	
	public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
	{
		MarkedStun blessing = new MarkedStun((MarkedStun)baseBlessing);
		return blessing;
	}
	
	public override BaseBlessing CreateNewInstance()
	{
		MarkedStun blessing = new MarkedStun(this);
		return blessing;
	}

	public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
	{
		base.ResolveBlessing(baseAction, tile);
		if (tile.CharacterIsOnTile())
		{
			Player player = tile.GetCurrentPlayer();
			if (!IsAllegianceSame(baseAction.GetPlayer(), tile, baseAction))
			{
				// Create Can't move
				GD.PrintErr("Fix can't do anything");
			}
		}
	}
}
