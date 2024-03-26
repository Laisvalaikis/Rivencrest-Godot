using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public override async void OnBeforeStart(ChunkData chunkData)
    {
        base.OnBeforeStart(chunkData);
        if (i >= 1)
        {
            foreach (Object field in spawnedFields)
            {
                field.Death();
            }
            spawnedFields.Clear();
            _player.CurrentIdleAnimation = "Idle";
            AnimationPlayer animationPlayer = _player.objectInformation.GetObjectInformation().animationPlayer;
            animationPlayer.Play("FogToIdle");
            await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("FogToIdle").Length-0.1f));
            animationPlayer.Play(_player.CurrentIdleAnimation);
        }
        i++;
    }

    public override bool CanBeUsedOnTile(ChunkData chunkData)
    {
        return true;
    }
    
    public override async void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        i = 0;
        AnimationPlayer animationPlayer = _player.objectInformation.GetObjectInformation().animationPlayer;
        animationPlayer.Play("CreateFog");
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("CreateFog").Length-0.1f));
        _player.CurrentIdleAnimation = "FogIdle";
        animationPlayer.Play(_player.CurrentIdleAnimation);
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