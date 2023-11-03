using Godot;

public partial class CreateFog : BaseAction
{
    [Export] 
    private Resource fogPrefab;
    private bool isFogActive = true;
    private Player spawnedFog;
    private int i = 0;
    
    public CreateFog()
    {
		
    }
    public CreateFog(CreateFog createFog) : base(createFog)
    {
        fogPrefab = createFog.fogPrefab;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CreateFog createFog = new CreateFog((CreateFog)action);
        return createFog;
    }
    
    public override void OnTurnStart()//reikes veliau tvarkyt kai bus animacijos ir fog of war
    {
        if (isFogActive)
        {
            i++;
            if (i >= 2)
            {
                spawnedFog.QueueFree();
                isFogActive = false;
                i = 0;
            }
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        PackedScene spawnCharacter = (PackedScene)fogPrefab;
        spawnedFog = spawnCharacter.Instantiate<Player>();
        player.GetTree().Root.CallDeferred("add_child", spawnedFog);
        GameTileMap.Tilemap.MoveSelectedCharacter(chunk, spawnedFog);        
        FinishAbility();
        isFogActive = true;
        i = 0;
    }
}