using System.Collections.Generic;
using Godot;

public partial class Blaze : BaseAction //removed ability
{
    [Export] public int bonusDamage = 4;
    public Blaze()
    {

    }

    public Blaze(Blaze blaze) : base(blaze)
    {
        bonusDamage = blaze.bonusDamage;
    }
	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Blaze blaze = new Blaze((Blaze)action);
        return blaze;
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
                // if (target.debuffs.PlayerHaveAFlame())
                // {
                //     Aflame(chunk);
                // }
                // else
                // {
                    DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
                // }
            }
            
        }
    }

    private void Aflame(ChunkData tile)
    {
        (int x, int y) = tile.GetIndexes();
        
        var directionVectors = new List<(int, int)>
        {
            (1 + x, 0 + y),
            (0 + x, 1 + y),
            (-1 + x, 0 + y),
            (0 + x, -1 + y)
        };

        foreach (var directions in directionVectors)
        {
            ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(directions.Item1, directions.Item2);
            if (chunkData.CharacterIsOnTile())
            {
                Player player = chunkData.GetCurrentPlayer();
                if (IsAllegianceSame(tile))
                {
                    DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
                }
            }
        }
        if (tile.CharacterIsOnTile())
        {
            DealRandomDamageToTarget(tile, minAttackDamage, maxAttackDamage);
        }
    }
}