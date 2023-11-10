using System.Collections;
using System.Collections.Generic;
using Godot;

public partial class Purify : BaseAction
{

    public Purify()
    {
        
    }
    
    public Purify(Purify ability): base(ability)
    {
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        Purify ability = new Purify((Purify)action);
        return ability;
    }
 
    public override void ResolveAbility(ChunkData chunk)
    { 
        base.ResolveAbility(chunk);
        if (chunk.CharacterIsOnTile())
        {
            UpdateAbilityButton();
            Player target = chunk.GetCurrentPlayer();
            target.RemoveWeakSpot();
            target.debuffs.RemoveSilence();
            target.debuffs.RemoveAFlame();
            target.debuffs.RemoveSilence();
            target.ClearPoison();
        }
    }
   
   
}
