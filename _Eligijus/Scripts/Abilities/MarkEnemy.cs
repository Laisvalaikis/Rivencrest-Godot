public partial class MarkEnemy : BaseAction
{
    private Player _target;
 
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

    public override void OnTurnEnd(ChunkData chunkData)
    {
        base.OnTurnEnd(chunkData);
        if (_target != null && _target.debuffs.IsMarked())
        {
            _target.debuffs.UnMark();
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            _target = chunk.GetCurrentPlayer();
            player.debuffs.Mark();
            FinishAbility();
        }
        // chunk.GetCurrentPlayerInformation().Marker = gameObject;
    }

    public override void Die() // probably ateity kitaip bus daroma
    {
        base.Die();
        if (_target != null && _target.debuffs.IsMarked())
        {
            _target.debuffs.UnMark();
        }
    }
}