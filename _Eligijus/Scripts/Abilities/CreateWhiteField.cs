using System.Collections.Generic;
using Godot;
using Godot.Collections;


public partial class CreateWhiteField : BaseAction
{
    [Export] private ObjectData whiteFieldData;
    [Export] private Resource whiteFieldPrefab;
    private Object spawnedArrowTile;
    private Array<Object> spawnedFields = new();
    private int i = 0;
    private bool hasAbilityBeenUsed = false;
    
    public CreateWhiteField()
    {
		
    }
    public CreateWhiteField(CreateWhiteField createWhiteField) : base(createWhiteField)
    {
        whiteFieldPrefab = createWhiteField.whiteFieldPrefab;
        whiteFieldData = createWhiteField.whiteFieldData;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CreateWhiteField createWhiteField = new CreateWhiteField((CreateWhiteField)action);
        return createWhiteField;
    }
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        i++;
        if (i >= 2)
        {
            foreach (Object field in spawnedFields)
            {
                field.Death();
            }
            spawnedFields.Clear();
            hasAbilityBeenUsed = false;
        }
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        hasAbilityBeenUsed = true;
        i = 0;
        foreach (var chunkData in _chunkList)
        {
            PackedScene spawnCharacter = (PackedScene)whiteFieldPrefab; 
            spawnedArrowTile = spawnCharacter.Instantiate<Object>();
            _player.GetTree().Root.CallDeferred("add_child", spawnedArrowTile);
            spawnedArrowTile.SetupObject(whiteFieldData);
            spawnedArrowTile.AddPlayerForObjectAbilities(_player);
            GameTileMap.Tilemap.SpawnObject(spawnedArrowTile, chunkData);
            spawnedFields.Add(spawnedArrowTile);
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        if (hoveredChunk == previousChunk) return;
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        if ((hoveredChunkHighlight == null && previousChunkHighlight!=null && previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight!=null && !hoveredChunkHighlight.isHighlighted && previousChunkHighlight.isHighlighted))
        {
            foreach (var chunk in _chunkList)
            {
                SetNonHoveredAttackColor(chunk);
            }
        }
        else if ((hoveredChunkHighlight!=null && previousChunkHighlight!=null && hoveredChunkHighlight.isHighlighted && !previousChunkHighlight.isHighlighted) || (hoveredChunkHighlight!=null && previousChunkHighlight==null && hoveredChunkHighlight.isHighlighted))
        {
            foreach (var chunk in _chunkList)
            {
                SetHoveredAttackColor(chunk);
            }
        }
    }
}