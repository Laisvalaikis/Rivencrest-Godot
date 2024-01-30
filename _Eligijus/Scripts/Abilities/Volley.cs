using System;
using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Volley : BaseAction
{
    [Export] private int spellDamage = 6;
    private ChunkData _tileToPullTo;
    private Sprite2D _characterSpriteRenderer;


    public Volley()
    {
    }

    public Volley(Volley ability) : base(ability)
    {
        spellDamage = ability.spellDamage;
        customText = ability.customText;
        customText = $"-{spellDamage}";
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Volley ability = new Volley((Volley)action);
        return ability;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        int index = FindChunkIndex(chunk);
        if (index != -1)
        {
            UpdateAbilityButton();
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                ChunkData damageChunk = _chunkArray[index, i];
                DealDamage(damageChunk, spellDamage);
                PoisonDebuff debuff = new PoisonDebuff(2, 2);
                chunk.GetCurrentPlayer()?.debuffManager.AddDebuff(debuff, _player);
            }

            GameTileMap.Tilemap.MoveSelectedCharacter(TileToDashTo(index), _player);
            FinishAbility();
            ResetCharacterSpriteRendererAndTilePreview();
        }
    }

    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;

        ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        PlayerInformation currentPlayerInfo = _player.objectInformation.GetPlayerInformation();
        if (_globalIndex != -1)
        {
            for (int i = 0; i < _chunkArray.GetLength(1); i++)
            {
                ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
                if (chunkToHighLight != null)
                {
                    SetNonHoveredAttackColor(chunkToHighLight);
                    DisableDamagePreview(chunkToHighLight);
                    ResetCharacterSpriteRendererAndTilePreview();
                }
            }
        }

        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            _globalIndex = FindChunkIndex(hoveredChunk);
            if (_globalIndex != -1)
            {
                if (currentPlayerInfo != null)
                {
                    AtlasTexture characterSprite =
                        (AtlasTexture)currentPlayerInfo.objectData.GetPlayerInformationData().characterSprite;
                    _tileToPullTo = TileToDashTo(_globalIndex);
                    if (_tileToPullTo != playerChunk)
                    {
                        HighlightTile tileToPullToHighlight = _tileToPullTo.GetTileHighlight();
                        tileToPullToHighlight.TogglePreviewSprite(true);
                        tileToPullToHighlight.SetPreviewSprite(characterSprite);
                        _characterSpriteRenderer = currentPlayerInfo.spriteRenderer;
                        _characterSpriteRenderer.SelfModulate = new Color(1f, 1f, 1f, 0.5f);
                    }
                }

                for (int i = 0; i < _chunkArray.GetLength(1); i++)
                {
                    ChunkData chunkToHighLight = _chunkArray[_globalIndex, i];
                    if (chunkToHighLight != null)
                    {
                        SetHoveredAttackColor(chunkToHighLight);
                        EnableDamagePreview(chunkToHighLight);
                    }
                }
            }
        }
    }

    private ChunkData TileToDashTo(int index)
    {
        Vector2 playerPosition = _player.GlobalPosition;
        ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(playerPosition);
        (int x, int y) = playerChunk.GetIndexes();
        switch (index)
        {
            case 0:
                y++;
                break;
            case 1:
                y--;
                break;
            case 2:
                x++;
                break;
            case 3:
                x--;
                break;
        }

        if (GameTileMap.Tilemap.CheckBounds(x, y))
        {
            ChunkData tileToDashTo = GameTileMap.Tilemap.GetChunkDataByIndex(x, y);
            if (tileToDashTo.GetCharacterType() == null)
            {
                return tileToDashTo;
            }
        }

        return playerChunk;
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

    public override void CreateAvailableChunkList(int range)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();

        int startRadius = 1;
        int count = startRadius + (attackRange * 2) - 2;
        int topLeftCornerX = centerX - attackRange;
        int topLeftCornerY = centerY - attackRange;
        int bottomRightCornerX = centerX + attackRange;
        int bottomRightCornerY = centerY + attackRange;

        _chunkArray = new ChunkData[4, count];

        const int rowStart = 1;
        for (int i = 0; i < count; i++)
        {
            if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX + i + rowStart, topLeftCornerY))
            {
                ChunkData chunkData =
                    GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX + i + rowStart, topLeftCornerY);
                _chunkList.Add(chunkData);
                _chunkArray[0, i] = chunkData;
            }

            if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX - i - rowStart, bottomRightCornerY))
            {
                ChunkData chunkData =
                    GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX - i - rowStart, bottomRightCornerY);
                _chunkList.Add(chunkData);
                _chunkArray[1, i] = chunkData;
            }

            if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX, topLeftCornerY + i + rowStart))
            {
                ChunkData chunkData =
                    GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY + i + rowStart);
                _chunkList.Add(chunkData);
                _chunkArray[2, i] = chunkData;
            }

            if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX, bottomRightCornerY - i - rowStart))
            {
                ChunkData chunkData =
                    GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY - i - rowStart);
                _chunkList.Add(chunkData);
                _chunkArray[3, i] = chunkData;
            }
        }
    }
}