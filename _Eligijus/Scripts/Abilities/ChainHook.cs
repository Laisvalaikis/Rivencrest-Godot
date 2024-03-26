using System;
using System.Collections.Generic;
using Godot;

public partial class ChainHook : BaseAction
{
	private ChunkData _tileToPullTo;
	private Sprite2D _characterSpriteRenderer;
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
	
	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk); 
		UpdateAbilityButton(); 
		Player character = chunk.GetCurrentPlayer(); 
		if (character != null && character.objectInformation.GetPlayerInformation().GetInformationType() != 
		    typeof(Object))
		{
			if (!IsAllegianceSame(chunk)) 
			{ 
				ModifyBonusDamage(chunk); 
				DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
			} 
			ChunkData chunkToPullTo = TileToPullTo(chunk); 
			GameTileMap.Tilemap.MoveSelectedCharacter(chunkToPullTo, character);
			PlayerAbilityAnimation();
			PlayAnimation("Burgundy2", chunkToPullTo);
			ResetCharacterSpriteRendererAndTilePreview(); 
			DisableDamagePreview(chunk); 
			FinishAbility();
		}
	}

	private int GetMultiplier(Vector2 position)
	{
		Vector2 vector2 = position - _player.GlobalPosition;
		int multiplier = Mathf.Abs((int)vector2.X + (int)vector2.Y) - 1;
		return multiplier/100;
	}

	protected override void ModifyBonusDamage(ChunkData chunk)
	{
		bonusDamage = GetMultiplier(chunk.GetPosition());
	}

	private ChunkData TileToPullTo(ChunkData chunk)
	{
		Vector2 position = _player.GlobalPosition;
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
		PlayerInformation currentPlayerInfo = currentCharacter?.objectInformation.GetPlayerInformation();

		if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
		{
			SetNonHoveredAttackColor(previousChunk);
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
			SetNonHoveredAttackColor(previousChunk);
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

	private void SetHoveredChunkHighlight(ChunkData hoveredChunk, PlayerInformation currentPlayerInfo)
	{
		SetHoveredAttackColor(hoveredChunk);
		if (currentPlayerInfo != null)
		{
			AtlasTexture characterSprite = (AtlasTexture)currentPlayerInfo.objectData.GetPlayerInformationData().characterSprite;
			_tileToPullTo = TileToPullTo(hoveredChunk);
			ModifyBonusDamage(hoveredChunk);
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
