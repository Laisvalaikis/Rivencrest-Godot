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

    public override void Start()
    {
        base.Start();
        customText = "MARK";
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            Player target = chunk.GetCurrentPlayer();
            MarkDebuff debuff = new MarkDebuff();
            target.debuffManager.AddDebuff(debuff,player);
            FinishAbility();
        }
        // chunk.GetCurrentPlayerInformation().Marker = gameObject;
    }
    
}