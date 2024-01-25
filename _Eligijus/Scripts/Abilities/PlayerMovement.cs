using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerMovement : BaseAction
{
	private bool isFacingRight = true;
	private GameTileMap _gameTileMap;
	private List<ChunkData> _path;
	private int movementRange = 0;
	private PathCache cache=new PathCache();

	public PlayerMovement()
	{
		
	}
	
	public PlayerMovement(PlayerMovement playerMovement): base(playerMovement)
	{
		isFacingRight = playerMovement.isFacingRight;
		_gameTileMap = playerMovement._gameTileMap;
		_path = new List<ChunkData>();
	}

	public override BaseAction CreateNewInstance(BaseAction action)
	{
		PlayerMovement movement = new PlayerMovement((PlayerMovement)action);
		return movement;
	}
	
	protected override void Start()
	{
		base.Start();
		_gameTileMap = GameTileMap.Tilemap;
	}
	
	public override void OnMoveArrows(ChunkData hoveredChunk, ChunkData previousChunk)
	{
		if (hoveredChunk==null || !hoveredChunk.GetTileHighlight().isHighlighted)
		{
			ClearArrowPath();
			return;
		}        
		HighlightTile hoveredChunkHighlight = hoveredChunk.GetTileHighlight();
		if (hoveredChunkHighlight == null || previousChunk!=null && hoveredChunkHighlight == previousChunk.GetTileHighlight()) return;
		
		if (hoveredChunkHighlight.isHighlighted)
		{
			ClearArrowPath();
			_path = AStarSearch(GameTileMap.Tilemap.GetChunk(_player.Position), hoveredChunk);
			if (_path != null)
			{
				SetTileArrow(_path,0,_path.Count-1);
			}
		}
	}
	
	private void ClearArrowPath()
	{
		if (_path != null)
		{
			foreach (ChunkData chunk in _path)
			{
				chunk.GetTileHighlight().DeactivateArrowTile();
			}
		}
	}

	public override void ClearGrid()
	{
		base.ClearGrid();
		ClearArrowPath();
	}

	protected override void HighlightGridTile(ChunkData chunkData)
	{
		if (chunkData.GetCurrentPlayer() == null)
		{
			chunkData.GetTileHighlight().EnableTile(true);
			chunkData.GetTileHighlight().ActivateColorGridTile(true);
			if (_turnManager.GetCurrentTeamIndex() == _player.GetPlayerTeam())
			{
				chunkData.GetTileHighlight().SetHighlightColor(abilityHighlight);
			}
			else
			{
				chunkData.GetTileHighlight().SetHighlightColor(otherTeamAbilityHighlight);	
			}
		}
	}

	public override void ResolveAbility(ChunkData chunk)
	{
		if (CheckAbilityBounds(chunk))
		{
			ClearArrowPath();
			_path = null;
			if (!GameTileMap.Tilemap.CharacterIsOnTile(chunk))
			{
				GameTileMap.Tilemap.MoveSelectedCharacter(chunk);
				_player.AddMovementPoints(movementRange * (-1));
			}

			base.ResolveAbility(chunk);
			FinishAbility();
			CreateGrid();
			UpdateAbilityButton();
		}
	}
	
	protected override bool CanAddTile(ChunkData chunk)
	{
		if (chunk != null && !chunk.TileIsLocked() && !chunk.IsFogOnTile() && chunk.GetCurrentPlayer() != GameTileMap.Tilemap.GetCurrentCharacter() && (!chunk.ObjectIsOnTile() || chunk.ObjectIsOnTile() && chunk.GetCurrentObject().CanStepOn()))
		{
			return true;
		}

		return false;
	}

	protected override bool CheckAbilityBounds(ChunkData chunkData)
	{
		if (_path != null && _path.Contains(chunkData))
		{
			return true;
		}
		return false;
	}

	public override bool AbilityCanBeActivated()
	{
		if (abilityCooldown <= cooldownCount || _player.GetMovementPoints() != 0 && abilityCooldown >= cooldownCount)
		{
			cooldownCount = 0;
			return true;
		}

		return false;
	}
	
	public override bool CheckIfAbilityIsActive()
	{
		if (_player.GetMovementPoints() != 0 && abilityCooldown >= cooldownCount)
		{
			return true;
		}
		
		return false;
	}
	
	protected override void FinishAbility()
	{
	
	}

	public override void CreateGrid()
	{
		CreateAvailableChunkList(_player.GetMovementPoints());
		HighlightAllGridTiles();
	}
	
	
	protected override void GenerateDiamondPattern(ChunkData centerChunk, int radius)
	{
		Queue<(ChunkData chunk, int distance)> queue = new Queue<(ChunkData, int)>();
		HashSet<ChunkData> visited = new HashSet<ChunkData>();

		queue.Enqueue((centerChunk, 0));
		visited.Add(centerChunk);

		while (queue.Count > 0)
		{
			(ChunkData currentChunk, int currentDistance) = queue.Dequeue();
			if (currentDistance > radius)
			{
				continue;
			}
			
			TryAddTile(currentChunk);

			foreach (ChunkData adjacentChunk in GetAdjacentChunks(currentChunk))
			{
				if (!visited.Contains(adjacentChunk) && adjacentChunk.GetCharacterType() != typeof(Object) && (IsAllegianceSame(adjacentChunk)||adjacentChunk.GetCurrentPlayer()==null))
				{
					queue.Enqueue((adjacentChunk, currentDistance + 1));
					visited.Add(adjacentChunk);
				}
			}
		}
	}

	private IEnumerable<ChunkData> GetAdjacentChunks(ChunkData chunk)
	{
		ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
		(int chunkX, int chunkY) = chunk.GetIndexes();

		// Check and return the chunk above
		if (GameTileMap.Tilemap.CheckBounds(chunkX, chunkY - 1))
		{
			yield return chunksArray[chunkX, chunkY - 1];
		}

		// Check and return the chunk below
		if (GameTileMap.Tilemap.CheckBounds(chunkX, chunkY + 1))
		{
			yield return chunksArray[chunkX, chunkY + 1];
		}

		// Check and return the chunk to the left
		if (GameTileMap.Tilemap.CheckBounds(chunkX - 1, chunkY))
		{
			yield return chunksArray[chunkX - 1, chunkY];
		}

		// Check and return the chunk to the right
		if (GameTileMap.Tilemap.CheckBounds(chunkX + 1, chunkY))
		{
			yield return chunksArray[chunkX + 1, chunkY];
		}
	}


	private List<ChunkData> GetDiagonalPath(ChunkData start, ChunkData end, ChunkData[,] chunkArray)
	{
		List<ChunkData> stairStepPath = new List<ChunkData>();
		// Get the starting and ending indexes
		int startX = start.GetIndexes().Item1;
		int startY = start.GetIndexes().Item2;
		int endX = end.GetIndexes().Item1;
		int endY = end.GetIndexes().Item2;
	
		int x = startX, y = startY;
	
		// Determine the direction of the moves
		int xStep = (endX > startX) ? 1 : -1;
		int yStep = (endY > startY) ? 1 : -1;
	
		while (x != endX || y != endY)
		{
			if (x != endX)
			{
				stairStepPath.Add(chunkArray[x, y]);
				if (!chunkArray[x + xStep,y].GetTileHighlight().isHighlighted && chunkArray[x + xStep,y].GetCurrentPlayer()==null)
				{
					y += yStep;
				}
				else
				{
					x += xStep;
				}
			}
			if (y != endY)
			{
				stairStepPath.Add(chunkArray[x, y]);
				if (!chunkArray[x, y + yStep].GetTileHighlight().isHighlighted && chunkArray[x, y + yStep].GetCurrentPlayer()==null)
				{
					x += xStep;
				}
				else
				{
					y += yStep;
				}
			}
		}
		// Add the end point
		stairStepPath.Add(chunkArray[endX, endY]);
		return stairStepPath;
	}
	
	
	private void UpdatePath(ChunkData newEnd, List<ChunkData> existingPath, ChunkData[,] chunkArray)
	{
		int expectedLength = GetExpectedPathLength(existingPath[0], newEnd);
	
		if (existingPath.Count > expectedLength)
		{
			ChunkData startingPoint = existingPath[0];
			existingPath.Clear();
			existingPath.AddRange(GetDiagonalPath(startingPoint, newEnd, chunkArray));
			// Reset all arrows as the path has been cleared
			SetTileArrow(existingPath, 0, existingPath.Count - 1);
		}
		else
		{
			// Update the arrow for the old end point first
			if (existingPath.Count > 1)
			{
				SetTileArrow(existingPath, existingPath.Count - 2, existingPath.Count - 1);
			}
	
			// Add the new end point and set its arrow
			existingPath.Add(newEnd);
			SetTileArrow(existingPath, existingPath.Count - 1, existingPath.Count - 1);
		}
	}
	
	private int GetExpectedPathLength(ChunkData start, ChunkData end)
	{
		var (startX, startY) = start.GetIndexes();
		var (endX, endY) = end.GetIndexes();
	
		return Math.Abs(endX - startX) + Math.Abs(endY - startY);
	}
	
	private bool IsAdjacent(ChunkData a, ChunkData b)
	{
		var (ax, ay) = a.GetIndexes();
		var (bx, by) = b.GetIndexes();

		return Math.Abs(ax - bx) + Math.Abs(ay - by) == 1;
	}
	
	private void SetTileArrow(List<ChunkData> path, int start, int end)
	{
		movementRange = 0;
		for (int i = start; i <= end; i++)
		{
			ChunkData current = path[i];
			ChunkData prev = i > 0 ? path[i - 1] : null;
			ChunkData next = i < path.Count - 1 ? path[i + 1] : null;
			movementRange++;
			int arrowType = DetermineArrowType(current, prev, next);
			path[i].GetTileHighlight().SetArrowSprite(arrowType);
		}
		movementRange--; // nes jis pradeda nuo character
	}
	
	private int DetermineArrowType(ChunkData current, ChunkData prev, ChunkData next)
	{
		if (prev == null && next == null) return 0;  // Invalid case

		var (cx, cy) = current.GetIndexes();
		var (px, py) = prev?.GetIndexes() ?? (0, 0);
		var (nx, ny) = next?.GetIndexes() ?? (0, 0);
		
		if (prev == null)  // Start
		{
			if (cx < nx) return 1;  // Right Start
			if (cx > nx) return 2;  // Left Start
			if (cy < ny) return 3;  // Down Start
			if (cy > ny) return 4;  // Up Start
		}
		else if (next == null)  // End
		{
			if (cx > px) return 5;  // Right End
			if (cx < px) return 6;  // Left End
			if (cy > py) return 7;  // Down End
			if (cy < py) return 8;  // Up End
		}
		else  // Intermediate or Corner
		{
			if (cx == px && cx == nx) return 9;  // Vertical Intermediate
			if (cy == py && cy == ny) return 10; // Horizontal Intermediate
			
			//corner math
			if ((cx > px && cy == py && cx == nx && cy > ny) || (cx == px && cy > py && cx > nx && cy == ny))
				return 11;
			
			if ((px == cx && py < cy && cx < nx && cy == ny) || (px > cx && cy == py && cx == nx && cy > ny))
				return 12;
			
			if ((cx == px && cy < py && cx < nx && cy == ny) || (cx < px && cy == py && cx == nx && cy < ny))
				return 14;

			if ((cx == px && cy < py && cx > nx && cy == ny) || (cx > px && cy == py && cx == nx && cy < ny))
				return 13;
		}
		return 0;  
	}
	
	public List<ChunkData> AStarSearch(ChunkData start, ChunkData goal)
{
    // Check if route has already once been found and return it from cache
    if (cache.IsCached(start, goal)) return cache.Get(start, goal);

    // Setup to begin A* Search
    Dictionary<ChunkData, ChunkData> parentMap = new Dictionary<ChunkData, ChunkData>();
    HashSet<ChunkData> visited = new HashSet<ChunkData>();
    Dictionary<ChunkData, int> distances = InitializeAllToInfinity();

    // Using a priority queue to manage nodes to explore
    PriorityQueue<ChunkData, int> priorityQueue = new PriorityQueue<ChunkData, int>();

    // Enqueue StartNode, with distance 0
    distances[start] = 0;
    priorityQueue.Enqueue(start, 0);
    ChunkData current = null;

    while (priorityQueue.Count > 0)
    {
        priorityQueue.TryDequeue(out current, out _);

        if (!visited.Contains(current))
        {
            visited.Add(current);

            // Check if there is cached route from this node to the end
            if (cache.IsCached(current, goal)) return MergePathWithCache(goal, start, parentMap, current);

            // If last element in PQ reached
            if (current!=null && current.Equals(goal)) return ReconstructPath(parentMap, start, goal).ToList();

            foreach (var neighbor in GetAdjacentChunks(current))
            {
                if (!visited.Contains(neighbor) && neighbor.GetCharacterType() != typeof(Object) && (IsAllegianceSame(neighbor)||neighbor.GetCurrentPlayer()==null))
                {
                    // Calculate predicted distance to the end node (heuristic)
                    int predictedDistance = GetExpectedPathLength(neighbor, goal);

                    // Calculate distance to neighbor and total distance from start
                    int neighborDistance = Distance(current, neighbor);
                    
                    int totalDistance = distances[current] + neighborDistance;

                    // Check if total distance is smaller than the current known distance
                    if (distances.ContainsKey(neighbor) && totalDistance + predictedDistance < distances[neighbor])
                    {
                        // Update distance
                        distances[neighbor] = totalDistance + predictedDistance;
                        // Set parent
                        parentMap[neighbor] = current;
                        // Enqueue with new priority
                        priorityQueue.Enqueue(neighbor, totalDistance + predictedDistance);
                    }
                }
            }
        }
    }

    return null; // Path not found
	}

	private Dictionary<ChunkData, int> InitializeAllToInfinity()
	{
		var distances = new Dictionary<ChunkData, int>();
		foreach (var chunk in _chunkList)
		{
			distances[chunk] = int.MaxValue;
		}
		return distances;
}
	private int Distance(ChunkData a, ChunkData b)
	{
		var (ax, ay) = a.GetIndexes();
		var (bx, by) = b.GetIndexes();

		// Assuming each move costs '1', the distance between two adjacent nodes is always 1
		// Non-adjacent nodes are not directly reachable
		return Math.Abs(ax - bx) + Math.Abs(ay - by) == 1 ? 1 : Int32.MaxValue;
	}


	
	private List<ChunkData> MergePathWithCache(ChunkData goal, ChunkData startNode,
		Dictionary<ChunkData, ChunkData> parentMap, ChunkData current)
	{
		List<ChunkData> newRoute = ReconstructPath(parentMap, startNode, current);
		List<ChunkData> cachedSubRoute = cache.Get(current, goal);

		// Remove last element if newRoute is not empty to avoid duplication with the cachedSubRoute
		if (newRoute.Count > 0)
		{ 
			newRoute.RemoveAt(newRoute.Count-1);
		}
    
		// Combine with cached route
		foreach (var chunk in cachedSubRoute) 
		{
			newRoute.Add(chunk);
		}

		// Cache the whole route
		cache.Put(startNode, goal, newRoute);
    
		// Return result
		return newRoute;
	}

	private List<ChunkData> ReconstructPath(Dictionary<ChunkData, ChunkData> parentMap,
		ChunkData startNode, ChunkData current)
	{
		List<ChunkData> path = new List<ChunkData>();
		// Reconstruct the path from startNode to current by following the parentMap
		for (ChunkData node = current; !node.Equals(startNode); node = parentMap[node])
		{
			path.Insert(0,node);
		}
		path.Insert(0,startNode); // Add the start node at the beginning
		return path;
	}
	
	public class PathCache
	{
		// from, to -> list of tiles representing the path
		private Dictionary<ChunkData, Dictionary<ChunkData, List<ChunkData>>> pathCache = new Dictionary<ChunkData, Dictionary<ChunkData, List<ChunkData>>>();

		public void Put(ChunkData start, ChunkData end, List<ChunkData> path)
		{
			if (!pathCache.ContainsKey(start))
			{
				pathCache[start] = new Dictionary<ChunkData, List<ChunkData>>();
			}
			pathCache[start][end] = new List<ChunkData>(path); // Creates a copy of the path
		}

		public List<ChunkData> Get(ChunkData start, ChunkData end)
		{
			if (IsCached(start, end))
			{
				return new List<ChunkData>(pathCache[start][end]); // Return a copy to prevent external modification
			}
			return new List<ChunkData>();
		}

		public bool IsCached(ChunkData start, ChunkData end)
		{
			return pathCache.ContainsKey(start) && pathCache[start].ContainsKey(end);
		}
	}
}
