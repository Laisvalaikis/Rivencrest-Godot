using Godot;
using System;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

//Player is marked, if damage is dealt to him, he becomes rooted
//NEEDS FIXING
public partial class MarkDebuff: BaseDebuff
{
    public MarkDebuff()
    {
        
    }
    public MarkDebuff(MarkDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        MarkDebuff debuff = new MarkDebuff((MarkDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        MarkDebuff debuff = new MarkDebuff(this);
        return debuff;
    }
    
    public override void Start()
    {

    }
    
    public override void PlayerWasAttacked()
    {
        
        RootDebuff rootDebuff = new RootDebuff();
        _player.debuffManager.AddDebuff(rootDebuff, playerWhoCreatedDebuff);
    }
}