using System.Collections.Generic;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class PowerShot : BaseAction
{
    public PowerShot()
    {
        
    }
    public PowerShot(PowerShot ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PowerShot ability = new PowerShot((PowerShot)action);
        return ability;
    }
    protected override void GeneratePlusPattern(ChunkData centerChunk, int length)
    {
        (int centerX, int centerY) = centerChunk.GetIndexes();
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
        bool[] canExtend = { true, true, true, true };

        for (int i = 1; i <= length; i++)
        {
            List<(int, int, int)> positions = new List<(int, int, int)> 
            {
                (centerX, centerY + i, 0),  // Up
                (centerX, centerY - i, 1),  // Down
                (centerX + i, centerY, 2),  // Right
                (centerX - i, centerY, 3)   // Left
            };
            foreach (var (x, y, direction) in positions)
            {
                if (!canExtend[direction])
                {
                    continue;
                }
                if (x >= 0 && x < chunksArray.GetLength(0) && y >= 0 && y < chunksArray.GetLength(1))
                {
                    ChunkData chunk = chunksArray[x, y];
                    if (chunk != null && !chunk.TileIsLocked())
                    {
                        if (chunk.GetCharacterType() == typeof(Object))
                        {
                            canExtend[direction] = false;
                            continue;
                        }
                        _chunkList.Add(chunk);
                        if (chunk.GetCurrentPlayer() != null)
                        {
                            canExtend[direction] = false;
                        }
                    }
                }
            }
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        int bonusDamage = 0;
        DealRandomDamageToTarget(chunk, minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
        if (chunk.CharacterIsOnTile())
        {
           // Player target = chunk.GetCurrentPlayer();
            SlowDebuff debuff = new SlowDebuff(2,2);
            chunk.GetCurrentPlayer().debuffManager.AddDebuff(debuff,_player);
        }
        FinishAbility();
    }
}