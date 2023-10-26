using System.Collections.Generic;
using Godot;

public partial class WallSmash : BaseAction
{
    [Export]
    private int damageToWall = 1;

    public WallSmash()
    {
    }

    public WallSmash(WallSmash ability): base(ability)
    {
        damageToWall = ability.damageToWall;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        WallSmash ability = new WallSmash((WallSmash)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        if (chunk.GetCurrentPlayer() != null && chunk.GetInformationType() == InformationType.Player)
        {
            DealRandomDamageToTarget(chunk, minAttackDamage-2, maxAttackDamage-2);
        }
        else if(chunk.GetCurrentPlayer() != null && chunk.GetInformationType() != InformationType.Player)
        {
            DestroyObject(chunk);
        }
        FinishAbility();
    }
    private void DestroyObject(ChunkData chunkData)
    {
        (int x, int y) = chunkData.GetIndexes();
        var pushDirectionVectors = new List<(int, int)>
        {
            (1, 0),
            (0, 1),
            (-1, 0),
            (0,-1)
        };
        foreach (var position in pushDirectionVectors)
        {
            ChunkData targetChunkData =
                GameTileMap.Tilemap.GetChunkDataByIndex(position.Item1 + x, position.Item2 + y);
            if (targetChunkData != null && targetChunkData.GetCurrentPlayer() == null)
            {
                ChunkData targetChunkDataPlayer =
                    GameTileMap.Tilemap.GetChunkDataByIndex(position.Item1 * 2 + x, position.Item2 * 2 + y);
                if (targetChunkDataPlayer != null && targetChunkDataPlayer.GetCurrentPlayer() != null 
                                                  && targetChunkDataPlayer.GetInformationType() == InformationType.Player && !IsAllegianceSame(targetChunkDataPlayer))
                {
                    DealRandomDamageToTarget(targetChunkDataPlayer, minAttackDamage, maxAttackDamage);
                }
            }
        }
        chunkData.GetCurrentPlayer().playerInformation.DealDamage(damageToWall, false, player);
    }
}
