using System;
using System.Collections.Generic;
using Godot;

public partial class TileAction : Resource
{
    [Export] public int minAttackDamage = 0;
    [Export] public int maxAttackDamage = 0;
    protected int bonusDamage = 0;
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
    [Export] protected string customText = string.Empty;
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
        teamDisplayText = action.teamDisplayText;
        enemyDisplayText = action.enemyDisplayText;
        customText = action.customText;
    }

    protected virtual void Start()
    {
        _chunkList = new List<ChunkData>();
        
    }

    protected virtual void HighlightGridTile(ChunkData chunkData)
    {
        chunkData.GetTileHighlight().ActivateColorGridTile(true);
        SetNonHoveredAttackColor(chunkData);
        chunkData.GetTileHighlight().EnableTile(true);
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
    
    private void SetCurrentTeamNonHoverAttackColor(ChunkData chunkData)
    {
        if (!CanTileBeClicked(chunkData))
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
            return canUseOnSelf || chunk.GetCurrentPlayer() != _player;
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
        _chunkArray = new ChunkData[4,length];
        (int centerX, int centerY) = centerChunk.GetIndexes();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();

        for (int i = 1; i <= length; i++)
        {
            List<(int, int,int)> positions = new List<(int, int, int)> 
            {
                (centerX, centerY + i,0),  // Up
                (centerX, centerY - i,1),  // Down
                (centerX + i, centerY,2),  // Right
                (centerX - i, centerY,3)   // Left
            };

            foreach (var (x, y,direction) in positions)
            {
                if (GameTileMap.Tilemap.CheckBounds(x,y))
                {
                    ChunkData chunk = chunksArray[x, y];
                    _chunkArray[direction, i-1] = chunk;
                    TryAddTile(chunk);
                }
            }
        }
    }
    
    protected void GeneratePlusPatternNonExtendable(ChunkData centerChunk, int length)
    {
        _chunkArray = new ChunkData[4,length];
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
                if (GameTileMap.Tilemap.CheckBounds(x,y))
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
                        _chunkArray[direction, i-1] = chunk;
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
            HighlightGridTile(chunk);
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
            if (laserGrid)
            {
                OnMoveHoverLaserSelectAll(hoveredChunk, previousChunk);
            }
            else
            {
                OnMoveHoverEntireGrid(hoveredChunk, previousChunk);
            }
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
    
    protected virtual void ModifyBonusDamage(ChunkData chunk)
    {
			
    }

    protected ChunkData[,] _chunkArray;
    protected int _index = -1;
    protected int _globalIndex = -1;

    protected int FindChunkIndex(ChunkData chunkData)
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
	
    public virtual void OnMoveHoverLaserSelectAll(ChunkData hoveredChunk, ChunkData previousChunk)
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
                    if (chunkToHighLight != null)
                    {
                        SetHoveredAttackColor(chunkToHighLight);
                        EnableDamagePreview(chunkToHighLight);
                    }
                }
            }
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
                EnableDamagePreview(hoveredChunk);
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
                        EnableDamagePreview(chunk);
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
                        EnableDamagePreview(chunk);
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
                        EnableDamagePreview(chunk);
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
                        EnableDamagePreview(chunk);
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
    public virtual void EnableDamagePreview(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            ModifyBonusDamage(chunk);
            HighlightTile highlightTile = chunk.GetTileHighlight();
            if (customText != string.Empty)
            {
                highlightTile.SetDamageText(customText);
            }
            else if (teamDisplayText!=string.Empty && IsAllegianceSame(chunk))
            {
                highlightTile.SetDamageText(teamDisplayText);
            }
            else if (enemyDisplayText!=string.Empty && !IsAllegianceSame(chunk))
            {
                highlightTile.SetDamageText(enemyDisplayText);
            }
            else if (maxAttackDamage == minAttackDamage)
            {
                highlightTile.SetDamageText((maxAttackDamage + bonusDamage).ToString());
            }
            else
            {
                highlightTile.SetDamageText($"{minAttackDamage + bonusDamage}-{maxAttackDamage + bonusDamage}");
            }

            if (chunk.GetCurrentPlayer() != null && 
                chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation().GetHealth() <= (minAttackDamage+bonusDamage)) 
            {
                highlightTile.ActivateDeathSkull(true);
            }
            bonusDamage = 0;
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