using Godot;
using System;
using System.Threading.Tasks;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

//Player is marked, if damage is dealt to him, he becomes rooted
public partial class MarkDebuff: BaseDebuff
{
    public MarkDebuff()
    {
        debuffAnimation = "MarkIdle";
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

    public override async void OnRemove()
    {
        ChangeAnimation("MarkExplode");
        AnimationPlayer animationPlayer = animatedObject.objectInformation.GetObjectInformation().animationPlayer;
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("MarkExplode").Length-0.1f));
        base.OnRemove();
    }
    
    public override void PlayerWasAttacked()
    {
        RootDebuff rootDebuff = new RootDebuff(1,"CMBurgundy");//?
        _player.debuffManager.AddDebuff(rootDebuff, playerWhoCreatedDebuff);
    }
}