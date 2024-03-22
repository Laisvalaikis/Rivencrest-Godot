using System;
using System.Threading.Tasks;
using Godot;

namespace Rivencrestgodot._Eligijus.Scripts.Debuffs;

public partial class BlockerDebuff : BaseDebuff
{
    
    //Takes damage from enemy attacks
    //This functionality is handled in the player buff BlockedBuff
    //BlockerDebuff exists, because in the future we will want to display
    //All player debuff and buff related information above abilities
    //Change sprites, etc
    public BlockerDebuff()
    {
        
    }
    public BlockerDebuff(BlockerDebuff debuff): base(debuff)
    {
        
    }
    
    public override BaseDebuff CreateNewInstance(BaseDebuff baseDebuff)
    {
        BlockerDebuff debuff = new BlockerDebuff((BlockerDebuff)baseDebuff);
        return debuff;
    }
    
    public override BaseDebuff CreateNewInstance()
    {
        BlockerDebuff debuff = new BlockerDebuff(this);
        return debuff;
    }
    
    public override async void Start()
    {
        _player.CurrentIdleAnimation = "BlockIdle";
        _player.CurrentHitAnimation = "BlockHit";
        AnimationPlayer animationPlayer = _player.objectInformation.GetObjectInformation().animationPlayer;
        animationPlayer.Play("BlockStart");
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("BlockStart").Length-0.1f));
        animationPlayer.Play(_player.CurrentIdleAnimation);
    }

    public override async void OnRemove()
    {
        AnimationPlayer animationPlayer = _player.objectInformation.GetObjectInformation().animationPlayer;
        animationPlayer.Play("BlockToIdle");
        await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("BlockToIdle").Length-0.1f));
        _player.CurrentIdleAnimation = "Idle";
        _player.CurrentHitAnimation = "Hit";
        animationPlayer.Play(_player.CurrentIdleAnimation);
    }
}