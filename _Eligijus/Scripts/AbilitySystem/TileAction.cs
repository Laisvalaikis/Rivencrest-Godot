using System;
using System.Collections.Generic;
using Godot;

public partial class TileAction : Resource
{
    [Export] public int minAttackDamage = 0;
    [Export] public int maxAttackDamage = 0;
    [Export] protected int attackRange = 1;

    [Export] protected bool friendlyFire = false;
    [Export] protected Color abilityHighlight = new Color("#B25E55");
    [Export] protected Color abilityHighlightHover = new Color("#9E4A41");
    [Export] protected Color abilityHoverCharacter = new Color("#FFE300");
    [Export] protected Color characterOnGrid = new Color("#FF5947");
    // Other Team
    [Export] protected Color otherTeamAbilityHighlight = new Color("#5d5d5d");
    [Export] protected Color otherTeamHighlightHover = new Color("#3a3a3a");
    [Export] protected Color otherTeamHoverCharacter = new Color("#CB36D6");
    [Export] protected Color otherTeamCharacterOnGrid = new Color("#141414");
    
    protected List<ChunkData> _chunkList;
    protected List<ChunkData> _visionChunkList;
    protected TurnManager _turnManager;
    protected Player _player;
    
    // Booleans used for generating and highliting grid
    // Also used to determine who you can use ability on
    [Export] protected bool canUseOnSelf;
    [Export] protected bool canUseOnTeammate;
    [Export] protected bool canUseOnEnemy;
    [Export] protected bool canUseOnObject;
    [Export] protected bool areaOfEffect;
    [Export] protected bool laserGridExtendsBeyondFirstEnemy;
    [Export] protected bool laserGrid;
    [Export] protected bool affectsAllEnemiesInGrid;
    [Export] protected bool affectsAllAlliesInGrid;
    [Export] protected bool affectsAllCharactersAtOnce;
    [Export] protected string teamDisplayText = string.Empty;
    [Export] protected string enemyDisplayText = string.Empty;
    //--------------------------------------------------------------
    public TileAction()
    {
        
    }

    public TileAction(TileAction action)
    {
        minAttackDamage = action.minAttackDamage;
        maxAttackDamage = action.maxAttackDamage;
        attackRange = action.attackRange;
        friendlyFire = action.friendlyFire;
        abilityHighlight = action.abilityHighlight;
        abilityHighlightHover = action.abilityHighlightHover;
        abilityHoverCharacter = action.abilityHoverCharacter;
        characterOnGrid = action.characterOnGrid;
        otherTeamAbilityHighlight = action.otherTeamAbilityHighlight;
        otherTeamHighlightHover = action.otherTeamHighlightHover;
        otherTeamHoverCharacter = action.otherTeamHoverCharacter;
        otherTeamCharacterOnGrid = action.otherTeamCharacterOnGrid;
        
        laserGrid = action.laserGrid;
        canUseOnSelf = action.canUseOnSelf;
        canUseOnEnemy = action.canUseOnEnemy;
        canUseOnTeammate = action.canUseOnTeammate;
        canUseOnObject = action.canUseOnObject;
        areaOfEffect = action.areaOfEffect;
        laserGridExtendsBeyondFirstEnemy = action.laserGridExtendsBeyondFirstEnemy;
        affectsAllAlliesInGrid = action.affectsAllAlliesInGrid;
        affectsAllEnemiesInGrid = action.affectsAllEnemiesInGrid;
        affectsAllCharactersAtOnce = action.affectsAllCharactersAtOnce;
    }

    public virtual void Start()
    {
        _chunkList = new List<ChunkData>();
        
    }

    protected virtual void HighlightGridTile(ChunkData chunkData)
    {
        SetNonHoveredAttackColor(chunkData);
        chunkData.GetTileHighlight().EnableTile(true);
        chunkData.GetTileHighlight().ActivateColorGridTile(true);
    }
    
    protected virtual void SetNonHoveredAttackColor(ChunkData chunkData)
    {
        if (_turnManager.GetCurrentTeamIndex() == _player.GetPlayerTeam())
        {
            SetCurrentTeamNonHoverAttackColor(chunkData);
        }
        else
        {
            SetOtherTeamNonHoverAttackColor(chunkData);
        }
    }
    
