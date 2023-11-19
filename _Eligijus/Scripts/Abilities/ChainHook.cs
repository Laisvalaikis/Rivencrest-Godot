using System;
using System.Collections.Generic;
using Godot;

public partial class ChainHook : BaseAction
{
	private ChunkData _tileToPullTo;
	private Sprite2D _characterSpriteRenderer;
	private int multiplier=0;
	public ChainHook()
	{

	}

	public ChainHook(ChainHook ability) : base(ability)
	{
	}

	public override BaseAction CreateNewInstance(BaseAction action)
	{
		ChainHook ability = new ChainHook((ChainHook)action);
		return ability;
	}

	protected override void GeneratePlusPattern(ChunkData centerChunk, int length)
	{
		(int centerX, int centerY) = centerChunk.GetIndexes();
		ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
		bool[] canExtend = { true, true, true, true };

		for (int i = 1; i <= length; i++)
		{
			List<(int, int, int)> positions = new List<(int, int, int)>
			{
				(centerX, centerY + i, 0), // Up
				(centerX, centerY - i, 1), // Down
				(centerX + i, centerY, 2), // Right
				(centerX - i, centerY, 3) // Left
			};
			foreach (var (x, y, direction) in positions)
			{
				if (!canExtend[direction])
				{
					continue;
				}

				if (x >= 0 && x < chunksArray.GetLength(0) && y >= 0 && y < chunksArray.GetLength(1))
				{
					ChunkData chunk = chunksArray[x, y];
					if (chunk != null && !chunk.TileIsLocked())
					{
						if (chunk.GetInformationType() == InformationType.Object)
						{
							canExtend[direction] = false;
							continue;
						}

						_chunkList.Add(chunk);
						if (chunk.GetCurrentPlayer() != null)
						{
							canExtend[direction] = false;
						}
					}
				}
			}
		}
	}
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		Player character = chunk.GetCurrentPlayer();
		if (character != null && character.playerInformation.GetInformationType() != InformationType.Object)
		{
			UpdateAbilityButton();
			if (!IsAllegianceSame(chunk))
			{
				multiplier = GetMultiplier(chunk.GetPosition());
				if (multiplier != 0)
				{
					DealRandomDamageToTarget(chunk, minAttackDamage + multiplier, maxAttackDamage + multiplier);
				}
			}

			ChunkData chunkToPullTo = TileToPullTo(chunk);
			GameTileMap.Tilemap.MoveSelectedCharacter(chunkToPullTo.GetPosition(), new Vector2(0, 0), character);
			ResetCharacterSpriteRendererAndTilePreview();
			DisableDamagePreview(chunk);
			FinishAbility();
		}
	}

	private int GetMultiplier(Vector2 position)
	{
		Vector2 vector2 = position - player.GlobalPosition;
		int multiplier = Mathf.Abs((int)vector2.X + (int)vector2.Y) - 1;
		return multiplier/100;
	}

	private ChunkData TileToPullTo(ChunkData chunk)
	{
		Vector2 position = player.GlobalPosition;
		ChunkData currentPlayerChunk = GameTileMap.Tilemap.GetChunk(position);
		(int chunkX, int chunkY) = chunk.GetIndexes();
		(int playerX, int playerY) = currentPlayerChunk.GetIndexes();

		int deltaX = chunkX - playerX;
		int deltaY = chunkY - playerY;

		// Determine the direction from the player to the chunk
		int directionX = deltaX != 0 ? deltaX / Math.Abs(deltaX) : 0;
		int directionY = deltaY != 0 ? deltaY / Math.Abs(deltaY) : 0;
		
		// Get the chunk next to the player in the determined direction
		ChunkData targetChunk = GameTileMap.Tilemap.GetChunkDataByIndex(playerX+directionX,playerY+directionY);

		return targetChunk;
	}
	
	public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
	{
		HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
		HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

		Player currentCharacter = hoveredChunk?.GetCurrentPlayer();
		PlayerInformation currentPlayerInfo = currentCharacter?.playerInformation;

		if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
		{
			previousChunkHighlight.SetHighlightColor(abilityHighlight);
			ResetCharacterSpriteRendererAndTilePreview();
			DisableDamagePreview(previousChunk);
		}
		if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
		{
			return;
		}
		if (hoveredChunkHighlight.isHighlighted && currentCharacter != null)
		{
			SetHoveredChunkHighlight(hoveredChunk, currentPlayerInfo);
		}
		if (previousChunkHighlight != null)
		{
			if (_tileToPullTo != null && currentCharacter == null)
			{
				_tileToPullTo.GetTileHighlight().TogglePreviewSprite(false);
				if(_characterSpriteRenderer != null)
				{
					_characterSpriteRenderer.SelfModulate = new Color(1f, 1f, 1f, 1f);
				}
			}
			DisableDamagePreview(previousChunk);
			previousChunkHighlight.SetHighlightColor(abilityHighlight);
		}
	}

	private void ResetCharacterSpriteRendererAndTilePreview()
	{
		if (_tileToPullTo != null)
		{
			if (_characterSpriteRenderer != null)
			{
				_characterSpriteRenderer.SelfModulate = new Color(1f, 1f, 1f, 1f);
			}
			_tileToPullTo.GetTileHighlight().TogglePreviewSprite(false);
		}
	}
	
	public override void EnableDamagePreview(ChunkData chunk, string text = null)
	{
		HighlightTile highlightTile = chunk.GetTileHighlight();
		if (customText != null)
		{
			highlightTile.SetDamageText(customText);
		}
		else
		{
			if (maxAttackDamage == minAttackDamage)
			{
				highlightTile.SetDamageText((maxAttackDamage+multiplier).ToString());
			}
			else
			{
				highlightTile.SetDamageText($"{minAttackDamage+multiplier}-{maxAttackDamage+multiplier}");
			}

			if (chunk.GetCurrentPlayer()!=null && chunk.GetCurrentPlayer().playerInformation.GetHealth() <= minAttackDamage)
			{
				highlightTile.ActivateDeathSkull(true);
			}
		}
	}

	private void SetHoveredChunkHighlight(ChunkData hoveredChunk, PlayerInformation currentPlayerInfo)
	{
		SetHoveredAttackColor(hoveredChunk);
		if (currentPlayerInfo != null)
		{
			AtlasTexture characterSprite = (AtlasTexture)currentPlayerInfo.objectData.GetPlayerInformationData().characterSprite;
			_tileToPullTo = TileToPullTo(hoveredChunk);
			multiplier = GetMultiplier(hoveredChunk.GetPosition());
			if (!IsAllegianceSame(hoveredChunk))
			{
				EnableDamagePreview(hoveredChunk);
			}
			HighlightTile tileToPullToHighlight = _tileToPullTo.GetTileHighlight();
			tileToPullToHighlight.TogglePreviewSprite(true);
			tileToPullToHighlight.SetPreviewSprite(characterSprite);
			_characterSpriteRenderer = currentPlayerInfo.spriteRenderer;
			_characterSpriteRenderer.SelfModulate = new Color(1f, 1f, 1f, 0.5f);
		}
	}
}
