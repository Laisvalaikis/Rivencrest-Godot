using System.Collections.Generic;
using Godot;

public partial class CometFall : BaseAction
{
    [Export] private ObjectData cometTileData;
    [Export] private Resource cometTilePrefab;
    private Object spawnedCometTile;

    public CometFall()
    {
        
    }
    public CometFall(CometFall ability): base(ability)
    {
        cometTilePrefab = ability.cometTilePrefab;
        cometTileData = ability.cometTileData;
        enemyDisplayText = " ";
        teamDisplayText = " ";
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CometFall ability = new CometFall((CometFall)action);
        return ability;
    }

    public override bool CanBeUsedOnTile(ChunkData chunkData)
    {
        return true;
    }

    
    public override async void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        await PlayAnimation("CometFall", chunk);
        PlayerAbilityAnimation();
        PackedScene spawnCharacter = (PackedScene)cometTilePrefab;
        spawnedCometTile = spawnCharacter.Instantiate<Object>();
        _player.GetTree().CurrentScene.CallDeferred("add_child", spawnedCometTile);
        spawnedCometTile.SetupObject(cometTileData);
        spawnedCometTile.AddPlayerForObjectAbilities(_player);
        GameTileMap.Tilemap.SpawnObject(spawnedCometTile, chunk);
        FinishAbility();
    }
}
