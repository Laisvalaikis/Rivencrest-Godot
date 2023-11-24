using System.Collections.Generic;
using Godot;

public partial class RainOfArrows : BaseAction
{
    private List<ChunkData> arrowTiles = new List<ChunkData>();
    [Export] private ObjectData arrowTileData;
    [Export] private Resource arrowTilePrefab;
    private List<Object> arrowTileObjects=new List<Object>();
    public RainOfArrows()
    {
        
    }
    public RainOfArrows(RainOfArrows ability): base(ability)
    {
        arrowTilePrefab = ability.arrowTilePrefab;
        arrowTileData = ability.arrowTileData;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        RainOfArrows ability = new RainOfArrows((RainOfArrows)action);
        return ability;
    }
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (arrowTiles.Count!=0)
        {
            foreach (ChunkData tile in arrowTiles)
            {
                DealRandomDamageToTarget(tile, minAttackDamage, maxAttackDamage);
            }
            arrowTiles.Clear();
        }
        if (arrowTileObjects.Count != 0)
        {
            foreach (Object arrowTileObject in arrowTileObjects)
            {
                arrowTileObject.Death();
            }
            arrowTileObjects.Clear();
        }
    }
      
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        List<ChunkData> damageChunks = CreateDamageTileList(chunk);
        foreach (ChunkData chunkData in damageChunks)
        {
            arrowTiles.Add(chunkData);
            PackedScene spawnCharacter = (PackedScene)arrowTilePrefab;
            Object spawnedArrowTile = spawnCharacter.Instantiate<Object>();
            player.GetTree().Root.CallDeferred("add_child", spawnedArrowTile);
            spawnedArrowTile.SetupObject(arrowTileData);
            GameTileMap.Tilemap.SpawnObject(spawnedArrowTile, chunkData);
            arrowTileObjects.Add(spawnedArrowTile);
        }
        FinishAbility();
    }
    
    public List<ChunkData> CreateDamageTileList(ChunkData chunk)
    {
        List<ChunkData> damageTiles = new List<ChunkData>();
        (int x, int y) index = chunk.GetIndexes();
        var spellDirectionVectors = new List<(int, int)>
        {
            (0 + index.x, 0 + index.y),
            (1 + index.x, 0 + index.y),
            (0 + index.x, 1 + index.y),
            (-1 + index.x, 0 + index.y),
            (0 + index.x, -1 + index.y)
        };
        foreach (var direction in spellDirectionVectors)
        {
            if (GameTileMap.Tilemap.CheckBounds(direction.Item1, direction.Item2))
            {
                ChunkData temp = GameTileMap.Tilemap.GetChunkDataByIndex(direction.Item1, direction.Item2);
                damageTiles.Add(temp);
            }
        }
        return damageTiles;
    }
}
