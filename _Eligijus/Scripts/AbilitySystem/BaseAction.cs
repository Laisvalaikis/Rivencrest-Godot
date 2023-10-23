using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Array = System.Array;

public abstract partial class BaseAction: Resource
	{
		protected Player player;
		[Export] protected bool laserGrid = false;
		[Export] public int turnsSinceCast = 0;
		[Export] public int turnLifetime = 1;
		//private RaycastHit2D raycast;
		[Export] protected int abilityPointsUsage = 1;
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
		protected int AbilityCooldown = 1;
		[Export]
		protected Color abilityHighlight = new Color("#B25E55");
		[Export]
		protected Color abilityHighlightHover = new Color("#9E4A41");
		[Export]
		protected Color abilityHoverCharacter = new Color("#FFE300");
		[Export]
		protected Color otherOnGrid = new Color("#717171");
		[Export]
		protected Color characterOnGrid = new Color("#FF5947");
		[Export]
		protected Array<AbilityBlessing> _abilityBlessingsRef;
		protected Array<AbilityBlessing> _abilityBlessingsCreated;
		protected List<ChunkData> _chunkList;
		protected bool turinIsEven = false;
		
		private PlayerInformationData _playerInformationData;
		private Random _random;

		public BaseAction()
		{
			
		}

		public BaseAction(BaseAction action)
		{
			laserGrid = action.laserGrid;
			turnsSinceCast = action.turnsSinceCast;
			turnLifetime = action.turnLifetime;
			attackRange = action.attackRange;
			minAttackDamage = action.minAttackDamage;
			maxAttackDamage = action.maxAttackDamage;
			isAbilitySlow = action.isAbilitySlow;
			friendlyFire = action.friendlyFire;
			abilityHighlight = action.abilityHighlight;
			abilityHighlightHover = action.abilityHighlightHover;
			abilityHoverCharacter = action.abilityHoverCharacter;
			otherOnGrid = action.otherOnGrid;
			characterOnGrid = action.characterOnGrid;
		}

		public virtual BaseAction CreateNewInstance(BaseAction action)
		{
			throw new NotImplementedException();
		}

		public void SetupAbility(Player player)
		{
			this.player = player;
			Start();
		}

		public virtual void Start()
		{
			_random = new Random();
			_playerInformationData = new PlayerInformationData();
			_playerInformationData.CopyData(player.playerInformation.playerInformationData);
			_chunkList = new List<ChunkData>();
			if (_abilityBlessingsRef != null)
			{
				for (int i = 0; i < _abilityBlessingsRef.Count; i++)
				{
					if (_abilityBlessingsRef[i].IsBlessingUnlocked())
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
		}

		protected virtual void HighlightGridTile(ChunkData chunkData)
		{
			if (chunkData.GetCurrentPlayer() != GameTileMap.Tilemap.GetCurrentCharacter())
			{
				SetNonHoveredAttackColor(chunkData);
				chunkData.GetTileHighlight().EnableTile(true);
				chunkData.GetTileHighlight().ActivateColorGridTile(true);
			}
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
		
		public virtual void CreateGrid(ChunkData chunkData, int radius)
		{
			CreateAvailableChunkList(attackRange);
			HighlightAllGridTiles();
		}

		public virtual void CreateGrid()
		{
			CreateAvailableChunkList(attackRange);
			HighlightAllGridTiles();
		}

		public virtual void ClearGrid()
		{
			foreach (var chunk in _chunkList)
			{
				HighlightTile highlightTile = chunk.GetTileHighlight();
				highlightTile.EnableTile(false);
				highlightTile.ActivateColorGridTile(false);
				DisableDamagePreview(chunk);
			}
			_chunkList.Clear();
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

		public Array<AbilityBlessing> GetBlessings()
		{
			return _abilityBlessingsCreated;
		}
		
		public Array<AbilityBlessing> GetAllBlessings()
		{
			return _abilityBlessingsRef;
		}

		protected void GenerateDiamondPattern(ChunkData centerChunk, int radius)
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
							if (chunk != null && !chunk.TileIsLocked())
							{
								_chunkList.Add(chunk);
							}
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
						if (chunk != null && !chunk.TileIsLocked())
						{
							_chunkList.Add(chunk);
						}
					}
				}
			}
		}

		protected virtual void SetNonHoveredAttackColor(ChunkData chunkData)
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

		public virtual void SetHoveredAttackColor(ChunkData chunkData)
		{
			if (!chunkData.CharacterIsOnTile() || (chunkData.CharacterIsOnTile() && !CanTileBeClicked(chunkData)))
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(abilityHighlightHover);
			}
			else
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(abilityHoverCharacter);
				EnableDamagePreview(chunkData);
			}
		}

		public Color GetAbilityHighlightColor()
		{
			return abilityHighlight;
		}
		
		public Color GetAbilityHighlightHoverColor()
		{
			return abilityHighlightHover;
		}
		
		public Color GetAbilityHoverCharacterColor()
		{
			return abilityHoverCharacter;
		}
		
		public Color GetOtherOnGridColor()
		{
			return otherOnGrid;
		}
		
		public Color GetCharacterOnGridColor()
		{
			return characterOnGrid;
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
			if (playerChunkIndex.x > chunkIndex.x)
			{
				return Side.isFront;
			}
			else if (playerChunkIndex.x < chunkIndex.x)
			{
				return Side.isBack;
			}
			else if (playerChunkIndex.y < chunkIndex.y)
			{
				return Side.isRight;
			}
			else if (playerChunkIndex.y > chunkIndex.y)
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
					sideVector = (-1, 0);
					break;
				case Side.isBack:
					sideVector = (1, 0);
					break;
				case Side.isRight:
					sideVector = (0, 1);
					break;
				case Side.isLeft:
					sideVector = (0, -1);
					break;
				case Side.none:
					sideVector = (0, 0);
					break;
			}
			return sideVector;
		}

		protected void MovePlayerToSide(ChunkData player, (int, int) sideVector, ChunkData positionTile = null)
		{
			(int x, int y) indexes = player.GetIndexes();
			if (positionTile != null)
			{
				indexes = positionTile.GetIndexes();
			}
			(int, int) tempIndexes = new (indexes.x + sideVector.Item1, indexes.y + sideVector.Item2);
			if (GameTileMap.Tilemap.CheckBounds(tempIndexes.Item1, tempIndexes.Item2))
			{
				ChunkData tempTile = GameTileMap.Tilemap.GetChunkDataByIndex(tempIndexes.Item1, tempIndexes.Item2);
				if (!tempTile.CharacterIsOnTile())
				{
					GameTileMap.Tilemap.MoveSelectedCharacter(tempTile.GetPosition(), new Vector2(0, 0.5f), player.GetCurrentPlayer());
				}
			}
		}

		protected virtual void EnableDamagePreview(ChunkData chunk, string customText = null)
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

				if (chunk.GetCurrentPlayer().playerInformation.GetHealth() <= minAttackDamage)
				{
					highlightTile.ActivateDeathSkull(true);
				}
			}
		}
		
		protected virtual void EnableDamagePreview(List<ChunkData> chunks, string customText=null)
		{
			foreach(ChunkData chunk in chunks)
			{
				EnableDamagePreview(chunk, customText);
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
			return CheckIfSpecificInformationType(chunkData, InformationType.Player) && !IsAllegianceSame(chunkData);
		}
		
		public virtual void OnTurnStart()
		{
		}

		public virtual void OnTurnEnd()
		{
			//RefillActionPoints();
			turnsSinceCast++;
			if (!turinIsEven)
			{
				turinIsEven = true;
			}
			else
			{
				turinIsEven = false;
			}
		}

		public void SetFriendlyFire(bool friendlyFire)
		{
			this.friendlyFire = friendlyFire;
		}

		public Player GetPlayer()
		{
			return player;
		}

		public virtual void RemoveActionPoints()//panaudojus action
		{
			// AvailableAttacks--;
		}

		public virtual void ResolveAbility(ChunkData chunk)
		{
			// _assignSound.PlaySound(selectedEffectIndex, selectedSongIndex);
			GD.PushWarning("PlaySound");
			ClearGrid();
		}

		public virtual void DeselectAbility()
		{
			ClearGrid();
		}


		protected virtual void FinishAbility()
		{
			GameTileMap.Tilemap.DeselectCurrentCharacter();
		}

		protected ChunkData GetSpecificGroundTile(Vector2 position)
		{
			return GameTileMap.Tilemap.GetChunk(position);
		}

		protected static bool CheckIfSpecificInformationType(ChunkData chunk, InformationType informationType)
		{
			return chunk.GetInformationType() == informationType;
		}

		public bool IsAllegianceSame(ChunkData chunk)
		{
			return chunk == null || (chunk.GetCurrentPlayer().playerInformation.GetPlayerTeam() == player.playerInformation.GetPlayerTeam() && !friendlyFire);
		}

		protected bool IsItCriticalStrike(ref int damage)
		{
			int critNumber = _random.Next(0, 100);
			bool crit;
			if (critNumber > _playerInformationData.critChance)
			{
				crit = false;
			}
			else
			{
				damage += 3;
				crit = true;
			}
			return crit;
		}

		private void DodgeActivation(ref int damage, PlayerInformation target) //Dodge temporarily removed
		{
			int dodgeNumber = _random.Next(0, 100);
			if (dodgeNumber > _playerInformationData.accuracy - target.playerInformationData.dodgeChance)
			{
				damage = -1;
			}
		}

		protected void DealRandomDamageToTarget(ChunkData chunkData, int minDamage, int maxDamage)
		{
			if (chunkData != null && chunkData.CharacterIsOnTile() && !IsAllegianceSame(chunkData))
			{
				
				int randomDamage = _random.Next(minDamage, maxDamage);
				bool crit = IsItCriticalStrike(ref randomDamage);
				DodgeActivation(ref randomDamage, chunkData.GetCurrentPlayer().playerInformation);
				chunkData.GetCurrentPlayer().playerInformation.DealDamage(randomDamage, crit, player);
			}
		}

		protected void DealDamage(ChunkData chunkData, int damage, bool crit)
		{
			if (chunkData != null && chunkData.GetCurrentPlayer() != null && IsAllegianceSame(chunkData))
			{
				chunkData.GetCurrentPlayer().playerInformation.DealDamage(damage, crit, player);
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

		// protected IEnumerator ExecuteAfterTime(float time, Action task)
		// {
		//     yield return new WaitForSeconds(time);
		//     task();
		// }
		
	}

