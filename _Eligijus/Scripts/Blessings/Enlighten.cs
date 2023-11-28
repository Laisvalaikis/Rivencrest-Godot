using System;
using Godot;

public partial class Enlighten : AbilityBlessing
{

    [Export] private int minHeal = 3;
    [Export] private int maxHeal = 5;
    private Random _random;
    
    public Enlighten()
    {
			
    }
    
    public Enlighten(Enlighten blessing): base(blessing)
    {
        _random = new Random();
        minHeal = blessing.minHeal;
        maxHeal = blessing.maxHeal;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Enlighten blessing = new Enlighten((Enlighten)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Enlighten blessing = new Enlighten(this);
        return blessing;
    }
    
    public override void ResolveBlessing(BaseAction baseAction, ChunkData tile)
    {
        base.ResolveBlessing(baseAction, tile);
        if (tile.CharacterIsOnTile())
        {
            int heal = _random.Next(minHeal, maxHeal);
            tile.GetCurrentPlayer().objectInformation.GetPlayerInformation().Heal(heal);
        }
    }
    
}