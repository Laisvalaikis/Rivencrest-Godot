using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public partial class AcidRain : BaseAction
{	
	public AcidRain()
	{
	}
 	public AcidRain(AcidRain acidRain): base(acidRain)
 	{
		
 	}
 	
 	public override BaseAction CreateNewInstance(BaseAction action)
 	{
 		AcidRain acidRain = new AcidRain((AcidRain)action);
 		return acidRain;
 	}
	public override void Start()
	{
		base.Start();
		customText = "POISON";
	}

	public override void ResolveAbility(ChunkData chunk)
	{
		if (CanTileBeClicked(chunk))
		{
			base.ResolveAbility(chunk);
			foreach (ChunkData tile in _chunkList)
			{
				if (CanTileBeClicked(tile))
				{
					tile.GetCurrentPlayer().AddPoison(new Poison(tile, 2, 2));
				}
			}
			FinishAbility();
		}
	}
}
