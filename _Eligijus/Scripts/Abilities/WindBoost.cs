using Godot;

public partial class WindBoost : BaseAction
{
    private bool isAbilityActive = false;
    public WindBoost()
    {
    }

    public WindBoost(WindBoost ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        WindBoost ability = new WindBoost((WindBoost)action);
        return ability;
    }
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
