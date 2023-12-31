using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Avalanche : BaseAction
{
    public Avalanche()
    {
    		
    }
    public Avalanche(Avalanche avalanche): base(avalanche)
    {
        
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Avalanche avalanche = new Avalanche((Avalanche)action);
        return avalanche;
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
                        EnableDamagePreview(chunk, minAttackDamage, maxAttackDamage);
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
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            foreach (ChunkData chunkData in _chunkList)
            {
                if (CanTileBeClicked(chunkData))
                {
                    DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
                }
            }
            base.ResolveAbility(chunk);
            FinishAbility();
        }
    }
    public override bool CanTileBeClicked(ChunkData chunk)
    {
        if (chunk.GetTileHighlight().isHighlighted && 
            !IsAllegianceSame(chunk) &&
            IsCharacterAffectedByCrowdControl(chunk))
        {
            return true;
        }
        return false;
    }
    private bool IsCharacterAffectedByCrowdControl(ChunkData chunk)
    {
        if (CheckIfSpecificInformationType(chunk, typeof(Player)))
        {
            DebuffManager targetDebuffManager = chunk.GetCurrentPlayer().debuffManager;
            if(targetDebuffManager.ContainsDebuff(typeof(PlayerStun))||targetDebuffManager.ContainsDebuff(typeof(RootDebuff))||
               targetDebuffManager.ContainsDebuff(typeof(SlowDebuff))||targetDebuffManager.ContainsDebuff(typeof(SilenceDebuff)))
                return true;
        }
        return false;
    }
}