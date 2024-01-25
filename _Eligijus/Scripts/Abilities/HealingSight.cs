using System;
using Godot;

public partial class HealingSight : BaseAction
{
    [Export]
    public int minHealAmount = 3;
    [Export]
    public int maxHealAmount = 7;
    [Export] 
    public int visionRangeIncrease = 2;

    private bool abilityUsed = false;
    public HealingSight()
    {
 		
    }
    public HealingSight(HealingSight healingSight): base(healingSight)
    {
        minHealAmount = healingSight.minHealAmount;
        maxHealAmount = healingSight.maxHealAmount;
        visionRangeIncrease = healingSight.visionRangeIncrease;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        HealingSight healSingle = new HealingSight((HealingSight)action);
        return healSingle;
    }
    public override void OnTurnStart(ChunkData chunkData)
    {
        base.OnTurnStart(chunkData);
        if (abilityUsed)
        {
            _player.IncreaseVisionRange(visionRangeIncrease*(-1));
            abilityUsed = false;
        }
    }

    public override void ResolveAbility(ChunkData chunk)
    {
        if (CanTileBeClicked(chunk))
        {
            abilityUsed = true;
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            Random random = new Random();
            int randomHeal = random.Next(minHealAmount, maxHealAmount);
            _player.objectInformation.GetPlayerInformation().Heal(randomHeal);
            _player.IncreaseVisionRange(visionRangeIncrease);
            GameTileMap.Tilemap.UpdateFog(this,_player);
            FinishAbility();
        }
    }
}