using System.Collections.Generic;
using Godot;

public partial class CometFall : BaseAction
{
    [Export] private ObjectData cometTileData;
    [Export] private Resource cometTilePrefab;
    private Object spawnedCometTile;
    private List<ChunkData> _damageTiles = new List<ChunkData>();

    public CometFall()
    {
        
    }
    public CometFall(CometFall ability): base(ability)
    {
        cometTilePrefab = ability.cometTilePrefab;
        cometTileData = ability.cometTileData;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CometFall ability = new CometFall((CometFall)action);
        return ability;
    }
    
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (_damageTiles.Count > 0)
        {
            foreach (ChunkData chunk in _damageTiles)
            {
                //Enemy
                if (CheckIfSpecificInformationType(chunk, typeof(Player)) && !IsAllegianceSame(chunk))
                {
                    DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
                }
                //Ally
                else if (CheckIfSpecificInformationType(chunk, typeof(Player)) && IsAllegianceSame(chunk))
                {
                    DealRandomDamageToTarget(chunk, minAttackDamage/3, maxAttackDamage/3);
                }
            }
            spawnedCometTile.Death();
            _damageTiles.Clear();
        }
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        _damageTiles.Clear();
        _damageTiles.Add(chunk);
        PackedScene spawnCharacter = (PackedScene)cometTilePrefab;
        spawnedCometTile = spawnCharacter.Instantiate<Object>();
        player.GetTree().Root.CallDeferred("add_child", spawnedCometTile);
        spawnedCometTile.SetupObject(cometTileData);
        GameTileMap.Tilemap.SpawnObject(spawnedCometTile, chunk);
        FinishAbility();
    }

    public override bool CanTileBeClicked(ChunkData chunkData)
    {
        return chunkData.GetTileHighlight().isHighlighted; //might be bullshit
    }
}
