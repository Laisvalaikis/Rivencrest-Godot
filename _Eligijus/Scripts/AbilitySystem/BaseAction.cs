using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

public abstract partial class BaseAction: TileAction
	{
		[Export] protected int turnBeforeStartLifetime = 1;
		[Export] protected int turnAfterResolveLifetime = 1;
		[Export] protected int abilityPoints = 1;
		[Export] public bool isAbilitySlow = true;
		[Export] protected int abilityCooldown = 1;
		[Export] protected Array<AbilityBlessing> _abilityBlessingsRef;
		protected Array<AbilityBlessing> _abilityBlessingsCreated;
		protected Array<UnlockedBlessingsResource> unlockedBlessingsList;
		protected bool turinIsEven = false;
		protected int cooldownCount = 0;
		protected SelectActionButton _selectActionButton;
		private bool firstTimeUsage = false;
		protected ObjectData _objectData;
		protected Object _object;
		private Random _random;

		[Export] public string AttackAnimation = "Attack";
		[Export] public Resource animatedObjectPrefab;
		[Export] public ObjectData animatedObjectPrefabData;

		public BaseAction()
		{
			
		}

		public BaseAction(BaseAction action) : base(action)
		{
			laserGrid = action.laserGrid;
			turnBeforeStartLifetime = action.turnBeforeStartLifetime;
			turnAfterResolveLifetime = action.turnAfterResolveLifetime;
			abilityPoints = action.abilityPoints;
			attackRange = action.attackRange;
			minAttackDamage = action.minAttackDamage;
			maxAttackDamage = action.maxAttackDamage;
			isAbilitySlow = action.isAbilitySlow;
			friendlyFire = action.friendlyFire;
			abilityCooldown = action.abilityCooldown;
			abilityHighlight = action.abilityHighlight;
			abilityHighlightHover = action.abilityHighlightHover;
			abilityHoverCharacter = action.abilityHoverCharacter;
			characterOnGrid = action.characterOnGrid;
			otherTeamAbilityHighlight = action.otherTeamAbilityHighlight;
			otherTeamHighlightHover = action.otherTeamHighlightHover;
			otherTeamHoverCharacter = action.otherTeamHoverCharacter;
			otherTeamCharacterOnGrid = action.otherTeamCharacterOnGrid;
			firstTimeUsage = true;
			_abilityBlessingsRef = action._abilityBlessingsRef;
			animatedObjectPrefab = action.animatedObjectPrefab;
			animatedObjectPrefabData = action.animatedObjectPrefabData;
			AttackAnimation = action.AttackAnimation;
		}

		public virtual BaseAction CreateNewInstance(BaseAction action)
		{
			throw new NotImplementedException();
		}

		public void SetupPlayerAbility(Player player, int abilityIndex)
		{
			_player = player;
			if (player.unlockedBlessingList.Count > abilityIndex)
			{
				unlockedBlessingsList = player.unlockedBlessingList[abilityIndex].UnlockedBlessingsList;
			}
			else
			{
				unlockedBlessingsList = new Array<UnlockedBlessingsResource>();
			}
			SetObjectInformationData(player.objectInformation.GetPlayerInformation().objectData.GetPlayerInformationData());
			
			Start();
		}

		public void SetupObjectAbility(Object player)
		{
			unlockedBlessingsList = new Array<UnlockedBlessingsResource>();
			for (int i = 0; i < _abilityBlessingsRef.Count; i++)
			{
				UnlockedBlessingsResource resource = new UnlockedBlessingsResource(_abilityBlessingsRef[i]);
				resource.blessingUnlocked = true;
				unlockedBlessingsList.Add(resource);
			}
			SetObjectInformationData(player.ObjectDataType.GetObjectData());
			_object = player;
			Start();
		}

		private void SetObjectInformationData(ObjectData data)
		{
			_objectData = new ObjectData();
			_objectData.CopyData(data);
		}

		protected override void Start()
		{
			base.Start();
			_random = new Random();
			if (_abilityBlessingsRef != null)
			{
				for (int i = 0; i < _abilityBlessingsRef.Count; i++)
				{
					if (_abilityBlessingsCreated == null)
					{
						_abilityBlessingsCreated = new Array<AbilityBlessing>();
						_abilityBlessingsCreated.Add((AbilityBlessing)_abilityBlessingsRef[i].CreateNewInstance());
					}
					else
					{
						_abilityBlessingsCreated.Add((AbilityBlessing)_abilityBlessingsRef[i].CreateNewInstance());
					}
				}
			}
		}

		public virtual void StartAction()
		{
			
		}

		public virtual void PlayerWasAttacked()
		{
			
		}

		public int GetAbilityPoints()
		{
			return abilityPoints;
		}
		
		public Array<AbilityBlessing> GetAllBlessings()
		{
			return _abilityBlessingsRef;
		}
		
		public int GetAbilityCooldown()
		{
			return abilityCooldown;
		}
		
		public int IncreaseAbilityCooldown()
		{
			return cooldownCount++;
		}

		public void ReduceAbilityCooldown(int reduceCount)
		{
			 cooldownCount+=reduceCount;
		}
		
		protected Side ChunkSideByCharacter(ChunkData playerChunk, ChunkData chunkDataTarget)
		{
			(int x, int y) playerChunkIndex = playerChunk.GetIndexes();
			(int x, int y) chunkIndex = chunkDataTarget.GetIndexes();
			if (playerChunkIndex.y > chunkIndex.y)
			{
				return Side.isFront;
			}
			else if (playerChunkIndex.y < chunkIndex.y)
			{
				return Side.isBack;
			}
			else if (playerChunkIndex.x < chunkIndex.x)
			{
				return Side.isRight;
			}
			else if (playerChunkIndex.x > chunkIndex.x)
			{
				return Side.isLeft;
			}
			return Side.none;
		}

		protected (int, int) GetSideVector(Side side)
		{
			(int, int) sideVector = (0, 0);
			switch (side)
			{
				case Side.isFront:
					sideVector = (0, -1);
					break;
				case Side.isBack:
					sideVector = (0, 1);
					break;
				case Side.isRight:
					sideVector = (1, 0);
					break;
				case Side.isLeft:
					sideVector = (-1, 0);
					break;
				case Side.none:
					sideVector = (0, 0);
					break;
			}
			return sideVector;
		}

		protected void MovePlayerToSide(ChunkData player, (int, int) sideVector, ChunkData positionTile = null)
		{
			if (player.CharacterIsOnTile())
			{
				(int x, int y) indexes = player.GetIndexes();
				if (positionTile != null)
				{
					indexes = positionTile.GetIndexes();
				}

				(int, int) tempIndexes = new(indexes.x + sideVector.Item1, indexes.y + sideVector.Item2);
				if (GameTileMap.Tilemap.CheckBounds(tempIndexes.Item1, tempIndexes.Item2))
				{
					ChunkData tempTile = GameTileMap.Tilemap.GetChunkDataByIndex(tempIndexes.Item1, tempIndexes.Item2);
					if (!tempTile.CharacterIsOnTile())
					{
						GameTileMap.Tilemap.MoveSelectedCharacter(tempTile, player.GetCurrentPlayer());
					}
				}
			}
		}

		public virtual void OnBeforeStart(ChunkData chunkData)
		{
			
		}

		public virtual void OnTurnStart(ChunkData chunkData)
		{
			if (firstTimeUsage)
			{
				cooldownCount = abilityCooldown;
				firstTimeUsage = false;
			}
			
		}

		public virtual void BlessingOnTurnStart(ChunkData chunkData)
		{
			if (_abilityBlessingsCreated != null)
			{
				for (int i = 0; i < _abilityBlessingsCreated.Count; i++)
				{
					if (unlockedBlessingsList[i].blessingUnlocked)
					{
						_abilityBlessingsCreated[i].OnTurnStart(this);
						_abilityBlessingsCreated[i].OnTurnStart(this, chunkData);
					}
				}
			}
		}

		public virtual void OnTurnEnd(ChunkData chunkData)
		{
			cooldownCount++;
			if (!turinIsEven)
			{
				turinIsEven = true;
			}
			else
			{
				turinIsEven = false;
			}
		}
		
		public virtual void BlessingOnTurnEnd(ChunkData chunkData)
		{
			if (_abilityBlessingsCreated != null)
			{
				for (int i = 0; i < _abilityBlessingsCreated.Count; i++)
				{
					if (unlockedBlessingsList[i].blessingUnlocked)
					{
						_abilityBlessingsCreated[i].OnTurnEnd(this);
						_abilityBlessingsCreated[i].OnTurnEnd(this, chunkData);
					}
				}
			}
		}

		public virtual void OnAfterResolve(ChunkData chunkData)
		{
			//GD.Print("SMTH");
		}

		public void SetFriendlyFire(bool friendlyFire)
		{
			this.friendlyFire = friendlyFire;
		}

		public Player GetPlayer()
		{
			return _player;
		}
		
		public virtual async void ResolveAbility(ChunkData chunk)
		{
			// _assignSound.PlaySound(selectedEffectIndex, selectedSongIndex);
			//GD.PushWarning("PlaySound");
			ClearGrid();
			UsedAbility usedAbility = new UsedAbility(this, chunk);
			if (_player is not null)
			{
				if (isAbilitySlow)
				{
					_player.SetMovementPoints(0);
				}
				_turnManager.AddUsedAbilityBeforeStartTurn(usedAbility, turnBeforeStartLifetime);
				_turnManager.AddUsedAbilityAfterResolve(usedAbility, turnAfterResolveLifetime);
				_turnManager.AddUsedAbilityOnTurnEnd(usedAbility, abilityCooldown);
			}
			else
			{
				_turnManager.AddUsedAbilityBeforeStartTurn(usedAbility, turnBeforeStartLifetime, true);
				_turnManager.AddUsedAbilityAfterResolve(usedAbility, turnAfterResolveLifetime, true);
				_turnManager.AddUsedAbilityOnTurnEnd(usedAbility, abilityCooldown, true);
			}
			ResetCooldown();
		}
		
		public virtual void OnExitAbility(ChunkData chunkDataPrev, ChunkData chunk)
		{
		}

		public virtual void ResolveBlessings(ChunkData chunk)
		{
			if (_abilityBlessingsCreated != null)
			{
				for (int i = 0; i < _abilityBlessingsCreated.Count; i++)
				{
					if (unlockedBlessingsList[i].blessingUnlocked)
					{
						_abilityBlessingsCreated[i].ResolveBlessing(this);
						_abilityBlessingsCreated[i].ResolveBlessing(this, chunk);
					}
				}
			}
		}

		public void UpdateAbilityButton()
		{
			if (_selectActionButton != null)
			{
				_selectActionButton.DisableAbility();
			}
		}

		public void UpdateAbilityButtonByActionPoints(int abilityPoints)
		{
			if (_selectActionButton != null)
			{
				_selectActionButton.UpdateAbilityCooldownWithPoints(abilityPoints);
			}
		}

		public void SetSelectActionButton(SelectActionButton selectActionButton)
		{
			_selectActionButton = selectActionButton;
		}

		public SelectActionButton GetActionButton()
		{
			return _selectActionButton;
		}

		public void ResetCooldown()
		{
			cooldownCount = 0;
		}
		
		public int GetCoolDown()
		{
			return abilityCooldown;
		}

		public virtual bool AbilityCanBeActivated()
		{
			if (abilityCooldown <= cooldownCount)
			{
				return true;
			}

			return false;
		}
		
		public virtual bool CheckIfAbilityIsActive()
		{
			if (abilityCooldown <= cooldownCount)
			{
				return true;
			}
		
			return false;
		}

		public int GetCoolDownCount()
		{
			return cooldownCount;
		}

		public virtual void DeselectAbility()
		{
			ClearGrid();
		}


		protected virtual void FinishAbility()
		{
			GameTileMap.Tilemap.DeselectCurrentCharacter();
		}
		
		private void DodgeActivation(ref int damage)
		{
			int dodgeNumber = _random.Next(0, 100);
			if (dodgeNumber > _objectData.accuracy)
			{
				damage = -1;
			}
		}
		
		protected void DealRandomDamageToTarget(ChunkData chunkData, int minDamage, int maxDamage)
		{
			if (chunkData != null && CanUseAttack() && (
				    chunkData.ObjectIsOnTile() && canUseOnObject || 
				    !IsAllegianceSame(chunkData) && canUseOnEnemy ||
				    IsAllegianceSame(chunkData) && canUseOnTeammate))
			{
				int randomDamage =  _random.Next(minDamage, maxDamage);
				DodgeActivation(ref randomDamage);
				DealDamage(chunkData, randomDamage);
			}
		}

		protected virtual bool CanUseAttack()
		{
			if (_player is not null && _player.debuffManager is not null && _player.debuffManager.CanUseAttack())
			{
				return true;
			}
			else if(_player is not null && _player.debuffManager is null)
			{
				return true;
			}
			else if (_player is null)
			{
				return true;
			}
			return false;
		}

		protected void DealDamage(ChunkData chunkData, int damage)
		{
			if (chunkData != null && CanUseAttack())
			{
				ModifyBonusDamage(chunkData);
				if (chunkData.CharacterIsOnTile() && !IsAllegianceSame(chunkData) && canUseOnEnemy ||
				    IsAllegianceSame(chunkData) && canUseOnTeammate)
				{
					Player attackedPlayer = chunkData.GetCurrentPlayer();
					attackedPlayer.DealDamage(damage+bonusDamage, _player);
					ChunkData enemyChunkData =  GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
					if (!attackedPlayer.CheckIfVisionTileIsUnlocked(enemyChunkData))
					{
						attackedPlayer.AddVisionTile(enemyChunkData);
					}
				}
				else if (chunkData.ObjectIsOnTile() && canUseOnObject)
				{
					Object attackedObject = chunkData.GetCurrentObject();
					attackedObject.DealDamage(damage+bonusDamage,_player);
				}
			}
		}
		
		public virtual string GetDamageString()
		{
			return $"{minAttackDamage}-{maxAttackDamage}";
		}

		public bool TurnIsEven()
		{
			return turinIsEven;
		}

		public virtual void Die()
		{
			
		}

		//Create object pool. Pool should be global. Each ability should have currentanimationobject (or maybe list of current animation objects) from pool.
		public async Task PlayAnimation(string animationName, ChunkData chunk)
		{
			PackedScene spawnCharacter = (PackedScene)animatedObjectPrefab; 
			Object animatedObject = spawnCharacter.Instantiate<Object>(); 
			_player.GetTree().Root.CallDeferred("add_child", animatedObject); 
			animatedObject.SetupObject(animatedObjectPrefabData);
			animatedObject.GlobalPosition = chunk.GetPosition();
			AnimationPlayer animationPlayer = animatedObject.objectInformation.GetObjectInformation().animationPlayer;
			if(animationPlayer != null && animationPlayer.HasAnimation(animationName))
			{
				animationPlayer.Play(animationName);
				await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation(animationName).Length-0.1f));
			}
			animatedObject.QueueFree();
		}
		
		public async Task PlayerAbilityAnimation()
		{
			AnimationPlayer animationPlayer = _player.objectInformation.GetObjectInformation().animationPlayer;
			animationPlayer.Play(AttackAnimation);
			await Task.Delay(TimeSpan.FromSeconds(animationPlayer.GetAnimation(AttackAnimation).Length-0.1f));
			animationPlayer.Play(_player.CurrentIdleAnimation);
		}
	}