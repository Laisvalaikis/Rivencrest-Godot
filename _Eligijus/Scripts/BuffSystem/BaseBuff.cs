using System;
using System.Threading.Tasks;
using Godot;
using Rivencrestgodot._Eligijus.Scripts.Debuffs;

namespace Rivencrestgodot._Eligijus.Scripts.BuffSystem;

public partial class BaseBuff : Resource
{
	[Export]
	protected int lifetime = 1;
	protected int lifetimeCount = 0;
	protected Player _player;
	
	private Resource animatedObjectPrefab;
	private ObjectData animatedObjectPrefabData;
	protected string buffAnimation;
	private Object animatedObject;
		
	public BaseBuff()
	{
				
	}
	
	public BaseBuff(BaseBuff buff)
	{ 
		lifetime = buff.lifetime;
	}
	
	public virtual BaseBuff CreateNewInstance(BaseBuff baseBuff)
	{
		throw new NotImplementedException();
	}
	
	public virtual BaseBuff CreateNewInstance()
	{
		throw new NotImplementedException();
	}
		
	public virtual void Start()
	{
	
	}
		
	public virtual void ResolveBuff(ChunkData chunkData)
	{
			
	}
		
	public virtual void OnTurnStart()
	{
		IncreaseLifetimeCount(1);
	}
	
	public virtual void OnTurnEnd()
	{
	}
		
	public virtual void PlayerWasAttacked()
	{
				
	}
		
	public virtual void PlayerDied()
	{
				
	}
	
	public int GetLifetime()
	{ 
		return lifetime;
	}
	
	public int GetLifetimeCounter()
	{
		return lifetimeCount;
	}
	
	public void IncreaseLifetimeCount(int count)
	{
		lifetimeCount += count;
	}
	
	public void ResetLifetimeCount()
	{ 
		lifetimeCount = 0;
	}
	
	public void SetPLayer(Player player)
	{
		_player = player;
	}
	public virtual void ModifyDamage(ref int damage, ref Player player)
	{
		
	}

	public virtual void ModifyMovement(ref int movementPoints)
	{
		
	}

	public virtual void ModifyDebuff(ref BaseDebuff debuff)
	{
		
	}

	public virtual void ModifyBuff(ref BaseBuff buff)
	{
		//If we have a buff that makes it so adding another buff is different. 
	}
	public virtual void OnRemove()
	{
		animatedObject.QueueFree();
	}
	
	public virtual async Task PlayAnimation(string animationName, ChunkData chunk)
	{
		animatedObjectPrefab = _player.debuffManager.animatedObjectPrefab;
		animatedObjectPrefabData = _player.debuffManager.animatedObjectPrefabData;
		PackedScene spawnCharacter = (PackedScene)animatedObjectPrefab; 
		animatedObject = spawnCharacter.Instantiate<Object>(); 
		_player.CallDeferred("add_child", animatedObject); 
		animatedObject.SetupObject(animatedObjectPrefabData);
		
		animatedObject.Position = new Vector2(0, 0);
		AnimationPlayer animationPlayer = animatedObject.objectInformation.GetObjectInformation().animationPlayer;
		if(animationPlayer != null && animationPlayer.HasAnimation(animationName))
		{
			animationPlayer.Play(animationName);
		}
	}
}
