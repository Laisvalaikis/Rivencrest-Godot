using Godot;

public partial class MistShield : BaseAction
{
    private bool isAbilityActive = false;

    public MistShield()
    {
        
    }
    public MistShield(MistShield ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        MistShield ability = new MistShield((MistShield)action);
        return ability;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (isAbilityActive)
        {
            player.RemoveBarrier();
        }
        player.playerInformation.Protected = false;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        isAbilityActive = true;
        player.playerInformation.Protected = true;
        player.AddBarrier();
        FinishAbility();
    }
}
