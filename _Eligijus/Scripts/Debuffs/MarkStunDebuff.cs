using Godot;
using System;
using System.Threading.Tasks;

namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class MarkStunDebuff: BaseDebuff
{
    public MarkStunDebuff()
    {
        debuffAnimation = "MarkIdle";
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
    

    public override async void OnRemove()
    {
        ChangeAnimation("MarkExplode");
        AnimationPlayer animationPlayer = animatedObject.objectInformation.GetObjectInformation().animationPlayer;
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("MarkExplode").Length-0.1f));
        base.OnRemove();
    }
    
    public override void PlayerWasAttacked()
    {
        PlayerStun stunDebuff = new PlayerStun();
        _player.debuffManager.AddDebuff(stunDebuff, playerWhoCreatedDebuff);
    }
}