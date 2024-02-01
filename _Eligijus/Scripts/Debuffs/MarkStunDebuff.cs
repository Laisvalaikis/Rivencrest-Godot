using Godot;
using System;
namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class MarkStunDebuff: BaseDebuff
{
    public MarkStunDebuff()
    {
        
    }
    public MarkStunDebuff(MarkStunDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        MarkStunDebuff debuff = new MarkStunDebuff((MarkStunDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        MarkStunDebuff debuff = new MarkStunDebuff(this);
        return debuff;
    }
    
    public override void Start()
    {

    }
    
    public override void PlayerWasAttacked()
    {
        PlayerStun stunDebuff = new PlayerStun();
        _player.debuffManager.AddDebuff(stunDebuff, playerWhoCreatedDebuff);
    }
}