﻿
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class Antitoxic : PlayerBlessing
{
    public Antitoxic()
    {
			
    }
    
    public Antitoxic(Antitoxic blessing): base(blessing)
    {

    }
    
    public override BaseBlessing CreateNewInstance(BaseBlessing baseBlessing)
    {
        Antitoxic blessing = new Antitoxic((Antitoxic)baseBlessing);
        return blessing;
    }
    
    public override BaseBlessing CreateNewInstance()
    {
        Antitoxic blessing = new Antitoxic(this);
        return blessing;
    }
    
    
    public override void Start(Player player)
    {
        base.Start(player);
        PoisonDebuff poisonDebuff = new PoisonDebuff(2,2); // :(
        player.debuffManager.RemoveDebuffsByType(poisonDebuff.GetType());
    }
}