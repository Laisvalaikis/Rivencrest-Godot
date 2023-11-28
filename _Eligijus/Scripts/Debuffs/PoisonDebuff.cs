namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class PoisonDebuff : BaseDebuff
{
    private int poisonDamage = 0;
    
    public PoisonDebuff()
    {

    }
    public PoisonDebuff(PoisonDebuff debuff) : base(debuff)
    {

    }
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        PoisonDebuff debuff = new PoisonDebuff((PoisonDebuff)baseDebuff);
        return debuff;
    }
    public override BaseDebuff CreateNewInstance()
    {
        PoisonDebuff debuff = new PoisonDebuff(this);
        return debuff;
    }
    public void SetPoisonDebuffDamage(int damage)
    {
        poisonDamage = damage;
    }
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        _player.playerInformation.DealDamage(poisonDamage, playerWhoCreatedDebuff);
        lifetimeCount++;
    }
}