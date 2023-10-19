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
        for (int i = 0; i < _chunkList.Count; i++)
        {
            PackedScene spawnResource = (PackedScene)whiteFieldPrefab;
            Player spawnedWhiteField = spawnResource.Instantiate<Player>();
            player.GetTree().Root.CallDeferred("add_child", spawnedWhiteField);
            GameTileMap.Tilemap.SetCharacter(_chunkList[i], spawnedWhiteField);
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }

}