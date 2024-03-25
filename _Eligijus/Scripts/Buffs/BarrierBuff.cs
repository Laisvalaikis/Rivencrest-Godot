using System;
using System.Threading.Tasks;
using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BarrierBuff : BaseBuff
{
    public BarrierBuff()
    {
        
    }
    public BarrierBuff(BarrierBuff buff): base(buff)
    {
        buffAnimation = "Barrier"; //this bitch dont exist, cia per parametrus gal pavadinima
    }
    
    public override BaseBuff CreateNewInstance(BaseBuff baseBuff)
    {
        BarrierBuff buff = new BarrierBuff((BarrierBuff)baseBuff);
        return buff;
    }
    
    public override BaseBuff CreateNewInstance()
    {
        BarrierBuff buff = new BarrierBuff(this);
        return buff;
    }
    
    public override async void Start()
    {
        PlayAnimation("ShieldTransition");
        AnimationPlayer animationPlayer = animatedObject.objectInformation.GetObjectInformation().animationPlayer;
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("ShieldTransition").Length-0.1f));
        ChangeAnimation("shield");
    }

    public override async void OnRemove()
    {
        ChangeAnimation("ShieldEnd");
        AnimationPlayer animationPlayer = animatedObject.objectInformation.GetObjectInformation().animationPlayer;
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("ShieldEnd").Length-0.1f));
        base.OnRemove();
    }
    
    public override void ResolveBuff(ChunkData chunkData)
    {
        
    }
    
    public override void ModifyDamage(ref int damage, ref Player player)
    {
        damage = 0;
    }
}