using Godot;

public partial class SideSlash : BaseAction
{
    [Export]
    private int spellDamage = 6;
    private ChunkData[,] _chunkArray;

    public SideSlash()
    {
    }

    public SideSlash(SideSlash ability): base(ability)
    {
        spellDamage = ability.spellDamage;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        SideSlash ability = new SideSlash((SideSlash)action);
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
                DealRandomDamageToTarget(damageChunk, minAttackDamage, maxAttackDamage);
            }

            FinishAbility();
        }
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
                    if (chunkToHighLight!=null && chunkToHighLight.GetCurrentPlayer()!=null)
                    {
                        SetHoveredAttackColor(chunkToHighLight);
                        EnableDamagePreview(chunkToHighLight, minAttackDamage, maxAttackDamage);
                    }
                }
            }
        }
    }
    // public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    // {
    //     HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
    //     HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();
    //
    //     if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted)) // nuhoverinome off-grid
    //     {
    //         foreach (var chunk in _chunkList)
    //         {
    //             SetNonHoveredAttackColor(chunk);
    //             DisableDamagePreview(chunk);
    //         }
    //     }
    //     if (hoveredChunkHighlight == null || hoveredChunk == previousChunk) //Hoveriname ant to pacio ar siaip kazkoks gaidys ivyko
    //     {
    //         return;
    //     }
    //     if (hoveredChunkHighlight.isHighlighted) //Jei uzhoverinome ant grido
    //     {
    //         if (CanTileBeClicked(hoveredChunk)) //Ant uzhoverinto langelio characteris
    //         {
    //             foreach (var chunk in _chunkList)
    //             {
    //                 if (CanTileBeClicked(chunk))
    //                 {
    //                     SetHoveredAttackColor(chunk);
    //                     EnableDamagePreview(chunk);
    //                 }
    //             }
    //         }
    //         else //ant uzhoverinto langelio ne characteris
    //         {
    //             hoveredChunkHighlight.SetHighlightColor(abilityHighlightHover);
    //         }
    //     }
    //     if (previousChunkHighlight != null) // Jei pries tai irgi buvome ant grido
    //     {
    //         if (CanTileBeClicked(previousChunk) && !CanTileBeClicked(hoveredChunk)) //Jei ten buvo veikėjas, be to, dabar nebe ant veikėjo esame
    //         {
    //             foreach (var chunk in _chunkList)
    //             {
    //                 SetNonHoveredAttackColor(chunk);
    //                 DisableDamagePreview(chunk);
    //             }
    //         }
    //         else if(!CanTileBeClicked(previousChunk) && !CanTileBeClicked(hoveredChunk)) //Nei buvo veikejas ant praeito, nei yra ant dabartinio
    //         {
    //             SetNonHoveredAttackColor(previousChunk);
    //         }
    //     }
    // }

    private int FindChunkIndex(ChunkData chunkData)
    {
        int index = -1;
        for (int i = 0; i < _chunkArray.GetLength(1); i++)
        {
            if (_chunkArray[0, i] != null && _chunkArray[0, i] == chunkData)
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

    public override void CreateAvailableChunkList(int attackRange)
    {
        ChunkData centerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _chunkList.Clear();

        int startRadius = 1;
        int count = startRadius + (attackRange * 2)-2; // -2
        int topLeftCornerX = centerX - attackRange;
        int topLeftCornerY = centerY - attackRange;
        int bottomRightCornerX = centerX + attackRange;
        int bottomRightCornerY = centerY + attackRange;

        _chunkArray = new ChunkData[4,count];

        int rowStart = 1; // start is 1, because we need ignore corner tile
        for (int i = 0; i < count; i++) 
        {
            if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX + i + rowStart, topLeftCornerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX + i + rowStart, topLeftCornerY);
                _chunkList.Add(chunkData);
                _chunkArray[0, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX - i - rowStart, bottomRightCornerY))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX-i - rowStart, bottomRightCornerY);
                _chunkList.Add(chunkData);
                _chunkArray[1, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX, topLeftCornerY + i + rowStart))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY + i + rowStart);
                _chunkList.Add(chunkData);
                _chunkArray[2, i] = chunkData;
            }
            if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX, bottomRightCornerY - i - rowStart))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY - i - rowStart);
                _chunkList.Add(chunkData);
                _chunkArray[3, i] = chunkData;
            }
        }

    }
}
