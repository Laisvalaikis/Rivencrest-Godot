using Godot;
using System;
using System.Threading.Tasks;


public partial class PlayerAttack : BaseAction
{
    [Export]
    public Animation AflameExplosionAnimation;
    [Export] private Resource animatedObjectPrefab;
    [Export] private ObjectData animatedObjectPrefabData;
    private Object animatedObject;

    public PlayerAttack()
    {
        
    }

    public PlayerAttack(PlayerAttack ability): base(ability)
    {
        animatedObjectPrefab = ability.animatedObjectPrefab;
        animatedObjectPrefabData = ability.animatedObjectPrefabData;
    }

    public override BaseAction CreateNewInstance(BaseAction action)
    {
        PlayerAttack ability = new PlayerAttack((PlayerAttack)action);
        return ability;
    }

    public override async void ResolveAbility(ChunkData chunk)
    {
        UpdateAbilityButton();
        base.ResolveAbility(chunk);
        DealRandomDamageToTarget(chunk, minAttackDamage, maxAttackDamage);
        
        PackedScene spawnCharacter = (PackedScene)animatedObjectPrefab; 
        animatedObject = spawnCharacter.Instantiate<Object>(); 
        _player.GetTree().Root.CallDeferred("add_child", animatedObject); 
        animatedObject.SetupObject(animatedObjectPrefabData); 
        animatedObject.AddPlayerForObjectAbilities(_player); 
        GameTileMap.Tilemap.SpawnObject(animatedObject, chunk); 
        
        AnimationPlayer animationPlayer = animatedObject.GetNode<AnimationPlayer>("AnimationPlayer");
        if(animationPlayer != null)
        {
            animationPlayer.Play("AflameExplosion");
            await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation("AflameExplosion").Length));
        }
        animatedObject.QueueFree();
        
        FinishAbility();
    }
    
}
