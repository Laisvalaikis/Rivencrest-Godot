using Godot;

public partial class WindBoost : BaseAction
{
    private bool isAbilityActive = false;
    
    public override void OnTurnStart()//pradzioj ejimo
    {
        base.OnTurnStart();
        isAbilityActive = false;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        isAbilityActive = true;
        FinishAbility();
    }

}
