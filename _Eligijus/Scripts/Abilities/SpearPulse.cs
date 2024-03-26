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
            PlayerAbilityAnimation();
            PlayAnimation("Purple1", chunk);
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
            if (laserGridExtendsBeyondFirstEnemy)
            {
                GeneratePlusPattern(startChunk, range);
            }
            else
            {
                GeneratePlusPatternNonExtendable(startChunk, range);
            }
        }
        else
        {
            GenerateDiamondPattern(startChunk, range);
        }
    }

    private void SpearHasBeenCast(ref ChunkData chunk)
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
}
