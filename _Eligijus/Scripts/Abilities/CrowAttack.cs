using System.Collections.Generic;
using Godot;

public partial class CrowAttack : BaseAction
{
    private List<Poison> _poisons;
    [Export]
    public int poisonBonusDamage=2;
    
    public CrowAttack()
    {
		
    }
    public CrowAttack(CrowAttack crowAttack) : base(crowAttack)
    {
        poisonBonusDamage = crowAttack.poisonBonusDamage;
    }
    public override BaseAction CreateNewInstance(BaseAction action)
    {
        CrowAttack createBearTrap = new CrowAttack((CrowAttack)action);
        return createBearTrap;
    }
    
    public override void ResolveAbility(ChunkData chunk)
    {
        foreach (var t in _chunkList)
        {
            if (t.CharacterIsOnTile())
            {
                int bonusDamage = 0;
                if (_poisons.Count > 0)
                {
                    bonusDamage += poisonBonusDamage;
                }
                DealRandomDamageToTarget(t,minAttackDamage + bonusDamage, maxAttackDamage + bonusDamage);
            }
        }
        base.ResolveAbility(chunk);
        FinishAbility();
    }
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        PoisonPlayer();
    }
    private void PoisonPlayer()
    {
        foreach (Poison x in _poisons)
        {
            if (x.poisonValue > 0 && x.chunk.GetCurrentPlayerInformation().GetHealth() > 0)
            {
                DealDamage(x.chunk, x.poisonValue, false);
            }
            x.turnsLeft--;
        }
    }
}