using System;
using Godot;
using Godot.Collections;

public partial class SpearPulse : BaseAction
{

    public SpearPulse()
    {
    }

    public SpearPulse(SpearPulse ability): base(ability)
    {
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        SpearPulse ability = new SpearPulse((SpearPulse)action);
        return ability;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        SpearHasBeenCast(ref chunk);
        UpdateAbilityButton();
        for (int i = 0; i < _chunkList.Count; i++)
        {
            DealRandomDamageToTarget(_chunkList[i], minAttackDamage, maxAttackDamage); //pirmus 2 naikina
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    
    public override void CreateAvailableChunkList(int range)
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        SpearHasBeenCast(ref startChunk);
        if(laserGrid)
        {
            GeneratePlusPattern(startChunk, range);
        }
        else
        {
            GenerateDiamondPattern(startChunk, range);
        }
    }

    public void SpearHasBeenCast(ref ChunkData chunk)
    {
        Array<Ability> playerAbilities = _player.actionManager.GetAllAbilities();
        foreach (Ability ability in playerAbilities)
        {
            if (typeof(ThrowSpear) == ability.Action.GetType())
            {
                ThrowSpear throwSpearAbility = (ThrowSpear)ability.Action;
                if (IsInstanceValid(throwSpearAbility.spawnedCharacter) && throwSpearAbility.spawnedCharacter != null)
                {
                    chunk = GameTileMap.Tilemap.GetChunk(throwSpearAbility.spawnedCharacter.GlobalPosition);
                    break;
                }
            }
        }
    }
    
    // public override void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
    // {
    //     if (hoveredChunk == previousChunk) return;
    //     if (hoveredChunk != null && hoveredChunk.GetTileHighlight().isHighlighted)
    //     {
    //         for (int i = 0; i < _chunkList.Count; i++)
    //         {
    //             ChunkData chunkToHighLight = _chunkList[i];
    //             if (chunkToHighLight != null)
    //             {
    //                 SetHoveredAttackColor(chunkToHighLight);
    //                 EnableDamagePreview(chunkToHighLight);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         for (int i = 0; i < _chunkList.Count; i++)
    //         {
    //             ChunkData chunkToHighLight = _chunkList[i];
    //             if (chunkToHighLight != null)
    //                 SetNonHoveredAttackColor(chunkToHighLight);
    //             DisableDamagePreview(chunkToHighLight);
    //         }
    //     }
    //
    // }
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
    
}
