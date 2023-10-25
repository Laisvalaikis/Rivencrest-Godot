using Godot;

public partial class PinkBarrier : BaseAction
{
    public PinkBarrier()
    {
        
    }
    public PinkBarrier(PinkBarrier ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PinkBarrier ability = new PinkBarrier((PinkBarrier)action);
        return ability;
    }
    
    protected override void TryAddTile(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked())
        {
            _chunkList.Add(chunk);
        }
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
       base.ResolveAbility(chunk);
        // chunk.GetCurrentPlayerInformation().BarrierProvider = gameObject;
       // GetSpecificGroundTile(position, 0, 0, blockingLayer).GetComponent<GridMovement>().AvailableMovementPoints++;
       if (chunk.CharacterIsOnTile())
       {
           Player target = chunk.GetCurrentPlayer();
           target.AddBarrier();
           target.actionManager.AddAbilityPoints(1);
       }

       FinishAbility();
    }
}
