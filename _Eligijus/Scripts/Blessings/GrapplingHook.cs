using System;
using Godot;

public partial class GrapplingHook : AbilityBlessing
{
    
    private ChunkData _tileToPullTo;
    private Sprite2D _characterSpriteRenderer;
    
    public GrapplingHook()
    {
			
    }
    
    public GrapplingHook(GrapplingHook blessing): base(blessing)
    {
 
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        GrapplingHook blessing = new GrapplingHook((GrapplingHook)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        GrapplingHook blessing = new GrapplingHook(this);
        return blessing;
    }
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (baseAction.GetPlayer() != null)
        {
            ChunkData chunkToPullTo = TileToPullTo(tile, baseAction.GetPlayer());
            if (chunkToPullTo != null)
            {
                GameTileMap.Tilemap.MoveSelectedCharacter(chunkToPullTo.GetPosition(), new Vector2(0, 0),
                    baseAction.GetPlayer());
                // ResetCharacterSpriteRendererAndTilePreview();
                // FinishAbility();
            }
        }
    }
    
    public override void OnMoveHover(BaseAction baseAction, ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

        Player currentCharacter = hoveredChunk?.GetCurrentPlayer();
        PlayerInformation currentPlayerInfo = currentCharacter?.playerInformation;

        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
        {
            previousChunkHighlight.SetHighlightColor(baseAction.GetAbilityHighlightColor());
            ResetCharacterSpriteRendererAndTilePreview();
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted && currentCharacter != null)
        {
            SetHoveredChunkHighlight(baseAction, hoveredChunk, currentPlayerInfo);
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
            previousChunkHighlight.SetHighlightColor(baseAction.GetAbilityHighlightColor());
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
    
    private void SetHoveredChunkHighlight(BaseAction baseAction, ChunkData hoveredChunk, PlayerInformation currentPlayerInfo)
    {
        baseAction.SetHoveredAttackColor(hoveredChunk);
        baseAction.EnableDamagePreview(hoveredChunk);

        if (currentPlayerInfo != null)
        {
            AtlasTexture characterSprite = (AtlasTexture)currentPlayerInfo.objectData.GetPlayerInformationData().characterSprite;
            _tileToPullTo = TileToPullTo(hoveredChunk, baseAction.GetPlayer());
            HighlightTile tileToPullToHighlight = _tileToPullTo.GetTileHighlight();
            tileToPullToHighlight.TogglePreviewSprite(true);
            tileToPullToHighlight.SetPreviewSprite(characterSprite);
            _characterSpriteRenderer = currentPlayerInfo.spriteRenderer;
            _characterSpriteRenderer.SelfModulate = new Color(1f, 1f, 1f, 0.5f);
        }
    }
    
    private ChunkData TileToPullTo(ChunkData chunk, Player player)
    {
        Vector2 position = player.GlobalPosition;
        ChunkData currentPlayerChunk = GameTileMap.Tilemap.GetChunk(position);
        (int chunkX, int chunkY) = chunk.GetIndexes();
        (int playerX, int playerY) = currentPlayerChunk.GetIndexes();

        int deltaX = chunkX + playerX;
        int deltaY = chunkY - playerY;

        // Determine the direction from the player to the chunk
        int directionX = deltaX != 0 ? deltaX / Math.Abs(deltaX) : 0;
        int directionY = deltaY != 0 ? deltaY / Math.Abs(deltaY) : 0;

        if (deltaY == 1 || deltaY == -1)
        {
            directionY = 0;
        }

        if (deltaX == 1 || deltaX == -1)
        {
            directionX = 0;
        }

        if (GameTileMap.Tilemap.CheckIfWall(chunkX + directionX, chunkY + directionY))
        {
            return GameTileMap.Tilemap.GetChunkDataByIndex(chunkX + directionX, chunkY + directionY);
        }

        return null;
    }

    
}