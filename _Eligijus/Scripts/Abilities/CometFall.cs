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
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CometFall ability = new CometFall((CometFall)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PackedScene spawnCharacter = (PackedScene)cometTilePrefab;
        spawnedCometTile = spawnCharacter.Instantiate<Object>();
        _player.GetTree().Root.CallDeferred("add_child", spawnedCometTile);
        spawnedCometTile.SetupObject(cometTileData);
        spawnedCometTile.AddPlayerForObjectAbilities(_player);
        GameTileMap.Tilemap.SpawnObject(spawnedCometTile, chunk);
        FinishAbility();
    }

    public override bool CanTileBeClicked(ChunkData chunkData)
    {
        return chunkData.GetTileHighlight().isHighlighted; //might be bullshit
    }
}
