using System.Collections.Generic;
using System.Diagnostics;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class AcidRain : BaseAction
{
	[Export] private int poisonDamage;
	[Export] private int poisonTurns;
	public AcidRain()
	{
	}
 	public AcidRain(AcidRain acidRain): base(acidRain)
	{
		poisonDamage = acidRain.poisonDamage;
		poisonTurns = acidRain.poisonTurns;
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
	
	public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
	{
		HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
		HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

		if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
		{
			DisableHighlights(hoveredChunk);
		}
		if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
		{
			return;
		}
		if (hoveredChunkHighlight.isHighlighted)
		{
			if (CanTileBeClicked(hoveredChunk))
			{
				foreach (var chunk in _chunkList)
				{
					if (CanTileBeClicked(chunk))
					{
						EnableDamagePreview(chunk);
						SetHoveredAttackColor(chunk);
					}
				}
			}
			SetHoveredAttackColor(hoveredChunk);
		}
		if (previousChunkHighlight != null && !CanTileBeClicked(hoveredChunk))
		{
			DisableHighlights(hoveredChunk);
		}
	}

	private void DisableHighlights(ChunkData hoveredChunk)
	{
		foreach (var chunk in _chunkList)
		{
			if (chunk != hoveredChunk)
			{
				SetNonHoveredAttackColor(chunk);
				DisableDamagePreview(chunk);
			}
		}
	}

	public override void ResolveAbility(ChunkData chunk)
	{
		if (CanTileBeClicked(chunk))
		{
			UpdateAbilityButton(); //this needs fixing
			foreach (ChunkData tile in _chunkList)
			{
				if (CanTileBeClicked(tile))
				{
					Player target = tile.GetCurrentPlayer();
					PoisonDebuff debuff = new PoisonDebuff(2,2);
					target.debuffManager.AddDebuff(debuff,player);
				}
			}
			base.ResolveAbility(chunk);
			FinishAbility();
		}
	}
}
