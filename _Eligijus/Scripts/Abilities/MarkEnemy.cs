public partial class MarkEnemy : BaseAction
{
    public MarkEnemy()
    {
 		
    }
    public MarkEnemy(MarkEnemy markEnemy): base(markEnemy)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        MarkEnemy markEnemy = new MarkEnemy((MarkEnemy)action);
        return markEnemy;
    }

    protected override void Start()
    {
        base.Start();
        customText = "MARK";
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PlayerAbilityAnimation();
        Player target = chunk.GetCurrentPlayer();
        MarkDebuff debuff = new MarkDebuff();
        target.debuffManager.AddDebuff(debuff,_player);
        FinishAbility();
    }
}