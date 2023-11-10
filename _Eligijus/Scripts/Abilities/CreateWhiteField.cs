using Godot;


public partial class CreateWhiteField : BaseAction
{
    [Export] 
    private Resource whiteFieldPrefab;
    
    public CreateWhiteField()
    {
		
    }
    public CreateWhiteField(CreateWhiteField createWhiteField) : base(createWhiteField)
    {
        whiteFieldPrefab = createWhiteField.whiteFieldPrefab;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CreateWhiteField createWhiteField = new CreateWhiteField((CreateWhiteField)action);
        return createWhiteField;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        for (int i = 0; i < _chunkList.Count; i++)
        {
            PackedScene spawnResource = (PackedScene)whiteFieldPrefab;
            Player spawnedWhiteField = spawnResource.Instantiate<Player>();
            player.GetTree().Root.CallDeferred("add_child", spawnedWhiteField);
            GameTileMap.Tilemap.MoveSelectedCharacter(_chunkList[i], spawnedWhiteField);
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