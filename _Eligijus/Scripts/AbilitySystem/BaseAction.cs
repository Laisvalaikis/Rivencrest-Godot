using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public abstract partial class BaseAction: TileAction
	{
		[Export] protected int turnBeforeStartLifetime = 1;
		[Export] protected int turnAfterResolveLifetime = 1;
		[Export] protected int abilityPoints = 1;
		[Export]
		public bool isAbilitySlow = true;
		[Export] 
		protected int abilityCooldown = 1;
		//
		[Export]
		protected Array<AbilityBlessing> _abilityBlessingsRef;
		protected Array<AbilityBlessing> _abilityBlessingsCreated;
		protected Array<UnlockedBlessingsResource> unlockedBlessingsList;
		protected bool turinIsEven = false;
		protected string customText = null;
		protected int cooldownCount = 0;
		protected SelectActionButton _selectActionButton;
		protected TurnManager _turnManager;
		private bool firstTimeUsage = false;
		protected ObjectData _objectData;
		protected Object _object;
		private Random _random;

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

		public void SetPlayer(Player player)
		{
			_player = player;
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

		public virtual void Start()
		{
			_random = new Random();
			_chunkList = new List<ChunkData>();
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

		public void AddTurnManager(TurnManager turnManager)
		{
			_turnManager = turnManager;
		}

		public int GetAbilityPoints()
		{
			return abilityPoints;
		}
		
		public Array<AbilityBlessing> GetAllBlessings()
		{
			return _abilityBlessingsRef;
		}

		// this need to go
		protected virtual void GenerateDiamondPattern(ChunkData centerChunk, int radius)
		{
			(int centerX, int centerY) = centerChunk.GetIndexes();
			ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray(); 
			for (int y = -radius; y <= radius; y++)
			{
				for (int x = -radius; x <= radius; x++)
				{
					if (Mathf.Abs(x) + Mathf.Abs(y) <= radius)
					{
						int targetX = centerX + x;
						int targetY = centerY + y;

						// Ensuring we don't go out of array bounds.
						if (targetX >= 0 && targetX < chunksArray.GetLength(0) && targetY >= 0 && targetY < chunksArray.GetLength(1))
						{
							ChunkData chunk = chunksArray[targetX, targetY];
							TryAddTile(chunk);
						}
					}
				}
			}
		}
		
		
		public int GetAbilityCooldown()
		{
			return abilityCooldown;
		}
		
		public int IncreaseAbilityCooldown()
		{
			return cooldownCount++;
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
			GD.Print("SMTH");
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
			GD.Print("SMTH");
		}

		public void SetFriendlyFire(bool friendlyFire)
		{
			this.friendlyFire = friendlyFire;
		}

		public Player GetPlayer()
		{
			return _player;
		}
		
		public virtual void ResolveAbility(ChunkData chunk)
		{
			// _assignSound.PlaySound(selectedEffectIndex, selectedSongIndex);
			GD.PushWarning("PlaySound");
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
		}
		
		public virtual void OnExitAbility(ChunkData chunkDataPrev, ChunkData chunk)
		{
			GD.Print("SMTH");	
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
				cooldownCount = 0;
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
			if (chunkData != null && CanUseAttack())
			{

				if (chunkData.CharacterIsOnTile() &&
				    (!IsAllegianceSame(chunkData) || friendlyFire))
				{
					int randomDamage = _random.Next(minDamage, maxDamage);
					DodgeActivation(ref randomDamage);
					DealDamage(chunkData, randomDamage);
				}

				if (chunkData.ObjectIsOnTile())
				{
					int randomDamage = _random.Next(minDamage, maxDamage);
					DodgeActivation(ref randomDamage);
					DealDamage(chunkData, randomDamage);
				}
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
				if (chunkData.CharacterIsOnTile() && (!IsAllegianceSame(chunkData) || friendlyFire))
				{
					Player attackedPlayer = chunkData.GetCurrentPlayer();
					attackedPlayer.objectInformation.GetObjectInformation().DealDamage(damage, _player);
					ChunkData enemyChunkData =  GameTileMap.Tilemap.GetChunk(_player.GlobalPosition);
					if (!attackedPlayer.CheckIfVisionTileIsUnlocked(enemyChunkData))
					{
						attackedPlayer.AddVisionTile(enemyChunkData);
					}
				}
				else if (chunkData.ObjectIsOnTile())
				{
					Object attackedObject = chunkData.GetCurrentObject();
					attackedObject.objectInformation.GetObjectInformation().DealDamage(damage,_player);
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
	}

