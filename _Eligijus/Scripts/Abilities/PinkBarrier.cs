using Godot;

public partial class PinkBarrier : BaseAction
{
    public PinkBarrier(PinkBarrier ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PinkBarrier ability = new PinkBarrier((PinkBarrier)action);
        return ability;
    }

    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        // chunk.GetCurrentPlayerInformation().BarrierProvider = gameObject;
       // GetSpecificGroundTile(position, 0, 0, blockingLayer).GetComponent<GridMovement>().AvailableMovementPoints++;
        FinishAbility();
    }
}
