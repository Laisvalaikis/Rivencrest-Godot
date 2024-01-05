using System.Collections.Generic;
using Godot;


public partial class CreateWhiteField : BaseAction
{
    [Export] private ObjectData whiteFieldData;
    [Export] private Resource whiteFieldPrefab;
    private Object spawnedArrowTile;
    
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

    public override void OnExitAbility(ChunkData chunkDataPrev, ChunkData chunk)
    {
        //Dont use this, create seperate ability for adding/removing buff and add ability to whitefield object
    }
    
    
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        foreach (var chunkData in _chunkList)
        {
            PackedScene spawnCharacter = (PackedScene)whiteFieldPrefab; 
            spawnedArrowTile = spawnCharacter.Instantiate<Object>();
            _player.GetTree().Root.CallDeferred("add_child", spawnedArrowTile);
            spawnedArrowTile.SetupObject(whiteFieldData);
            spawnedArrowTile.AddPlayerForObjectAbilities(_player);
            GameTileMap.Tilemap.SpawnObject(spawnedArrowTile, chunkData);
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