using System;
using Godot;

public partial class ThrowBehind : BaseAction
{
    private Side _side;

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
        if (chunk.CharacterIsOnTile())
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
            _side = ChunkSideByCharacter(playerChunk, chunk);
            if (!IsAllegianceSame(chunk))
            {
                DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            }
            MoveCharacter(playerChunk, chunk);
            FinishAbility();
        }
    }
    

    private void MoveCharacter(ChunkData chunk, ChunkData target)
    {
        ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
        (int x, int y) playerChunkIndex = chunk.GetIndexes();
        (int x, int y) targetChunkIndex = target.GetIndexes();
        if (_side == Side.isFront)
        {
            int range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y + range) != null && chunk.CharacterIsOnTile() && GameTileMap.Tilemap.CheckBounds(playerChunkIndex.x,playerChunkIndex.y))
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y + range);
                //GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk, target.GetCurrentPlayer());
                //MovePlayerToSide(positionChunk,targetChunkIndex,target);
            }
        }
        else if (_side == Side.isBack)
        {
            int range = Math.Abs(playerChunkIndex.y - targetChunkIndex.y);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y - range) != null && chunk.CharacterIsOnTile() && GameTileMap.Tilemap.CheckBounds(targetChunkIndex.y,targetChunkIndex.y)) //works
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(targetChunkIndex.x, playerChunkIndex.y  - range);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk, target.GetCurrentPlayer());
            }
        }
        else if (_side == Side.isRight)
        {
            int range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x - range, playerChunkIndex.y) != null && chunk.CharacterIsOnTile() && GameTileMap.Tilemap.CheckBounds(targetChunkIndex.x,targetChunkIndex.x)) //works
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x - range, playerChunkIndex.y);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk, target.GetCurrentPlayer());
            }
        }
        else if (_side == Side.isLeft)
        {
            int range = Math.Abs(playerChunkIndex.x - targetChunkIndex.x);
            if (GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x + range, targetChunkIndex.y) != null && chunk.CharacterIsOnTile() && GameTileMap.Tilemap.CheckBounds(-playerChunkIndex.y,-playerChunkIndex.y))
            {
                ChunkData positionChunk =
                    GameTileMap.Tilemap.GetChunkDataByIndex(playerChunkIndex.x + range, targetChunkIndex.y);
                GameTileMap.Tilemap.MoveSelectedCharacter(positionChunk, target.GetCurrentPlayer());
            }
        }
    }

    protected override void FinishAbility()
    {
        _side = Side.none;
        base.FinishAbility();
    }
    
}
