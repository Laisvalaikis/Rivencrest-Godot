using System.Collections.Generic;
using Godot;

public partial class IncreaseVision : BaseAction
{
    [Export] private int vision = 3;
    private List<ChunkData> _savedViewTiles;
    
    public IncreaseVision()
    {
        
    }
    
    public IncreaseVision(IncreaseVision ability): base(ability)
    {
        vision = ability.vision;
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
        GameTileMap.Tilemap.UpdateFog(this, player);
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
            FinishAbility();
        }
    }

    public override bool CanTileBeClicked(ChunkData chunkData)
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
    
    
}