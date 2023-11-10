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

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();
        if (_target != null && _target.debuffs.IsMarked())
        {
            _target.debuffs.UnMark();
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        if (chunk.CharacterIsOnTile())
        {
            UpdateAbilityButton();
            Player player = chunk.GetCurrentPlayer();
            _target = player;
            player.debuffs.Mark();
        }
        
        // chunk.GetCurrentPlayerInformation().Marker = gameObject;
        FinishAbility();
    }
    
    public override void SetHoveredAttackColor(ChunkData chunkData)
    {
        if (!chunkData.CharacterIsOnTile() || (chunkData.CharacterIsOnTile() && !CanTileBeClicked(chunkData)))
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(abilityHighlightHover);
        }
        else
        {
            chunkData.GetTileHighlight()?.SetHighlightColor(abilityHoverCharacter);
            EnableDamagePreview(chunkData, "MARK");
        }
    }

    public override void Die()
    {
        base.Die();
        if (_target != null && _target.debuffs.IsMarked())
        {
            _target.debuffs.UnMark();
        }
    }
}