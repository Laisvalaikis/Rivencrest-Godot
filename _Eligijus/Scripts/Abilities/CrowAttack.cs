using System.Collections.Generic;
using Godot;

public partial class CrowAttack : BaseAction
{
    [Export]
    public int poisonBonusDamage=2;
    
    public CrowAttack()
    {
		
    }
    public CrowAttack(CrowAttack crowAttack) : base(crowAttack)
    {
        poisonBonusDamage = crowAttack.poisonBonusDamage;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CrowAttack crowAttack = new CrowAttack((CrowAttack)action);
        return crowAttack;
    }
    
    public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    {
        HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
        HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

        if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted)) // nuhoverinome off-grid
        {
            foreach (var chunk in _chunkList)
            {
                SetNonHoveredAttackColor(chunk);
                DisableDamagePreview(chunk);
            }
        }
        if (hoveredChunkHighlight == null || hoveredChunk == previousChunk) //Hoveriname ant to pacio ar siaip kazkoks gaidys ivyko
        {
            return;
        }
        if (hoveredChunkHighlight.isHighlighted) //Jei uzhoverinome ant grido
        {
            if (CanTileBeClicked(hoveredChunk)) //Ant uzhoverinto langelio characteris
            {
                foreach (var chunk in _chunkList)
                {
                    if (CanTileBeClicked(chunk))
                    {
                        SetHoveredAttackColor(chunk);
                    }
                }
            }
            else //ant uzhoverinto langelio ne characteris
            {
                hoveredChunkHighlight.SetHighlightColor(abilityHighlightHover);
            }
        }
        if (previousChunkHighlight != null) // Jei pries tai irgi buvome ant grido
        {
            if (CanTileBeClicked(previousChunk) && !CanTileBeClicked(hoveredChunk)) //Jei ten buvo veikėjas, be to, dabar nebe ant veikėjo esame
            {
                foreach (var chunk in _chunkList)
                {
                    SetNonHoveredAttackColor(chunk);
                    DisableDamagePreview(chunk);
                }
            }
            else if(!CanTileBeClicked(previousChunk) && !CanTileBeClicked(hoveredChunk)) //Nei buvo veikejas ant praeito, nei yra ant dabartinio
            {
                SetNonHoveredAttackColor(previousChunk);
            }
        }
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        foreach (var t in _chunkList)
        {
            if (t.CharacterIsOnTile() && CanTileBeClicked(t))
            {
                int bonusDamage = 0;
                if (t.GetCurrentPlayer().GetPoisonCount() > 0)
                {
                    bonusDamage += poisonBonusDamage;
                }
                DealRandomDamageToTarget(t,minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            }
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    public override void OnTurnStart()
    {
        base.OnTurnStart();
    }

}