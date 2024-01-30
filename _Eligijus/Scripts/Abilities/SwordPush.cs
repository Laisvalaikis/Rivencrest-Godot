using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class SwordPush : BaseAction
{
    [Export]
    public int pushDamage = 15;
    [Export]
    public int centerDamage = 30;
    private List<ChunkData> _attackTiles;
    private List<HighlightTile> _arrowTiles;
    private List<ChunkData> _text;
    private ChunkData _adjacent;

    public SwordPush()
    {
    }

    public SwordPush(SwordPush ability): base(ability)
    {
        pushDamage = ability.pushDamage;
        centerDamage = ability.centerDamage;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        SwordPush ability = new SwordPush((SwordPush)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        Side _side = Side.none;
        CreateAttackGrid(chunk);
        for (int i = 0; i < _attackTiles.Count; i++)
        {
            if (i > 0)
            {
                int damage = pushDamage;
                DealDamage(_attackTiles[i], damage);
                _side = ChunkSideByCharacter(chunk, _attackTiles[i]);
                (int x, int y) sideVector = GetSideVector(_side);
                MovePlayerToSide(_attackTiles[i], sideVector);
                ClearArrows();
            }
            else
            {
                CheckAdjacent(chunk);
                int damage = centerDamage;
                DealDamage(_attackTiles[i], damage);
                // ClearArrows();
                if (_adjacent != null)
                {
                   // SilenceDebuff debuff = new SilenceDebuff(1);
                    //chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);
                  //  _side = ChunkSideByCharacter(chunk, _adjacent);
                   // (int x, int y) sideVector = GetSideVector(_side);
                  //  MovePlayerToSide(_attackTiles[i], sideVector);
                    //ClearArrows();
                }
            }
        }
        chunk.GetTileHighlight().SetSideArrowsSprite(0);
        chunk.GetTileHighlight().ActivateSideArrows(false);
       // ClearArrows();
        //ClearText();
        FinishAbility();
    }

    private void CheckAdjacent(ChunkData tile)
    {
        (int x, int y) = tile.GetIndexes();
		
        var directionVectors = new List<(int x, int y)>
        {
            (1 + x, 0 + y),
            (0 + x, 1 + y),
            (-1 + x, 0 + y),
            (0 + x, -1 + y)
        };
        _adjacent = null;

        foreach (var directions in directionVectors)
        {
            if (GameTileMap.Tilemap.CheckBounds(directions.x, directions.y))
            {

                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(directions.x, directions.y);
                if (!chunkData.CharacterIsOnTile())
                {
                    _adjacent = chunkData;
                    break;
                }
            }
        }
    }

    private void CreateAttackGrid(ChunkData selected)
    {
        (int x, int y) tileIndex = selected.GetIndexes();
        List<(int, int)> positionIndexes = new List<(int, int)> 
        {
            (tileIndex.x, tileIndex.y + 1),  // Up
            (tileIndex.x, tileIndex.y - 1),  // Down
            (tileIndex.x + 1, tileIndex.y),  // Right
            (tileIndex.x - 1, tileIndex.y)   // Left
        };
        if (_attackTiles == null)
        {
            _attackTiles = new List<ChunkData>();
        }
        else
        {
            _attackTiles.Clear();
        }
        _attackTiles.Add(selected); // added center chunk;
        foreach (var indexes in positionIndexes)
        {
            if (GameTileMap.Tilemap.CheckBounds(indexes.Item1, indexes.Item2))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(indexes.Item1, indexes.Item2);
                _attackTiles.Add(chunkData);
            }
        }
    }
    
    
    public override void CreateAvailableChunkList(int attackRange)
    {
        base.CreateAvailableChunkList(base.attackRange);
        _chunkList.Add(GameTileMap.Tilemap.GetChunk(_player.GlobalPosition));
    }
    
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        
        if (hoveredChunk == previousChunk) return;
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            CreateAttackGrid(hoveredChunk);
            if (previousChunk != null && previousChunk.GetTileHighlight().isHighlighted)
            {
                previousChunk.GetTileHighlight().ActivateSideArrows(false);
                ClearArrows();
                ClearText();
            }
            
            for (int i = 0; i < _attackTiles.Count; i++)
            {
                HighlightSwordPush(i, hoveredChunk);
            }
            
            hoveredChunk.GetTileHighlight().ActivateSideArrows(true);
            if (hoveredChunk.CharacterIsOnTile())
            {
                if (_text == null)
                {
                    _text = new List<ChunkData>();
                }
                _text.Add(hoveredChunk);
                customText = $"-{centerDamage}";
                EnableDamagePreview(hoveredChunk);
                customText = string.Empty;
            }
        }
        else if(hoveredChunk != null && !hoveredChunk.GetTileHighlight().isHighlighted)
        {
            hoveredChunk.GetTileHighlight().ActivateSideArrows(false);
            if (previousChunk != null && previousChunk.GetTileHighlight().isHighlighted)
            {
                previousChunk.GetTileHighlight().ActivateSideArrows(false);
            }
            ClearArrows();
            ClearText();
        }
        else if (previousChunk != null && previousChunk.GetTileHighlight().isHighlighted)
        {
            previousChunk.GetTileHighlight().ActivateSideArrows(false);
            ClearArrows();
            ClearText();
        }
    }
    
    private void HighlightSwordPush(int index, ChunkData hoveredChunk)
    {
        if (_arrowTiles == null)
        {
            _arrowTiles = new List<HighlightTile>();
        }

        if (index == 0)
        {
            CheckAdjacent(hoveredChunk);
            if (_adjacent != null)
            {
                (int x, int y) indexes = _attackTiles[index].GetIndexes();
                Side side = ChunkSideByCharacter(hoveredChunk, _adjacent);
                (int x, int y) sideVector = GetSideVector(side);
                (int, int) tempIndexes = new(indexes.x + sideVector.Item1, indexes.y + sideVector.Item2);
                if (GameTileMap.Tilemap.CheckBounds(tempIndexes.Item1, tempIndexes.Item2))
                {
                    ChunkData tempTile = GameTileMap.Tilemap.GetChunkDataByIndex(tempIndexes.Item1, tempIndexes.Item2);
                    if (_attackTiles[index].CharacterIsOnTile())
                    {
                        int arrowType = DetermineArrowType(tempTile, _attackTiles[index]);
                        tempTile.GetTileHighlight().SetArrowSprite(arrowType);
                        customText = $"-{pushDamage}";
                        EnableDamagePreview(_attackTiles[index]);
                        customText = string.Empty;
                        int sideArrowsType = DetermineSideArrowsType(tempTile, _attackTiles[index]);
                        _arrowTiles.Add(tempTile.GetTileHighlight());
                        hoveredChunk.GetTileHighlight().SetSideArrowsSprite(sideArrowsType);
                    }
                }
            }
            else
            {
                hoveredChunk.GetTileHighlight().SetSideArrowsSprite(0);
            }
        }
        else
        {
            Highlight(index, hoveredChunk);
        }
    }

    private void Highlight(int index, ChunkData hoveredChunk)
    {
        if (_text == null)
        {
            _text = new List<ChunkData>();
        }
        (int x, int y) indexes = _attackTiles[index].GetIndexes();
        Side side = ChunkSideByCharacter(hoveredChunk, _attackTiles[index]);
        (int x, int y) sideVector = GetSideVector(side);
        (int, int) tempIndexes = new(indexes.x + sideVector.Item1, indexes.y + sideVector.Item2);
        if (GameTileMap.Tilemap.CheckBounds(tempIndexes.Item1, tempIndexes.Item2))
        {
            ChunkData tempTile = GameTileMap.Tilemap.GetChunkDataByIndex(tempIndexes.Item1, tempIndexes.Item2);
            if (_attackTiles[index].CharacterIsOnTile())
            {
                customText = $"-{pushDamage}";
                EnableDamagePreview(_attackTiles[index]);
                customText = string.Empty;
                _text.Add(_attackTiles[index]);
                // tempTile.GetTileHighlight().SetHighlightColor(abilityHoverCharacter);
                if (!tempTile.CharacterIsOnTile())
                {
                    int arrowType = DetermineArrowType(tempTile, _attackTiles[index]);
                    tempTile.GetTileHighlight().SetArrowSprite(arrowType);
                    _arrowTiles.Add(tempTile.GetTileHighlight());
                }
            }
        }
        else
        {
            if (_attackTiles[index].CharacterIsOnTile())
            {
                customText = $"-{pushDamage}";
                EnableDamagePreview(_attackTiles[index]);
                customText = string.Empty;
                _text.Add(_attackTiles[index]);
            }
        }
    }

    private void ClearArrows()
    {
        if (_arrowTiles != null)
        {
            for (int i = 0; i < _arrowTiles.Count; i++)
            {
                _arrowTiles[i].DeactivateArrowTile();
            }
            _arrowTiles.Clear();
        }
    }

    private void ClearText()
    {
        if (_text != null)
        {
            for (int i = 0; i < _text.Count; i++)
            {
                DisableDamagePreview(_text[i]);
            }

            _text.Clear();
        }
    }

    private int DetermineArrowType(ChunkData current, ChunkData prev)
    {
        if (prev == null) return 0;  // Invalid case

        var (cx, cy) = current.GetIndexes();
        var (px, py) = prev?.GetIndexes() ?? (0, 0);
        
        if (cx > px) return 5;  // Right End
        if (cx < px) return 6;  // Left End
        if (cy > py) return 7;  // Down End
        if (cy < py) return 8;  // Up End
        
        return 0;  
    }
    
    private int DetermineSideArrowsType(ChunkData current, ChunkData prev)
    {
        if (prev == null) return 0;  // Invalid case

        var (cx, cy) = current.GetIndexes();
        var (px, py) = prev?.GetIndexes() ?? (0, 0);
        
        if (cx > px) return 1;  // Right End
        if (cx < px) return 2;  // Left End
        if (cy > py) return 3;  // Down End
        if (cy < py) return 4;  // Up End
        
        return 0;  
    }

    
}
