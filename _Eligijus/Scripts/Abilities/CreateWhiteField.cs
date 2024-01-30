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
    public override void OnBeforeStart(ChunkData chunkData)
    {
        base.OnBeforeStart(chunkData);
        if (i >= 2)
        {
            foreach (Object field in spawnedFields)
            {
                field.Death();
            }
            spawnedFields.Clear();
        }
        i++;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
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
}