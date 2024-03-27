using System;
using Godot;

public partial class ThrowBehind : BaseAction
{

    public ThrowBehind()
    {
    }

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
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        if (!IsAllegianceSame(chunk))
        {
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        }
        PlayerAbilityAnimation();
        MoveCharacter(chunk);
        FinishAbility();
    }

    private void MoveCharacter(ChunkData target)
    {
        ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        (int x, int y) playerChunkIndex = playerChunk.GetIndexes();
        (int x, int y) targetChunkIndex = target.GetIndexes();

        (int x, int y) chunkToMoveToIndex = targetChunkIndex;
        
        Side side = ChunkSideByCharacter(playerChunk, target);
        int range;
        if (side == Side.isFront)
        {
            range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            chunkToMoveToIndex.x = playerChunkIndex.x;
            chunkToMoveToIndex.y = playerChunkIndex.y + range;
        }
        else if (side == Side.isBack)
        {
            range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            chunkToMoveToIndex.x = playerChunkIndex.x;
            chunkToMoveToIndex.y = playerChunkIndex.y - range;
        }
        else if (side == Side.isRight)
        {
            range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            chunkToMoveToIndex.y = playerChunkIndex.y;
            chunkToMoveToIndex.x = playerChunkIndex.x - range;

        }
        else if (side == Side.isLeft)
        {
            range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            chunkToMoveToIndex.y = playerChunkIndex.y;
            chunkToMoveToIndex.x = playerChunkIndex.x + range;
        }
        
        ChunkData chunkToMoveTo = GameTileMap.Tilemap.GetChunkDataByIndex(chunkToMoveToIndex.x, chunkToMoveToIndex.y);

        if (GameTileMap.Tilemap.CheckBounds(chunkToMoveToIndex.x, chunkToMoveToIndex.y) && !chunkToMoveTo.CharacterIsOnTile())
        {
            GameTileMap.Tilemap.MoveSelectedCharacter(chunkToMoveTo,target.GetCurrentPlayer());
        }
    }
}
