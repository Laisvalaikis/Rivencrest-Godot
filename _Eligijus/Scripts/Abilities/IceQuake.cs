using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class IceQuake : BaseAction
{
    [Export] private int rootDamage = 5;
    private int bonusDamage = 0;
    public IceQuake()
    {
        
    }
    public IceQuake(IceQuake ability): base(ability)
    {
        rootDamage = ability.rootDamage;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        IceQuake ability = new IceQuake((IceQuake)action);
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
                Player target = hoveredChunk.GetCurrentPlayer();
                if (target.debuffManager.ContainsDebuff(typeof(SlowDebuff)))
                {
                    bonusDamage += rootDamage;
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
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            if (chunk.CharacterIsOnTile())
            {
                Player target = chunk.GetCurrentPlayer();
                if (target.debuffManager.ContainsDebuff(typeof(SlowDebuff)))
                {
                    RootDebuff rootDebuff = new RootDebuff();
                    target.debuffManager.AddDebuff(rootDebuff,_player);
                    bonusDamage += rootDamage;
                }
            }
            DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            SlowDebuff debuff = new SlowDebuff(2, 2);
            chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);
            FinishAbility();
        }
    }
}
    

