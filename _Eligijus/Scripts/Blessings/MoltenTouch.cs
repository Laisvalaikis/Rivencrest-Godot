﻿using Godot;
using System.Collections.Generic;

public partial class MoltenTouch : AbilityBlessing
{
    [Export] private int criticalDamage;
    
    public MoltenTouch()
    {
			
    }
    
    public MoltenTouch(MoltenTouch blessing): base(blessing)
    {
        criticalDamage = blessing.criticalDamage;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        MoltenTouch blessing = new MoltenTouch((MoltenTouch)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        MoltenTouch blessing = new MoltenTouch(this);
        return blessing;
    }
    
    public override void ResolveBlessing(ref BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(ref baseAction);
        if (tile.CharacterIsOnTile())
        {
            Player player = tile.GetCurrentPlayer();
            player.debuffs.AddAFlame();
        }

        Aflame(ref baseAction, tile);

    }
    
    
    private void Aflame(ref BaseAction baseAction, ChunkData tile)
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
                if (IsAllegianceSame(player.playerInformation, chunkData, baseAction))
                {
                    DealRandomDamageToTarget(baseAction.GetPlayer().playerInformation, chunkData, baseAction, baseAction.minAttackDamage, baseAction.maxAttackDamage);
                }
            }
        }
        if (tile.CharacterIsOnTile())
        {
            DealRandomDamageToTarget(baseAction.GetPlayer().playerInformation, tile, baseAction, baseAction.minAttackDamage, baseAction.maxAttackDamage);
        }
    }
    
}