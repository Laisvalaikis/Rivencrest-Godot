using Godot;
using System;
using System.Threading.Tasks;

public partial class BaseDebuff : Resource
{
	
	[Export]
	protected int lifetime = 1;
	protected int lifetimeCount = 0;
	protected Player _player;
	protected Player playerWhoCreatedDebuff;
	
	private Resource animatedObjectPrefab;
	private ObjectData animatedObjectPrefabData;
	protected string debuffAnimation="Empty";

	protected Object animatedObject;
	public BaseDebuff()
	{
			
	}

	public BaseDebuff(BaseDebuff blessing)
	{
		lifetime = blessing.lifetime;
	}

	public virtual BaseDebuff CreateNewInstance(BaseDebuff baseBlessing)
	{
		throw new NotImplementedException();
	}

	public virtual BaseDebuff CreateNewInstance()
	{
		throw new NotImplementedException();
	}

	public virtual void OnRemove()
	{
		animatedObject.QueueFree();
	}
	
	public virtual void Start()
	{
		PlayAnimation(debuffAnimation);
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

	public void SetLifetime(int lifetimeTurns)
	{
		lifetime = lifetimeTurns;
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

	public void SetPlayerWhoCreatedDebuff(Player player)
	{
		playerWhoCreatedDebuff = player;
	}

	public override bool Equals(object obj)
	{
		return GetType().Equals(obj);
	}
	
	public bool EqualsType(Type obj)
	{
		return GetType() == obj;
	}

	public override int GetHashCode()
	{
		return GetHashCode();
	}

	public virtual async Task PlayAnimation(string animationName)
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
	
	public virtual async Task ChangeAnimation(string animationName)
	{
		AnimationPlayer animationPlayer = animatedObject.objectInformation.GetObjectInformation().animationPlayer;
		if(animationPlayer != null && animationPlayer.HasAnimation(animationName))
		{
			animationPlayer.Play(animationName);
		}
	}
}
