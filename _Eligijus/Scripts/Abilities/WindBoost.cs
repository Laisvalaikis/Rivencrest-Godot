using Godot;
using System;
using System.Collections.Generic;
public partial class WindBoost : BaseAction
{
    private bool isAbilityActive = false;
    [Export] private int minHeal = 3;
    [Export] private int maxHeal = 5;
    private Random _random;
    public WindBoost()
    {
    }

    public WindBoost(WindBoost ability): base(ability)
    {
        minHeal = ability.minHeal;
        maxHeal = ability.maxHeal;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        WindBoost ability = new WindBoost((WindBoost)action);
        return ability;
    }
    public override void CreateAvailableChunkList(int attackRange)
    {
        _chunkList.Add(GameTileMap.Tilemap.GetChunk(player.GlobalPosition));
    }
    public override void OnTurnStart()//pradzioj ejimo
    {
        base.OnTurnStart();
        isAbilityActive = false;
        ChunkData current = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        (int x, int y) indexes = current.GetIndexes();
        var pushDirectionVectors = new List<(int, int)>
        {
            (indexes.x, indexes.y),
            (attackRange + indexes.x, indexes.y),
            (indexes.x, attackRange + indexes.y),
            (-attackRange + indexes.x, indexes.y),
            (indexes.x, -attackRange + indexes.y)
        };
        foreach (var vector in pushDirectionVectors)
        {
            if (GameTileMap.Tilemap.CheckBounds(vector.Item1, vector.Item2))
            {
                ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(vector.Item1, vector.Item2);
                if (IsAllegianceSame(chunkData) && chunkData.GetCurrentPlayer() != null)
                {
                    int randomHeal = _random.Next(minHeal, maxHeal);
                    chunkData.GetCurrentPlayer().playerInformation.Heal(randomHeal);
                }
            }
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        base.ResolveAbility(chunk);
        isAbilityActive = true;
        FinishAbility();
    }

}
