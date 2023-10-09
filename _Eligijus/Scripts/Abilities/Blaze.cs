using System.Collections.Generic;
using Godot;

public partial class Blaze : BaseAction
{
    [Export] public int bonusDamage = 4;
    public Blaze()
    {

    }

    public Blaze(Blaze blaze) : base(blaze)
    {
        bonusDamage = blaze.bonusDamage;
    }
	
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Blaze blaze = new Blaze((Blaze)action);
        return blaze;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            base.ResolveAbility(chunk);
        }
    }
    

    
    public void TriggerAflame(ChunkData centerChunk, int radius)//pakeisti ji i public override void veliau jei kels problemu
    {
        if (centerChunk != null && centerChunk.GetCurrentPlayerInformation()/*.Aflame*/ != null &&
            centerChunk.GetCurrentPlayerInformation().GetHealth() > 0)
        {
            (int centerX, int centerY) = centerChunk.GetIndexes();

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