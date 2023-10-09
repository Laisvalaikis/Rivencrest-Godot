using System;
using Godot;

public partial class ThrowBehind : BaseAction
{
    private Side _side;
    
    public ThrowBehind(ThrowBehind ability): base(ability)
    {
        
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        ThrowBehind ability = new ThrowBehind((ThrowBehind)action);
        return ability;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        if (GameTileMap.Tilemap.CharacterIsOnTile(chunk))
        {
            base.ResolveAbility(chunk);
            ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition + new Vector2(0, 50f));
            ChunkSideByCharacter(playerChunk, chunk);
            MoveCharacter(playerChunk, chunk);
            // DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }
    

    private void MoveCharacter(ChunkData chunk, ChunkData target)
    {
        (int x, int y) playerChunkIndex = chunk.GetIndexes();
        (int x, int y) targetChunkIndex = target.GetIndexes();
        if (_side == Side.isFront)
        {
            int range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x + range, targetChunkIndex.y) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x + range, targetChunkIndex.y);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector2(0, 50f), target.GetCurrentCharacter());
            }
        }
        else if (_side == Side.isBack)
        {
            int range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x - range, targetChunkIndex.y) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x - range, targetChunkIndex.y);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector2(0, 50f), target.GetCurrentCharacter());
            }
        }
        else if (_side == Side.isRight)
        {
            int range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y - range) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y  - range);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector2(0, 50f), target.GetCurrentCharacter());
            }
        }
        else if (_side == Side.isLeft)
        {
            int range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y + range) != null)
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y  + range);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk.GetPosition(), new Vector2(0, 50f), target.GetCurrentCharacter());
            }
        }
    }

    protected override void FinishAbility()
    {
        _side = Side.none;
        base.FinishAbility();
    }
    
}
