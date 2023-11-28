using Godot;

public partial class IsolatedStrike : BaseAction
{
    [Export]
    private int isolationDamage = 7;

    private int bonusDamage = 0;

    public IsolatedStrike()
    {
        
    }
    public IsolatedStrike(IsolatedStrike ability): base(ability)
    {
        isolationDamage = ability.isolationDamage;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        IsolatedStrike ability = new IsolatedStrike((IsolatedStrike)action);
        return ability;
    }
    
    public override void EnableDamagePreview(ChunkData chunk, string text = null)
    {
        HighlightTile highlightTile = chunk.GetTileHighlight();
        if (customText != null)
        {
            highlightTile.SetDamageText(customText);
        }
        else
        {
            if (maxAttackDamage == minAttackDamage)
            {
                highlightTile.SetDamageText((maxAttackDamage+bonusDamage).ToString());
            }
            else
            {
                highlightTile.SetDamageText($"{minAttackDamage+bonusDamage}-{maxAttackDamage+bonusDamage}");
            }

            if (chunk.GetCurrentPlayer()!=null && chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation().GetHealth() <= minAttackDamage)
            {
                highlightTile.ActivateDeathSkull(true);
            }
        }
    }
    
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
        {
            SetNonHoveredAttackColor(previousChunk);
            DisableDamagePreview(previousChunk);
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted)
        {
            SetHoveredAttackColor(hoveredChunk);
            if (CanTileBeClicked(hoveredChunk))
            {
                if (IsTargetIsolated(hoveredChunk))
                {
                    bonusDamage += isolationDamage;
                }
                EnableDamagePreview(hoveredChunk);
                bonusDamage = 0;
            }
        }
        if (previousChunkHighlight != null)
        {
            SetNonHoveredAttackColor(previousChunk);
            DisableDamagePreview(previousChunk);
        }
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        int bonusDamage = 0;
        //Isolation
        if (IsTargetIsolated(chunk))
        {
            bonusDamage += isolationDamage;
        }
        DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        FinishAbility();
    }
    
    private bool IsTargetIsolated(ChunkData target)
    {
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int x, int y) indexes = target.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && chunks[nx, ny].CharacterIsOnTile() && chunks[nx, ny].GetCurrentPlayer().GetPlayerTeam()==target.GetCurrentPlayer().GetPlayerTeam())
            {
                return false;
            }
        }
        return true;
    }
}
