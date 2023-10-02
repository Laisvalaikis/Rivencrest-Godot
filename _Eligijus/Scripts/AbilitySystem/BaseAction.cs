using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
	public abstract partial class BaseAction: Resource
	{

		[Export] protected Node2D player;
		[Export] protected PlayerInformation playerInformation;
		[Export] protected bool laserGrid = false;
		[Export] public int turnsSinceCast = 0;
		[Export] public int turnLifetime = 1;
		//private RaycastHit2D raycast;
		[Export]
		public int AttackRange = 1;
		[Export]
		public int AbilityCooldown = 1;
		[Export]
		public int minAttackDamage = 0;
		[Export]
		public int maxAttackDamage = 0;
		[Export]
		public bool isAbilitySlow = true;
		[Export]
		public bool friendlyFire = false;
		
		protected Color AbilityHighlight = new Color("#B25E55");
		protected Color AbilityHighlightHover = new Color("#9E4A41");
		protected Color AbilityHoverCharacter = new Color("#FFE300");
		protected Color OtherOnGrid = new Color("#717171");
		protected Color CharacterOnGrid = new Color("#FF5947");
		protected List<ChunkData> _chunkList;
		private PlayerInformationData _playerInformationData;
		private Random _random;

		void Awake()
		{
			// playerInformation = GetComponent<PlayerInformation>();
			_random = new Random();
			_playerInformationData = new PlayerInformationData();
			_playerInformationData.CopyData(playerInformation.playerInformationData);
			_chunkList = new List<ChunkData>();
			// AbilityPoints = AbilityCooldown;
		}

		public virtual void Start()
		{
			
		}

		protected virtual void HighlightGridTile(ChunkData chunkData)
		{
			if (chunkData.GetCurrentCharacter() != GameTileMap.Tilemap.GetCurrentCharacter())
			{
				SetNonHoveredAttackColor(chunkData);
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
			CreateAvailableChunkList(AttackRange);
			HighlightAllGridTiles();
		}

		public virtual void CreateGrid()
		{
			CreateAvailableChunkList(AttackRange);
			HighlightAllGridTiles();
		}

		public virtual void ClearGrid()
		{
			foreach (var chunk in _chunkList)
			{
				HighlightTile highlightTile = chunk.GetTileHighlight();
				highlightTile.ActivateColorGridTile(false);
				DisableDamagePreview(chunk);
			}
			_chunkList.Clear();
		}

		private void CreateAvailableChunkList(int attackRange)
		{
			ChunkData startChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
			if(laserGrid)
			{
				GeneratePlusPattern(startChunk, attackRange);
			}
			else
			{
				GenerateDiamondPattern(startChunk, attackRange);
			}
		}

		public List<ChunkData> GetChunkList()
		{
			return _chunkList;
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
			if (chunkData.GetCurrentCharacter() == null || (chunkData.GetCurrentCharacter() != null && !CanTileBeClicked(chunkData)))
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(AbilityHighlight);
			}
			else
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(CharacterOnGrid);
			}
		}

		protected virtual void SetHoveredAttackColor(ChunkData chunkData)
		{
			if (!chunkData.CharacterIsOnTile() || (chunkData.CharacterIsOnTile() && !CanTileBeClicked(chunkData)))
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(AbilityHighlightHover);
			}
			else
			{
				chunkData.GetTileHighlight()?.SetHighlightColor(AbilityHoverCharacter);
				EnableDamagePreview(chunkData);
			}
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
					GameTileMap.Tilemap.MoveSelectedCharacter(tempTile.GetPosition(), new Vector2(0, 0.5f), player.GetCurrentCharacter());
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

				if (chunk.GetCurrentPlayerInformation().GetHealth() <= minAttackDamage)
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

		protected virtual bool CanTileBeClicked(ChunkData chunkData)
		{
			return CheckIfSpecificInformationType(chunkData, InformationType.Player) && !IsAllegianceSame(chunkData);
		}
		
		public virtual void OnTurnStart()
		{
			
		}

		public virtual void OnTurnEnd()
		{
			RefillActionPoints();
			turnsSinceCast++;
		}
		
		public virtual void RemoveActionPoints()//panaudojus action
		{
			// AvailableAttacks--;
		}

		public virtual void RefillActionPoints() //pradzioj ejimo
		{
			
		}

		public virtual void ResolveAbility(ChunkData chunk)
		{
			// _assignSound.PlaySound(selectedEffectIndex, selectedSongIndex);
			GD.PushWarning("PlaySound");
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

		protected bool IsAllegianceSame(ChunkData chunk)
		{
			return chunk == null || chunk.GetCurrentPlayerInformation().GetPlayerTeam() == playerInformation.GetPlayerTeam() || !friendlyFire;
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
			if (chunkData != null && chunkData.CharacterIsOnTile() && IsAllegianceSame(chunkData))
			{
				
				int randomDamage = _random.Next(minDamage, maxDamage);
				bool crit = IsItCriticalStrike(ref randomDamage);
				DodgeActivation(ref randomDamage, chunkData.GetCurrentPlayerInformation());
				chunkData.GetCurrentPlayerInformation().DealDamage(randomDamage, crit, player);
			}
		}

		protected void DealDamage(ChunkData chunkData, int damage, bool crit)
		{
			if (chunkData != null && chunkData.GetCurrentCharacter() != null && IsAllegianceSame(chunkData))
			{
				chunkData.GetCurrentPlayerInformation().DealDamage(damage, crit, player);
			}
		}

		protected bool DoesCharacterHaveBlessing(string blessingName)
		{
			Blessing blessing = null;
			for (int i = 0; i < _playerInformationData.BlessingsAndCurses.Count; i++)
			{
				if (_playerInformationData.BlessingsAndCurses[i].blessingName == blessingName)
				{
					blessing = _playerInformationData.BlessingsAndCurses[i];
					break;
				}
			}
			return blessing != null;
		}
		
		public virtual void BuffAbility()
		{
			
		}
		
		public virtual BaseAction GetBuffedAbility(List<Blessing> blessings)
		{
			return this;
		}
		
		public virtual string GetDamageString()
		{
			return $"{minAttackDamage}-{maxAttackDamage}";
		}
		
		// protected IEnumerator ExecuteAfterTime(float time, Action task)
		// {
		//     yield return new WaitForSeconds(time);
		//     task();
		// }
		
	}

