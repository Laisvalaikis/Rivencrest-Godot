using System;
using System.Collections.Generic;
using Godot;

public partial class SummonOrb : BaseAction
{
    [Export] private ObjectData orbData;
    [Export] private Resource orbPrefab;

    private Object orb;
    private PlayerInformation _orbInformation;
    private List<ChunkData> _attackList;
    private bool isSpawned = false;

    public SummonOrb()
    {
    }

    public SummonOrb(SummonOrb ability): base(ability)
    {
        orbPrefab = ability.orbPrefab;
        orbData = ability.orbData;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        SummonOrb ability = new SummonOrb((SummonOrb)action);
        return ability;
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        PlayAnimation("Forest4", chunk);
        SpawnOrb(chunk);
        FinishAbility();
    }

    private void SpawnOrb(ChunkData chunkData)
    {
        PackedScene spawnCharacter = (PackedScene)orbPrefab; 
        orb = spawnCharacter.Instantiate<Object>();
        _player.GetTree().Root.CallDeferred("add_child", orb);
        orb.SetupObject(orbData);
        orb.AddPlayerForObjectAbilities(_player);
        GameTileMap.Tilemap.SpawnObject(orb, chunkData);
        isSpawned = true;
        orb.StartActions();
    }
    
    public void GenerateAttackGrid(ChunkData centerChunk)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        _attackList = new List<ChunkData>();
        int startRadius = 1;
        for (int range = 0; range < attackRange; range++)
        {
            
            int count = startRadius + (range * 2);
            int topLeftCornerX = centerX - range;
            int topLeftCornerY = centerY - range;
            int bottomRightCornerX = centerX + range;
            int bottomRightCornerY = centerY + range;


            for (int i = 0; i < count; i++)
            {
                if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX + i, topLeftCornerY))
                {
                    ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX + i, topLeftCornerY);
                    _chunkList.Add(chunkData);
                   // HighlightGridTile(chunkData);
                    _attackList.Add(chunkData);
                    
                }

                if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX - i, bottomRightCornerY))
                {
                    ChunkData chunkData =
                        GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX - i, bottomRightCornerY);
                    _chunkList.Add(chunkData);
                   // HighlightGridTile(chunkData);
                    _attackList.Add(chunkData);
                }

                if (GameTileMap.Tilemap.CheckBounds(topLeftCornerX, topLeftCornerY + i))
                {
                    ChunkData chunkData = GameTileMap.Tilemap.GetChunkDataByIndex(topLeftCornerX, topLeftCornerY + i);
                    _chunkList.Add(chunkData);
                   // HighlightGridTile(chunkData);
                    _attackList.Add(chunkData);
                }

                if (GameTileMap.Tilemap.CheckBounds(bottomRightCornerX, bottomRightCornerY - i))
                {
                    ChunkData chunkData =
                        GameTileMap.Tilemap.GetChunkDataByIndex(bottomRightCornerX, bottomRightCornerY - i);
                    _chunkList.Add(chunkData);
                   // HighlightGridTile(chunkData);
                    _attackList.Add(chunkData);
                }
            }
            
            
        }
    }

    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (isSpawned)
        {
            ChunkData realChunkfr=GameTileMap.Tilemap.GetChunk(orb.GlobalPosition);
            GenerateAttackGrid(realChunkfr);
            DealRandomDamageToTarget(realChunkfr,4,6);
        }
    }
}
