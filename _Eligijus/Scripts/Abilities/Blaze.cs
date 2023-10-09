using System.Collections.Generic;

namespace Rivencrestgodot._Eligijus.Scripts.Abilities;

public partial class Blaze : BaseAction
{
    public int bonusDamage = 4;

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
            // ChunkData chunkData = GetSpecificGroundTile(position);
            // bool aflame = chunkData.GetCurrentPlayerInformation().Aflame != null;
            // if (!aflame)
            // {
            //     chunkData.GetCurrentPlayerInformation().Aflame = gameObject;
            // }
            // else
            // {
            //     TriggerAflame(target);
            // }
            //grizti prie aflame veliau
        }
    }
    
    public Blaze()
    {

    }

    public Blaze(Blaze blaze) : base(blaze)
    {
        bonusDamage = blaze.bonusDamage; //not sure if we need this, prob not
    }
	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Blaze blaze = new Blaze((Blaze)action);
        return blaze;
    }
    
    public void TriggerAflame(ChunkData centerChunk, int radius)//pakeisti ji i public override void veliau jei kels problemu
    {
        if (centerChunk != null && centerChunk.GetCurrentPlayerInformation()/*.Aflame*/ != null &&
            centerChunk.GetCurrentPlayerInformation().GetHealth() > 0)
        {
            (int centerX, int centerY) = centerChunk.GetIndexes();
            // GameTileMap.Tilemap.EnableAllTiles();

            for (int i = 1; i <= radius; i++)
            {
                List<(int, int)> positions = new List<(int, int)>
                {
                    (centerX, centerY + i), // Up
                    (centerX, centerY - i), // Down
                    (centerX + i, centerY), // Right
                    (centerX - i, centerY) // Left
                };
            }
        }
    }
}