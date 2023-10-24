public partial class MarkEnemy : BaseAction
{
    private ChunkData _target;
 
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
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
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
}