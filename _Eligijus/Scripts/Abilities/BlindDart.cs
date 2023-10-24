public partial class BlindDart : BaseAction
{
    public BlindDart()
    {

    }

    public BlindDart(BlindDart blaze) : base(blaze)
    {
    }
	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        BlindDart blaze = new BlindDart((BlindDart)action);
        return blaze;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);

        }
    }
}