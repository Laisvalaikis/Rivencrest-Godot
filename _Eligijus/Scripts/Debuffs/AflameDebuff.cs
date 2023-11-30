using System;
using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class AflameDebuff : BaseDebuff
{
    [Export] private int minDamage;
    [Export] private int maxDamage;

    public AflameDebuff()
    {
			
    }
    
    public AflameDebuff(AflameDebuff debuff): base(debuff)
    {
        minDamage = debuff.minDamage;
        maxDamage = debuff.maxDamage;
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        AflameDebuff debuff = new AflameDebuff((AflameDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        AflameDebuff debuff = new AflameDebuff(this);
        return debuff;
    }
    
    private void DamageArea(ChunkData centerChunk)
    {
        Random random = new Random();
        int randomDamage;
        ChunkData[,] chunks = GameTileMap.Tilemap.GetChunksArray();
        (int x, int y) indexes = centerChunk.GetIndexes();
        int x = indexes.x;
        int y = indexes.y;

        int[] dx = { 0, 0, 1, -1 };
        int[] dy = { 1, -1, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];
            Player player = chunks[nx, ny].GetCurrentPlayer();
            if (GameTileMap.Tilemap.CheckBounds(nx, ny) && player != null)
            {
                randomDamage = random.Next(minDamage, maxDamage);
                player.objectInformation.GetPlayerInformation().DealDamage(randomDamage, playerWhoCreatedDebuff);
            }
        }
        randomDamage = random.Next(minDamage, maxDamage);
        _player.objectInformation.GetPlayerInformation().DealDamage(randomDamage, playerWhoCreatedDebuff);
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        DamageArea(GameTileMap.Tilemap.GetChunk(_player.GlobalPosition));
        lifetimeCount++;
    }
}