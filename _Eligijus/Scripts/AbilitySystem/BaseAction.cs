using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

public abstract partial class BaseAction: Resource
	{
		protected Player player;
		[Export] protected bool laserGrid = false;
		[Export] protected int turnBeforeStartLifetime = 1;
		[Export] protected int turnAfterResolveLifetime = 1;
		[Export] protected int abilityPoints = 1;
		[Export]
		public int attackRange = 1;
		[Export]
		public int minAttackDamage = 0;
		[Export]
		public int maxAttackDamage = 0;
		[Export]
		public bool isAbilitySlow = true;
		[Export]
		public bool friendlyFire = false;
		[Export] 
		protected int abilityCooldown = 1;
		[Export]
		protected Color abilityHighlight = new Color("#B25E55");
		[Export]
		protected Color abilityHighlightHover = new Color("#9E4A41");
		[Export]
		protected Color abilityHoverCharacter = new Color("#FFE300");
		[Export]
		protected Color characterOnGrid = new Color("#FF5947");
		// Other Team
		[Export]
		protected Color otherTeamAbilityHighlight = new Color("#5d5d5d");
		[Export]
		protected Color otherTeamHighlightHover = new Color("#3a3a3a");
		[Export]
		protected Color otherTeamHoverCharacter = new Color("#CB36D6");
		[Export]
		protected Color otherTeamCharacterOnGrid = new Color("#141414");
		//
		[Export]
		protected Array<AbilityBlessing> _abilityBlessingsRef;
		protected Array<AbilityBlessing> _abilityBlessingsCreated;
		protected Array<UnlockedBlessingsResource> unlockedBlessingsList;
		protected List<ChunkData> _chunkList;
		protected bool turinIsEven = false;
		protected string customText = null;
		protected int cooldownCount = 0;
		protected SelectActionButton _selectActionButton;
		protected TurnManager _turnManager;
		private bool firstTimeUsage = false;
		private ObjectData _objectData;
		private Random _random;

		public BaseAction()
		{
			
		}

		public BaseAction(BaseAction action)
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
			this.player = player;
			if (player.unlockedBlessingList.Count > abilityIndex)
			{
				unlockedBlessingsList = player.unlockedBlessingList[abilityIndex].UnlockedBlessingsList;
			}
			else
			{
				unlockedBlessingsList = new Array<UnlockedBlessingsResource>();
			}
			SetObjectInformationData(player.playerInformation.objectData.GetPlayerInformationData());
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

		protected virtual void HighlightGridTile(ChunkData chunkData)
		{
			SetNonHoveredAttackColor(chunkData);
			chunkData.GetTileHighlight().EnableTile(true);
			chunkData.GetTileHighlight().ActivateColorGridTile(true);
		}

		protected void HighlightAllGridTiles()
		{
			foreach (var chunk in _chunkList)
			{
				HighlightGridTile(chunk);
			}
		}

		public bool IsPositionInGrid(Vector2 position)
		{
			return _chunkList.Contains(GameTileMap.Tilemap.GetChunk(position));
		}

		public virtual void CreateGrid()
		{
			CreateAvailableChunkList(attackRange);
			HighlightAllGridTiles();
		}

		public virtual void ClearGrid()
		{
			if (_chunkList != null)
			{
				foreach (var chunk in _chunkList)
				{
					HighlightTile highlightTile = chunk.GetTileHighlight();
					highlightTile.ActivateColorGridTile(false);
					DisableDamagePreview(chunk);
				}

				_chunkList.Clear();
			}
		}

		public virtual void CreateAvailableChunkList(int range)
		{
			ChunkData startChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
			if(laserGrid)
			{
				GeneratePlusPattern(startChunk, range);
			}
			else
			{
				GD.Print(startChunk.GetPosition());
				GenerateDiamondPattern(startChunk, range);
			}
		}

		public List<ChunkData> GetChunkList()
		{
			return _chunkList;
		}
		
		public Array<AbilityBlessing> GetAllBlessings()
		{
			return _abilityBlessingsRef;
		}

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

		protected virtual void GeneratePlusPattern(ChunkData centerChunk, int length)
		{
			(int centerX, int centerY) = centerChunk.GetIndexes();
			ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();

			for (int i = 1; i <= length; i++)
			{
				List<(int, int)> positions = new List<(int, int)> 
				{
					(centerX, centerY + i),  // Up
					(centerX, centerY - i),  // Down
					(centerX + i, centerY),  // Right
					(centerX - i, centerY)   // Left
				};

				foreach (var (x, y) in positions)
				{
					if (x >= 0 && x < chunksArray.GetLength(0) && y >= 0 && y < chunksArray.GetLength(1))
					{
						ChunkData chunk = chunksArray[x, y];
						TryAddTile(chunk);
					}
				}
			}
		}

		protected virtual void TryAddTile(ChunkData chunk)
		{
			if (chunk != null && !chunk.TileIsLocked() && chunk.GetCurrentPlayer() != GameTileMap.Tilemap.GetCurrentCharacter())
			{
				_chunkList.Add(chunk);
			}
		}

		protected virtual void SetNonHoveredAttackColor(ChunkData chunkData)
		{
			if (_turnManager.GetCurrentTeamIndex() == player.GetPlayerTeam())
			{
				SetCurrentTeamNonHoverAttackColor(chunkData);
			}
			else
			{
				SetOtherTeamNonHoverAttackColor(chunkData);
			}
		}

		protected virtual void SetCurrentTeamNonHoverAttackColor(ChunkData chunkData)
		{
			if (chunkData.GetCurrentPlayer() == null || (chunkData.GetCurrentPlayer() != null && !CanTileBeClicked(chunkData)))
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(abilityHighlight);
			}
			else
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(characterOnGrid);
			}
		}
		
		protected virtual void SetOtherTeamNonHoverAttackColor(ChunkData chunkData)
		{
			if (chunkData.GetCurrentPlayer() == null || (chunkData.GetCurrentPlayer() != null && !CanTileBeClicked(chunkData)))
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(otherTeamAbilityHighlight);
			}
			else
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(otherTeamCharacterOnGrid);
			}
		}

		public virtual void SetHoveredAttackColor(ChunkData chunkData)
		{
			if (_turnManager.GetCurrentTeamIndex() == player.GetPlayerTeam())
			{
				SetCurrentTeamHoveredAttackColor(chunkData);
			}
			else
			{
				SetOtherTeamHoveredAttackColor(chunkData);
			}
		}

		protected virtual void SetCurrentTeamHoveredAttackColor(ChunkData chunkData)
		{
			if (!chunkData.CharacterIsOnTile() || (chunkData.CharacterIsOnTile() && !CanTileBeClicked(chunkData)))
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(abilityHighlightHover);
			}
			else
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(abilityHoverCharacter);
			}
		}
		
		protected virtual void SetOtherTeamHoveredAttackColor(ChunkData chunkData)
		{
			if (!chunkData.CharacterIsOnTile() || (chunkData.CharacterIsOnTile() && !CanTileBeClicked(chunkData)))
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(otherTeamHighlightHover);
			}
			else
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(otherTeamHoverCharacter);
			}
		}
		public Color GetAbilityHighlightColor()
		{
			return abilityHighlight;
		}
		
		public int GetAbilityCooldown()
		{
			return abilityCooldown;
		}

		public virtual void OnMoveArrows(ChunkData hoveredChunk, ChunkData previousChunk)
		{
			
		}

		public virtual void OnMoveHover(ChunkData hoveredChunk, ChunkData previousChunk)
		{
			HighlightTile previousChunkHighlight = previousChunk?.GetTileHighlight();
			HighlightTile hoveredChunkHighlight = hoveredChunk?.GetTileHighlight();

			if (previousChunkHighlight != null && (hoveredChunk == null || !hoveredChunkHighlight.isHighlighted))
			{
				SetNonHoveredAttackColor(previousChunk);
				DisableDamagePreview(previousChunk);
			}
			if (hoveredChunkHighlight == null || hoveredChunk == previousChunk)
			{
				return;
			}
			if (hoveredChunkHighlight.isHighlighted)
			{
				SetHoveredAttackColor(hoveredChunk);
				if (CanTileBeClicked(hoveredChunk))
				{
					EnableDamagePreview(hoveredChunk);
				}
			}
			if (previousChunkHighlight != null)
			{
				SetNonHoveredAttackColor(previousChunk);
				DisableDamagePreview(previousChunk);
			}
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
		
		public virtual void EnableDamagePreview(ChunkData chunk, string text = null)
		{
			HighlightTile highlightTile = chunk.GetTileHighlight();
			if (customText != null)
			{
				highlightTile.SetDamageText(customText);
			}
			else
			{
				if (maxAttackDamage == minAttackDamage)
				{
					highlightTile.SetDamageText(maxAttackDamage.ToString());
				}
				else
				{
					highlightTile.SetDamageText($"{minAttackDamage}-{maxAttackDamage}");
				}

				if (chunk.GetCurrentPlayer()!=null && chunk.GetCurrentPlayer().playerInformation.GetHealth() <= minAttackDamage)
				{
					highlightTile.ActivateDeathSkull(true);
				}
			}
		}
		
		protected virtual void EnableDamagePreview(List<ChunkData> chunks)
		{
			foreach(ChunkData chunk in chunks)
			{
				EnableDamagePreview(chunk);
			}
		}

		protected virtual void DisableDamagePreview(ChunkData chunk)
		{
			HighlightTile highlightTile = chunk.GetTileHighlight();
			highlightTile.ActivateDeathSkull(false);
			highlightTile.DisableDamageText();
		}

		public virtual bool CanTileBeClicked(ChunkData chunkData)
		{
			return chunkData.GetTileHighlight().isHighlighted && (CheckIfSpecificInformationType(chunkData, typeof(Player)) || 
			        CheckIfSpecificInformationType(chunkData, typeof(Object))) && (!IsAllegianceSame(chunkData) || friendlyFire);
		}

		protected virtual bool CheckAbilityBounds(ChunkData chunkData)
		{
			return false;
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
			return player;
		}
		
		public virtual void ResolveAbility(ChunkData chunk)
		{
			// _assignSound.PlaySound(selectedEffectIndex, selectedSongIndex);
			if (isAbilitySlow)
			{
				player.SetMovementPoints(0);
			}
			GD.PushWarning("PlaySound");
			ClearGrid();
			UsedAbility usedAbility = new UsedAbility(this, chunk);
			if (player is not null)
			{
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
		
		protected static bool CheckIfSpecificInformationType(ChunkData chunk, Type type)
		{
			return chunk.GetCharacterType() == type;
		}

		public bool IsAllegianceSame(ChunkData chunk)
		{
			return chunk.GetCurrentPlayer() != null && player != null && chunk.GetCurrentPlayer().GetPlayerTeam() == player.GetPlayerTeam();
		}
		
		public bool IsAllegianceSame(ChunkData chunk, ChunkData seconChunck)
		{
			return chunk.GetCurrentPlayer() != null && seconChunck.GetCurrentPlayer() != null && chunk.GetCurrentPlayer().GetPlayerTeam() == seconChunck.GetCurrentPlayer().GetPlayerTeam();
		}
		
		public bool IsAllegianceSameForBuffs(ChunkData chunk)
		{
			return chunk == null || (chunk.GetCurrentPlayer() != null && chunk.GetCurrentPlayer().GetPlayerTeam() == player.GetPlayerTeam() && !friendlyFire);
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
			if (chunkData != null && chunkData.CharacterIsOnTile() && (!IsAllegianceSame(chunkData) || friendlyFire))
			{
				
				int randomDamage = _random.Next(minDamage, maxDamage);
				DodgeActivation(ref randomDamage);
				chunkData.GetCurrentPlayer().playerInformation.DealDamage(randomDamage, player);
			}
		}

		protected void DealDamage(ChunkData chunkData, int damage)
		{
			if (chunkData != null && chunkData.GetCurrentPlayer() != null && (!IsAllegianceSame(chunkData) || friendlyFire))
			{
				chunkData.GetCurrentPlayer().playerInformation.DealDamage(damage, player);
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

