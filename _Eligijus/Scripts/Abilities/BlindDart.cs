public partial class BlindDart : BaseAction // need fog of war for this one
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
            UpdateAbilityButton();
            base.ResolveAbility(chunk);

        }
    }
}