    protected virtual void SetCurrentTeamNonHoverAttackColor(ChunkData chunkData)
    {
        if (chunkData.GetCurrentPlayer() == null || (chunkData.GetCurrentPlayer() != null && !CanTileBeClicked(chunkData)))
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(abilityHighlight);
        }
        else
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(characterOnGrid);
        }
    }
    
    public virtual bool CanTileBeClicked(ChunkData chunkData)
    {
        if (chunkData.GetTileHighlight().isHighlighted)
        {
            if (CheckIfSpecificInformationType(chunkData, typeof(Player)))
            {
                if (IsAllegianceSame(chunkData) && canUseOnTeammate)
                {
                    return true;
                }
                if (!IsAllegianceSame(chunkData) && canUseOnEnemy)
                {
                    return true;
                }
            }
            if (CheckIfSpecificInformationType(chunkData, typeof(Object)) && canUseOnObject)
            {
                return true;
            }
        }
        return false;
    }
    
    public virtual bool IsAllegianceSame(ChunkData chunk)
    {
        return chunk!=null && chunk.CharacterIsOnTile() && _player != null && chunk.GetCurrentPlayer().GetPlayerTeam() == _player.GetPlayerTeam();
    }
    
    public virtual bool IsAllegianceSame(ChunkData chunk, ChunkData seconChunck)
    {
        return chunk.CharacterIsOnTile() && seconChunck.CharacterIsOnTile() && chunk.GetCurrentPlayer().GetPlayerTeam() == seconChunck.GetCurrentPlayer().GetPlayerTeam();
    }
		
    public virtual bool IsAllegianceSameForBuffs(ChunkData chunk)
    {
        return chunk == null || (chunk.GetCurrentPlayer() != null && chunk.GetCurrentPlayer().GetPlayerTeam() == _player.GetPlayerTeam() && !friendlyFire);
    }
    protected static bool CheckIfSpecificInformationType(ChunkData chunk, Type type)
    {
        return chunk.GetCharacterType() == type;
    }
    
    protected virtual bool CanAddVisionChunk(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked() && chunk.IsFogOnTile())
        {
            return true;
        }

        return false;
    }
    
    protected virtual void TryAddVisionChunk(ChunkData chunk, List<ChunkData> visionChunkList)
    {
        if (CanAddVisionChunk(chunk))
        {
            visionChunkList.Add(chunk);
        }
    }
    
    protected virtual List<ChunkData> GenerateVisionPattern(ChunkData centerChunk, int vision)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        List<ChunkData> visionList = new List<ChunkData>();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
			
        if (centerX >= 0 && centerX < chunksArray.GetLength(0) && centerY >= 0 && centerY < chunksArray.GetLength(1))
        {
            ChunkData chunk = chunksArray[centerX, centerY];
            TryAddVisionChunk(chunk, visionList);
        }
			
        for (int y = -vision; y <= vision; y++)
        {
            for (int x = -vision; x <= vision; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= vision)
                {
                    int targetX = centerX + x;
                    int targetY = centerY + y;

                    // Ensuring we don't go out of array bounds.
                    if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        TryAddVisionChunk(chunk, visionList);
                    }
                }
            }
        }
        return visionList;
    }
    
    public virtual List<ChunkData> GetVisionChunkList()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        _visionChunkList = GenerateVisionPattern(startChunk, _player.GetVisionRange());
        return _visionChunkList;
    }
    
    protected virtual bool CanAddTile(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked() && !chunk.IsFogOnTile()  && 
            (!chunk.ObjectIsOnTile() || chunk.GetCurrentObject().CanBeDestroyed() || chunk.ObjectIsOnTile() && chunk.GetCurrentObject().CanStepOn()))
        {
            return canUseOnSelf || chunk.GetCurrentPlayer() != GameTileMap.Tilemap.GetCurrentCharacter();
        }
        return false;
    }
    
    protected virtual void TryAddTile(ChunkData chunk)
    {
        if (CanAddTile(chunk))
        {
            _chunkList.Add(chunk);
        }
    }
    
    protected virtual void GeneratePlusPattern(ChunkData centerChunk, int length)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();

        for (int i = 1; i <= length; i++)
        {
            List<(int, int)> positions = new List<(int, int)> 
            {
                (centerX, centerY + i),  // Up
                (centerX, centerY - i),  // Down
                (centerX + i, centerY),  // Right
                (centerX - i, centerY)   // Left
            };

            foreach (var (x, y) in positions)
            {
                if (x >= 0 && x < chunksArray.GetLength(0) && y >= 0 && y < chunksArray.GetLength(1))
                {
                    ChunkData chunk = chunksArray[x, y];
                    TryAddTile(chunk);
                }
            }
        }
    }
    
    protected virtual void GeneratePlusPatternNonExtendable(ChunkData centerChunk, int length)
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
                        if (chunk.GetCharacterType() == typeof(Object))
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
    
    protected virtual void GenerateDiamondPattern(ChunkData centerChunk, int radius)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= radius)
                {
                    int targetX = centerX + x;
                    int targetY = centerY + y;

                    if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
                    {
                        ChunkData chunk = chunksArray[targetX, targetY];
                        TryAddTile(chunk);
                    }
                }
            }
        }
    }
    
    public virtual void CreateGrid()
    {
        CreateAvailableChunkList(attackRange);
        HighlightAllGridTiles();
    }
    
    public virtual void CreateAvailableChunkList(int range)
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        if(laserGrid)
        {
            if (laserGridExtendsBeyondFirstEnemy)
            {
                GeneratePlusPattern(startChunk, range);
            }
            else
            {
                GeneratePlusPatternNonExtendable(startChunk, range);
            }
        }
        else
        {
            GenerateDiamondPattern(startChunk, range);
        }
    }
    
    protected void HighlightAllGridTiles()
    {
        foreach (var chunk in _chunkList)
        {
            if (CanAddTile(chunk))
            {
                HighlightGridTile(chunk);
            }
        }
    }
    
    public bool IsPositionInGrid(Vector2 position)
    {
        return _chunkList.Contains(GameTileMap.Tilemap.GetChunk(position));
    }
    
    protected virtual void SetOtherTeamNonHoverAttackColor(ChunkData chunkData)
    {
        if (chunkData.GetCurrentPlayer() == null || (chunkData.GetCurrentPlayer() != null && !CanTileBeClicked(chunkData)))
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(otherTeamAbilityHighlight);
        }
        else
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(otherTeamCharacterOnGrid);
        }
    }
    
    public virtual void SetHoveredAttackColor(ChunkData chunkData)
    {
        if (_turnManager.GetCurrentTeamIndex() == _player.GetPlayerTeam())
        {
            SetCurrentTeamHoveredAttackColor(chunkData);
        }
        else
        {
            SetOtherTeamHoveredAttackColor(chunkData);
        }
    }
    
    protected virtual void SetCurrentTeamHoveredAttackColor(ChunkData chunkData)
    {
        if (!chunkData.CharacterIsOnTile() || (chunkData.CharacterIsOnTile() && !CanTileBeClicked(chunkData)))
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(abilityHighlightHover);
        }
        else
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(abilityHoverCharacter);
        }
    }
    
    protected virtual void SetOtherTeamHoveredAttackColor(ChunkData chunkData)
    {
        if (!chunkData.CharacterIsOnTile() || (chunkData.CharacterIsOnTile() && !CanTileBeClicked(chunkData)))
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(otherTeamHighlightHover);
        }
        else
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(otherTeamHoverCharacter);
        }
    }
    
    public Color GetAbilityHighlightColor()
    {
        return abilityHighlight;
    }
    
    public virtual void OnMoveArrows(ChunkData hoveredChunk, ChunkData previousChunk)
    {
			
    }

    private bool shouldResetColorsSwitch;
    private bool shouldResetColorsSwitchModes;
    public virtual void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (areaOfEffect)
        {
            OnMoveHoverEntireGrid(hoveredChunk, previousChunk);
        }
        else if (affectsAllCharactersAtOnce)
        {
            OnMoveHoverAllAffectedEnemiesAtOnce(hoveredChunk, previousChunk);
        }
        else if (affectsAllEnemiesInGrid && !IsAllegianceSame(hoveredChunk) && hoveredChunk!=null && hoveredChunk.CharacterIsOnTile())
        {
            if (shouldResetColorsSwitch)
            {
                ResetGridColors();
            }
            OnMoveHoverAllEnemiesInGrid(hoveredChunk, previousChunk);
            shouldResetColorsSwitchModes = true;
        }
        else if (affectsAllAlliesInGrid && IsAllegianceSame(hoveredChunk) && hoveredChunk!=null && hoveredChunk.CharacterIsOnTile())
        {
            if (!shouldResetColorsSwitch)
            {
                ResetGridColors();
            }
            OnMoveHoverAllAlliesInGrid(hoveredChunk, previousChunk);
            shouldResetColorsSwitchModes = true;
        }
        else
        {
            if (shouldResetColorsSwitchModes)
            {
                ResetGridColors();
                shouldResetColorsSwitchModes = false;
            }
            OnMoveHoverSingleCharacter(hoveredChunk, previousChunk);
        }
    }

    public virtual void OnMoveHoverSingleCharacter(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
        {
            SetNonHoveredAttackColor(previousChunk);
            DisableDamagePreview(previousChunk);
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted)
        {
            SetHoveredAttackColor(hoveredChunk);
            if (CanTileBeClicked(hoveredChunk))
            {
                EnableDamagePreview(hoveredChunk, minAttackDamage, maxAttackDamage);
            }
        }
        if (previousChunkHighlight != null)
        {
            SetNonHoveredAttackColor(previousChunk);
            DisableDamagePreview(previousChunk);
        }
    }

    private void ResetGridColors()
    {
        foreach (var chunk in _chunkList)
        {
            SetNonHoveredAttackColor(chunk);
            DisableDamagePreview(chunk);
        }
    }

    public virtual void OnMoveHoverAllEnemiesInGrid(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();
        
        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted)) // nuhoverinome off-grid
        {
            ResetGridColors();
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk) //Hoveriname ant to pacio ar siaip kazkoks gaidys ivyko
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted) //Jei uzhoverinome ant grido
        {
            if (CanTileBeClicked(hoveredChunk) && !IsAllegianceSame(hoveredChunk)) //Ant uzhoverinto langelio characteris
            {
                shouldResetColorsSwitch = false;
                foreach (var chunk in _chunkList)
                {
                    if (CanTileBeClicked(chunk) && !IsAllegianceSame(chunk))
                    {
                        SetHoveredAttackColor(chunk);
                        EnableDamagePreview(chunk, minAttackDamage, maxAttackDamage);
                    }
                }
            }
            else //ant uzhoverinto langelio ne characteris
            {
                hoveredChunkHighlight.SetHighlightColor(abilityHighlightHover);
            }
        }
        if (previousChunkHighlight != null) // Jei pries tai irgi buvome ant grido
        {
            if (CanTileBeClicked(previousChunk) && !IsAllegianceSame(previousChunk) && (!CanTileBeClicked(hoveredChunk) || IsAllegianceSame(hoveredChunk))) //Jei ten buvo veikėjas, be to, dabar nebe ant veikėjo esame
            {
                ResetGridColors();
            }
            else if(!CanTileBeClicked(previousChunk) && !CanTileBeClicked(hoveredChunk) && IsAllegianceSame(hoveredChunk) && IsAllegianceSame(previousChunk)) //Nei buvo veikejas ant praeito, nei yra ant dabartinio
            {
                SetNonHoveredAttackColor(previousChunk);
            }
        }
    }

    public virtual void OnMoveHoverAllAlliesInGrid(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted)) // nuhoverinome off-grid
        {
            ResetGridColors();
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk) //Hoveriname ant to pacio ar siaip kazkoks gaidys ivyko
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted) //Jei uzhoverinome ant grido
        {
            if (CanTileBeClicked(hoveredChunk) && IsAllegianceSame(hoveredChunk)) //Ant uzhoverinto langelio characteris
            {
                shouldResetColorsSwitch = true;
                foreach (var chunk in _chunkList)
                {
                    if (CanTileBeClicked(chunk) && IsAllegianceSame(chunk))
                    {
                        SetHoveredAttackColor(chunk);
                        EnableDamagePreview(chunk, minAttackDamage, maxAttackDamage);
                    }
                }
            }
            else //ant uzhoverinto langelio ne characteris
            {
                hoveredChunkHighlight.SetHighlightColor(abilityHighlightHover);
            }
        }
        if (previousChunkHighlight != null) // Jei pries tai irgi buvome ant grido
        {
            if (CanTileBeClicked(previousChunk) && IsAllegianceSame(previousChunk) && !CanTileBeClicked(hoveredChunk) && !IsAllegianceSame(hoveredChunk)) //Jei ten buvo veikėjas, be to, dabar nebe ant veikėjo esame
            {
                ResetGridColors();
            }
            else if(!CanTileBeClicked(previousChunk) && !IsAllegianceSame(previousChunk) && !CanTileBeClicked(hoveredChunk) && !IsAllegianceSame(hoveredChunk)) //Nei buvo veikejas ant praeito, nei yra ant dabartinio
            {
                SetNonHoveredAttackColor(previousChunk);
            }
        }
    }
    
    public virtual void OnMoveHoverAllAffectedEnemiesAtOnce(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();
        
        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted)) // nuhoverinome off-grid
        {
            ResetGridColors();
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk) //Hoveriname ant to pacio ar siaip kazkoks gaidys ivyko
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted) //Jei uzhoverinome ant grido
        {
            if (CanTileBeClicked(hoveredChunk)) //Ant uzhoverinto langelio characteris
            {
                foreach (var chunk in _chunkList)
                {
                    if (CanTileBeClicked(chunk))
                    {
                        SetHoveredAttackColor(chunk);
                        EnableDamagePreview(chunk, minAttackDamage, maxAttackDamage);
                    }
                }
            }
            else //ant uzhoverinto langelio ne characteris
            {
                hoveredChunkHighlight.SetHighlightColor(abilityHighlightHover);
            }
        }
        if (previousChunkHighlight != null) // Jei pries tai irgi buvome ant grido
        {
            if (CanTileBeClicked(previousChunk) && !CanTileBeClicked(hoveredChunk)) //Jei ten buvo veikėjas, be to, dabar nebe ant veikėjo esame
            {
                ResetGridColors();
            }
            else if(!CanTileBeClicked(previousChunk) && !CanTileBeClicked(hoveredChunk)) //Nei buvo veikejas ant praeito, nei yra ant dabartinio
            {
                SetNonHoveredAttackColor(previousChunk);
            }
        }
    }
    
    public virtual void OnMoveHoverEntireGrid(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                if (highlightTile != null)
                {
                    SetHoveredAttackColor(chunk);
                    if (CanTileBeClicked(chunk))
                    {
                        EnableDamagePreview(chunk, minAttackDamage, maxAttackDamage);
                    }
                }
            }
        }
        else if (hoveredChunk == null || !hoveredChunk.GetTileHighlight().isHighlighted)
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                if (highlightTile != null)
                {
                    SetNonHoveredAttackColor(chunk);
                    DisableDamagePreview(chunk);
                }
            }
        }
    }
    public virtual void EnableDamagePreview(ChunkData chunk, int minAttackDamage, int maxAttackDamage)
    {
        if (CanTileBeClicked(chunk))
        {
            HighlightTile highlightTile = chunk.GetTileHighlight();
            if (teamDisplayText!=string.Empty && IsAllegianceSame(chunk))
            {
                highlightTile.SetDamageText(teamDisplayText);
            }
            else if (enemyDisplayText!=string.Empty && !IsAllegianceSame(chunk))
            {
                highlightTile.SetDamageText(enemyDisplayText);
            }
            else if (maxAttackDamage == minAttackDamage)
            {
                highlightTile.SetDamageText(maxAttackDamage.ToString());
            }
            else
            {
                highlightTile.SetDamageText($"{minAttackDamage}-{maxAttackDamage}");
            }

            if (chunk.GetCurrentPlayer() != null && 
                chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation().GetHealth() <= minAttackDamage) 
            {
                highlightTile.ActivateDeathSkull(true);
            }
        }
    }
    
    public virtual void EnableDamagePreview(ChunkData chunk, string text)
    {
        if (CanTileBeClicked(chunk))
        {
            HighlightTile highlightTile = chunk.GetTileHighlight();
            highlightTile.SetDamageText(text);
        }
    }
    
    protected virtual void DisableDamagePreview(ChunkData chunk)
    {
        HighlightTile highlightTile = chunk.GetTileHighlight();
        highlightTile.ActivateDeathSkull(false);
        highlightTile.DisableDamageText();
    }
    
    public virtual void ClearGrid()
    {
        if (_chunkList != null)
        {
            foreach (var chunk in _chunkList)
            {
                HighlightTile highlightTile = chunk.GetTileHighlight();
                highlightTile.ActivateColorGridTile(false);
                DisableDamagePreview(chunk);
            }

            _chunkList.Clear();
        }
    }

    public void IncreaseRange(int range)
    {
        attackRange += range;
    }

    public bool GetFriendlyFire()
    {
        return friendlyFire;
    }

    public int GetRange()
    {
        return attackRange;
    }
    
    public void AddTurnManager(TurnManager turnManager)
    {
        _turnManager = turnManager;
    }
    
    public void SetPlayer(Player player)
    {
        _player = player;
    }
    
    protected virtual bool CheckAbilityBounds(ChunkData chunkData)
    {
        return false;
    }
    
    public List<ChunkData> GetChunkList()
    {
        return _chunkList;
    }
}