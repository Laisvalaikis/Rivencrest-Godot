using System;
using Godot;

public partial class HealAfterAbility : AbilityBlessing
{
    [Export] private int minHeal = 3;
    [Export] private int maxHeal = 4;
    private Random _random;
    
    public HealAfterAbility()
    {
			
    }
    
    public HealAfterAbility(HealAfterAbility blessing): base(blessing)
    {
        _random = new Random();
        minHeal = blessing.minHeal;
        maxHeal = blessing.maxHeal;
    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        HealAfterAbility blessing = new HealAfterAbility((HealAfterAbility)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        HealAfterAbility blessing = new HealAfterAbility(this);
        return blessing;
    }

    public override void OnTurnStart(ref BaseAction baseAction, ChunkData tile)
    {
        base.OnTurnEnd(ref baseAction, tile);
        if (tile != null && tile.GetCurrentPlayer() != null)
        {
            Player player = tile.GetCurrentPlayer();
            if (IsAllegianceSame(baseAction.GetPlayer().playerInformation, tile, baseAction))
            {
                int randomHeal = _random.Next(minHeal, maxHeal);
                player.playerInformation.Heal(randomHeal);
            }
        }
    }
}