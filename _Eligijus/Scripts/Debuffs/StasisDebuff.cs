using System;
using System.Threading.Tasks;
using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

//Reduces movement points of player by (two/2?)
public partial class StasisDebuff : BaseDebuff
{
    public StasisDebuff()
    {
			
    }
    
    public StasisDebuff(int lifetime)
    {
        this.lifetime = lifetime;
    }
    
    public StasisDebuff(StasisDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        StasisDebuff debuff = new StasisDebuff((StasisDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        StasisDebuff debuff = new StasisDebuff(this);
        return debuff;
    }
	
    public override async void Start()
    {
        _player.CurrentHitAnimation = "StasisHit";
        _player.CurrentIdleAnimation = "StasisIdle";
        AnimationPlayer animationPlayer = _player.objectInformation.GetObjectInformation().animationPlayer;
        animationPlayer.Play("StasisStart");
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("StasisStart").Length-0.1f));
        animationPlayer.Play(_player.CurrentIdleAnimation);
    }

    public override async void OnRemove()
    {
        _player.CurrentHitAnimation = "Hit";
        _player.CurrentIdleAnimation = "Idle";
        AnimationPlayer animationPlayer = _player.objectInformation.GetObjectInformation().animationPlayer;
        await Task.Delay(TimeSpan.FromSeconds(0.4f));
        animationPlayer.Play("StasisExplode");
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("StasisExplode").Length-0.1f));
        animationPlayer.Play(_player.CurrentIdleAnimation);
    }
    
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (lifetime != lifetimeCount)
        {
            _player.SetMovementPoints(0);
            _player.actionManager.RemoveAllActionPoints();
        }
    }
}