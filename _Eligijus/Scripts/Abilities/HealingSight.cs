using System;
using Godot;

public partial class HealingSight : BaseAction
{
    [Export]
    public int minHealAmount = 3;
    [Export]
    public int maxHealAmount = 7;
    
    public HealingSight()
    {
 		
    }

    public override void EnableDamagePreview(ChunkData chunk, string text = null)
    {
        //intentionally left empty
    }
    protected override void TryAddTile(ChunkData chunk)
    {
        if (chunk != null && !chunk.TileIsLocked() && chunk.GetCurrentPlayer() == GameTileMap.Tilemap.GetCurrentCharacter())
        {
            _chunkList.Add(chunk);
        }
    }
    public HealingSight(HealingSight healingSight): base(healingSight)
    {
        minHealAmount = healingSight.minHealAmount;
        maxHealAmount = healingSight.maxHealAmount;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        HealingSight healSingle = new HealingSight((HealingSight)action);
        return healSingle;
    }
    public override void ResolveAbility(ChunkData chunk)
    {
        if (_chunkList.Contains(chunk))
        {
            UpdateAbilityButton();
            base.ResolveAbility(chunk);
            Random random = new Random();
            int randomHeal = random.Next(minHealAmount, maxHealAmount);
            if (chunk.CharacterIsOnTile() && IsAllegianceSame(chunk))
            {
                chunk.GetCurrentPlayer().objectInformation.GetPlayerInformation().Heal(randomHeal);
            }
            FinishAbility();
        }
    }
}