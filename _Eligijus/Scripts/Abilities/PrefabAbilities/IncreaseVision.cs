using System.Collections.Generic;
using Godot;

public partial class IncreaseVision : BaseAction
{
    [Export] private int vision = 3;
    [Export] private bool removeVisionAfterDeath = true;
    private List<ChunkData> _savedViewTiles;
    
    public IncreaseVision()
    {
        
    }
    
    public IncreaseVision(IncreaseVision ability): base(ability)
    {
        vision = ability.vision;
        removeVisionAfterDeath = ability.removeVisionAfterDeath;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        IncreaseVision ability = new IncreaseVision((IncreaseVision)action);
        return ability;
    }
    
    public override List<ChunkData> GetVisionChunkList()
    {
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(_object.GlobalPosition);
        _visionChunkList = GenerateVisionPattern(startChunk, vision);
        return _visionChunkList;
    }

    public override void StartAction()
    {
        base.StartAction();
        ChunkData startChunk = GameTileMap.Tilemap.GetChunk(_object.GlobalPosition);
        _savedViewTiles = GenerateVisionPattern(startChunk, vision);
        GameTileMap.Tilemap.UpdateFog(this, _player);
    }

    public override bool CanBeUsedOnTile(ChunkData chunkData)
    {
        if (CheckIfSpecificInformationType(chunkData, typeof(Player)) 
            || CheckIfSpecificInformationType(chunkData, typeof(Object)))
        {
            return true;
        }
        return false;
    }
    
    public override bool IsAllegianceSame(ChunkData chunk)
    {
        return false;
    }

    public override void Die()
    {
        if (_savedViewTiles is not null && removeVisionAfterDeath)
        {
            for (int i = 0; i < _savedViewTiles.Count; i++)
            {
                GameTileMap.Tilemap.AddFog(_savedViewTiles[i], _player);
            }
        }
    }

}