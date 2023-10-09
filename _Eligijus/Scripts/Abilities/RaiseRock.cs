using Godot;

public partial class RaiseRock : BaseAction
{
    [Export]
    public Resource WallPrefab;
    
    public RaiseRock(RaiseRock ability): base(ability)
    {
        WallPrefab = ability.WallPrefab;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        RaiseRock ability = new RaiseRock((RaiseRock)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        PackedScene spawnResource = (PackedScene)WallPrefab;
        Player spawnedWall = spawnResource.Instantiate<Player>();
        player.GetTree().Root.CallDeferred("add_child", spawnedWall);
        PlayerInformation tempPlayerInformation = spawnedWall.playerInformation;
        GameTileMap.Tilemap.SetCharacter(chunk, spawnedWall, tempPlayerInformation);
        FinishAbility();
    }
}