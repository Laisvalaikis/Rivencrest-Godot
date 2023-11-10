using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.Abilities;

public partial class SilenceBeam : BaseAction
{
	[Export] private int _additionalDamage = 2;
	private ChunkData[,] _chunkArray;
	private int _index = -1;
	public SilenceBeam()
	{
	
	}
	
	public SilenceBeam(SilenceBeam action) : base(action)
	{
		_additionalDamage = action._additionalDamage;
	}
		
	public override BaseAction CreateNewInstance(BaseAction action)
	{ 
		SilenceBeam silenceBeam = new SilenceBeam((SilenceBeam)action); 
		return silenceBeam;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (_index != -1)
		{
			for (int i = 0; i < _chunkArray.GetLength(1); i++)
			{
				ChunkData damageChunk = _chunkArray[_index, i];
				if (damageChunk.CharacterIsOnTile())
				{
					Player target = damageChunk.GetCurrentPlayer();
					target.debuffs.SetTurnCounterFromThisTurn(1);
					target.debuffs.SilencePlayer();
				}
				DealRandomDamageToTarget(damageChunk, minAttackDamage + _additionalDamage, maxAttackDamage + _additionalDamage);
			}
			_chunkArray = new ChunkData[4, attackRange];
		}
		
	}

	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		_index = FindChunkIndex(chunk);
		if (_index != -1)
		{
			for (int i = 0; i < _chunkArray.GetLength(1); i++)
			{
				ChunkData damageChunk = _chunkArray[_index, i];
				DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
			}
			FinishAbility();
		}
	}
	

	
	private int FindChunkIndex(ChunkData chunkData)
	{
		int index = -1;
		for (int i = 0; i < _chunkArray.GetLength(1); i++)
		{
			if (_chunkArray[0,i] != null && _chunkArray[0,i] == chunkData)
			{
				index = 0;
			}
			if(_chunkArray[1,i] != null && _chunkArray[1,i] == chunkData)
			{
				index = 1;
			}
			if (_chunkArray[2,i] != null && _chunkArray[2,i] == chunkData)
			{
				index = 2;
			}
			if (_chunkArray[3,i] != null && _chunkArray[3,i] == chunkData)
			{
				index = 3;
			}
		}
		return index;
	}

	public override bool CanTileBeClicked(ChunkData chunkData)
	{
		return true;
		
	}

	private int _globalIndex = -1;
	public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
	{
		if (hoveredChunk == previousChunk) return;
		if (_globalIndex != -1)
		{
			for (int i = 0; i < _chunkArray.GetLength(1); i++)
			{
				ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
				if (chunkToHighLight != null)
				{
					SetNonHoveredAttackColor(chunkToHighLight);
					DisableDamagePreview(chunkToHighLight);
				}
			}
		}
		if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
		{
			_globalIndex = FindChunkIndex(hoveredChunk);
			if (_globalIndex != -1)
			{
				for (int i = 0; i < _chunkArray.GetLength(1); i++)
				{
					ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
					if (chunkToHighLight.GetCurrentPlayer()!=null)
					{
						SetHoveredAttackColor(chunkToHighLight);
						EnableDamagePreview(chunkToHighLight);
					}
				}
			}
		}
	}
	// public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
	// {
	// 	if (hoveredChunk == previousChunk) return;
	// 	if (_globalIndex != -1)
	// 	{
	// 		for (int i = 0; i < _chunkArray.GetLength(1); i++)
	// 		{
	// 			ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
	// 			if (chunkToHighLight != null)
	// 			{
	// 				SetNonHoveredAttackColor(chunkToHighLight);
	// 				DisableDamagePreview(chunkToHighLight);
	// 			}
	// 		}
	// 	}
	// 	if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
	// 	{
	// 		_globalIndex = FindChunkIndex(hoveredChunk);
	// 		if (_globalIndex != -1)
	// 		{
	// 			for (int i = 0; i < _chunkArray.GetLength(1); i++)
	// 			{
	// 				ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
	// 				if (chunkToHighLight != null)
	// 				{
	// 					SetHoveredAttackColor(chunkToHighLight);
	// 					if (chunkToHighLight.GetCurrentPlayer()!=null)
	// 					{
	// 						EnableDamagePreview(chunkToHighLight);
	// 					}
	// 				}
	// 			}
	// 		}
	// 	}
	// }

	public override void CreateAvailableChunkList(int range)
	{
		ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
		(int centerX, int centerY) = centerChunk.GetIndexes();
		_chunkList.Clear();
		_chunkArray = new ChunkData[4,range];

		int start = 1;
		for (int i = 0; i < range; i++) 
		{
			if (GameTileMap.Tilemap.CheckBounds(centerX + i + start, centerY))
			{
				ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX + i + start, centerY);
				_chunkList.Add(chunkData);
				_chunkArray[0, i] = chunkData;
			}
			if (GameTileMap.Tilemap.CheckBounds(centerX - i - start, centerY))
			{
				ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX-i - start, centerY);
				_chunkList.Add(chunkData);
				_chunkArray[1, i] = chunkData;
			}
			if (GameTileMap.Tilemap.CheckBounds(centerX, centerY + i + start))
			{
				ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY + i + start);
				_chunkList.Add(chunkData);
				_chunkArray[2, i] = chunkData;
			}
			if (GameTileMap.Tilemap.CheckBounds(centerX, centerY - i - start))
			{
				ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(centerX, centerY - i - start);
				_chunkList.Add(chunkData);
				_chunkArray[3, i] = chunkData;
			}
		}
	}
}
