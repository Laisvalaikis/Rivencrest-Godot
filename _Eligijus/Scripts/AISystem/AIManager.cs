using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace Rivencrestgodot._Eligijus.Scripts.AISystem;

//This class contains an AI team, calls AIEnemy functions and makes decisions
public partial class AIManager : Node
{
    public Team currentTeam;
    private List<(BaseAction, ChunkData, int)> possibleActions = new List<(BaseAction, ChunkData, int)>();
    private readonly List<ChunkData> _allChunks = new List<ChunkData>();
    private List<Player> AIPlayers = new List<Player>();

    public void Start()
    {
        _allChunks.Clear();
        ChunkData[,] array2D = GameTileMap.Tilemap.GetChunksArray();
        //Now for the fucky part TODO: fix later
        for (int i = 0; i < array2D.GetLength(0); i++)
        {
            for (int j = 0; j < array2D.GetLength(1); j++)
            {
                _allChunks.Add(array2D[i, j]);
            }
        }
    }
    
    public void OnTurnStart()
    {
        Start();
        GenerateAIPlayerList();
        GeneratePossibleActionsForTeamAbilities();
        bool actionsCanBeTaken = true;
        while (actionsCanBeTaken)
        {
            actionsCanBeTaken = false;
            while (possibleActions.Count > 0)
            {
                possibleActions.Sort((a, b) => b.Item3.CompareTo(a.Item3));
                possibleActions[0].Item1.ResolveAbility(possibleActions[0].Item2);
                possibleActions.Clear();
                GeneratePossibleActionsForTeamAbilities();
                actionsCanBeTaken = true;
            }

            foreach (var player in AIPlayers)
            {
                if (player.GetMovementPoints() > 0)
                {
                    MoveTowardsChunk(player, FindNearestPointOfInterest(player));
                    actionsCanBeTaken = true;
                }
            }
        }
    }

    //Generates list of players in the team that are AI. Currently, this is all players in team
    //But it could later be changed to where SOME players in a particular team ar AI
    public void GenerateAIPlayerList()
    {
        Godot.Collections.Dictionary<int, Player> characters = currentTeam.characters;

        foreach (var key in characters.Keys)
        {
            AIPlayers.Add(characters[key]);
        }
    }

    //Checks to see if a character has any abilities that can be used
    //If a character has at least one ability (excluding the first ability, movement)
    //that is unlocked and is not under cooldown, method returns true
    public bool PlayerHasAvailableAbilities(Player player)
    {
        Array<Ability> playerAbilities = player.actionManager.GetAllAbilities();
        for (int i = 1; i < playerAbilities.Count; i++)
        {
            if (playerAbilities[i].enabled && playerAbilities[i].Action.AbilityCanBeActivated())
            {
                return true;
            }
        }
        return false;
    }

    //Fills up possibleActions list with abilities that could
    //currently be used on the generated grid without moving the character.
    //List may include entries for same ability, but different tiles. We need this to choose possible tile options
    private void GeneratePossibleActionsForPlayerAbilities(Array<Ability> playerAbilities)
    {
        for (int i = 1; i < playerAbilities.Count; i++)
        {
            if (playerAbilities[i].enabled && playerAbilities[i].Action.AbilityCanBeActivated())
            {
                playerAbilities[i].Action.CreateAvailableChunkList(playerAbilities[i].Action.GetRange());
                foreach (var chunk in playerAbilities[i].Action.GetChunkList())
                {
                    if (playerAbilities[i].Action.CanBeUsedOnTile(chunk))
                    {
                        possibleActions.Add((playerAbilities[i].Action,chunk,0));
                    }
                }
            }
        }
    }

    //Generates possible actions for the whole team
    private void GeneratePossibleActionsForTeamAbilities()
    {
        possibleActions.Clear();
        foreach (var player in AIPlayers)
        {
            GeneratePossibleActionsForPlayerAbilities(player.actionManager.GetAllAbilities());
        }
    }

    //This method iterates through every possible action and calculates the weight.
    //Current implementation is very simple and almost static. This method can be made as complicated
    //As we want it to be. Needs to be constantly tweaked for balancing 
    public void GenerateWeights()
    {
        for (int i = 0; i < possibleActions.Count; i++)
        {
            var action = possibleActions[i];
            int weight = 0;

            if (!action.Item1.isAbilitySlow)
            {
                weight += 20;
            }
            if (action.Item1.IsAllegianceSame(action.Item2))
            {
                weight += 60;
            }
            else
            {
                weight += 40;
            }
            /*if (action.Item1.minAttackDamage + action.Item1.bonusDamage >= action.Item2.GetCurrentObject()
                    .objectInformation.GetObjectInformation().GetHealth()) //might be busted and shitty TODO: test
            {
                weight += 100;
            }

            weight -= action.Item2.GetCurrentObject().objectInformation.GetObjectInformation().GetHealth();*/
            possibleActions[i] = (action.Item1, action.Item2, weight);
        }
    }

    //Uses A* to find a path from character to the desired chunk.
    //Character then moves chunk by chunk towards the desired chunk
    //It is likely that the character will not complete his journey
    //In this case, the A* will be recalculated next time he moves
    //This is done, because the map might have changed since last time
    public void MoveTowardsChunk(Player player, ChunkData chunk)
    {
        List<ChunkData> path = AStarSearch(GameTileMap.Tilemap.GetChunk(player.GlobalPosition), chunk);
        int tilesWalked = 0;
        //Its probably possible for a character to die mid-move(by stepping on a trap)
        //So we probably have to check if player is still alive after each MoveSelectedCharacter
        //TODO: that
        while (player.GetMovementPoints() > 0 && path.Count > tilesWalked)
        {
            GameTileMap.Tilemap.MoveSelectedCharacter(path[tilesWalked],player);
            tilesWalked++;
            player.AddMovementPoints(-1);
        }
    }

    //Returns chunk that is considered a point of interest for the character
    //This, currently, is just a character of a different allegiance. It could
    //Later be a flag / item / object (for example we spawn berries and this distracts the bear)
    public ChunkData  FindNearestPointOfInterest(Player player)
    {
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        Queue<(ChunkData chunk, int distance)> queue = new Queue<(ChunkData, int)>();
        HashSet<ChunkData> visited = new HashSet<ChunkData>();

        queue.Enqueue((chunk, 0));
        visited.Add(chunk);

        while (queue.Count > 0)
        {
            (ChunkData currentChunk, int currentDistance) = queue.Dequeue();

            // Check if the current chunk is a point of interest
            if (currentChunk.GetCurrentPlayer() != null && !IsAllegianceSame(player, currentChunk))
            {
                return FindNearestAdjacentChunk(player, currentChunk); // Found the nearest point of interest
            }

            foreach (ChunkData adjacentChunk in GetAdjacentChunks(currentChunk))
            {
                if (!visited.Contains(adjacentChunk))
                {
                    queue.Enqueue((adjacentChunk, currentDistance + 1));
                    visited.Add(adjacentChunk);
                }
            }
        }
        return null; // No point of interest found (this should not happen)
    }

    //Finds an adjacent chunk for a given chunk that is 
    //1)Steppable on
    //2)Is the closest to our AI character
    private ChunkData FindNearestAdjacentChunk(Player player, ChunkData chunk)
    {
        List<ChunkData> adjacentChunks = GameTileMap.Tilemap.GetAllChunksAround(chunk);
        int minDistance = int.MaxValue;
        ChunkData resultChunk = null;
        foreach (var chunkData in adjacentChunks)
        {
            if (chunkData.GetCurrentPlayer() == null)
            {
                var (ax, ay) = GameTileMap.Tilemap.GetChunk(player.GlobalPosition).GetIndexes();
                var (bx, by) = chunkData.GetIndexes();
                int calculatedDistance = Math.Abs(ax - bx) + Math.Abs(ay - by);
                if (calculatedDistance < minDistance)
                {
                    minDistance = calculatedDistance;
                    resultChunk = chunkData;
                }
            }
        }
        return resultChunk;
    }
    
    
    
    //--------------------------------------------Methods for pathfinding--------------------------------------------
    
    //AStarSearch method taken from PlayerMovement, but without caching because we dont need to cache here
    public List<ChunkData> AStarSearch(ChunkData start, ChunkData goal)
    {
        System.Collections.Generic.Dictionary<ChunkData, ChunkData> parentMap = new System.Collections.Generic.Dictionary<ChunkData, ChunkData>();
        HashSet<ChunkData> visited = new HashSet<ChunkData>();
        System.Collections.Generic.Dictionary<ChunkData, int> distances = InitializeAllToInfinity();

        PriorityQueue<ChunkData, int> priorityQueue = new PriorityQueue<ChunkData, int>();

        distances[start] = 0;
        priorityQueue.Enqueue(start, 0);
        ChunkData current = null;

        while (priorityQueue.Count > 0)
        {
            priorityQueue.TryDequeue(out current, out _);

            if (!visited.Contains(current))
            {
                visited.Add(current);
                // If last element in PQ reached
                if (current != null && current.Equals(goal))
                {
                    return ReconstructPath(parentMap, start, goal);
                }

                foreach (var neighbor in GetAdjacentChunks(current))
                {
                    if (!visited.Contains(neighbor) && 
                        (neighbor.GetCharacterType() != typeof(Object) || neighbor.GetCurrentObject().CanStepOn()) && 
                        (neighbor.GetCurrentPlayer() == null))
                    {
                        int predictedDistance = GetExpectedPathLength(neighbor, goal);
                        int neighborDistance = Distance(current, neighbor);
                        int totalDistance = distances[current] + neighborDistance;

                        if (distances.ContainsKey(neighbor) && totalDistance + predictedDistance < distances[neighbor])
                        {
                            distances[neighbor] = totalDistance + predictedDistance;
                            parentMap[neighbor] = current;
                            priorityQueue.Enqueue(neighbor, totalDistance + predictedDistance);
                        }
                    }
                }
            }
        }
        return new List<ChunkData>(); // Path not found
    }

    //We use IsAllegianceSame in PlayerMovement, so we need it here as well. 
    private bool IsAllegianceSame(Player player, ChunkData chunk)
    {
        return chunk!=null && chunk.CharacterIsOnTile() && player != null && chunk.GetCurrentPlayer().GetPlayerTeam() == player.GetPlayerTeam();
    }

    //We need both Godot.Collections and System.Collections.Generic so uhhhhhhhhhhhhhhhh
    private System.Collections.Generic.Dictionary<ChunkData, int> InitializeAllToInfinity()
    {
        var distances = new System.Collections.Generic.Dictionary<ChunkData, int>();
        foreach (var chunk in _allChunks)
        {
            distances[chunk] = int.MaxValue;
        }
        return distances;
    }
    private int GetExpectedPathLength(ChunkData start, ChunkData end)
    {
        var (startX, startY) = start.GetIndexes();
        var (endX, endY) = end.GetIndexes();
        return Math.Abs(endX - startX) + Math.Abs(endY - startY);
    }
    
    private IEnumerable<ChunkData> GetAdjacentChunks(ChunkData chunk)
    {
        ChunkData[,] chunksArray = GameTileMap.Tilemap.GetChunksArray();
        (int chunkX, int chunkY) = chunk.GetIndexes();

        if (GameTileMap.Tilemap.CheckBounds(chunkX, chunkY - 1))
        {
            yield return chunksArray[chunkX, chunkY - 1];
        }
        if (GameTileMap.Tilemap.CheckBounds(chunkX, chunkY + 1))
        {
            yield return chunksArray[chunkX, chunkY + 1];
        }
        if (GameTileMap.Tilemap.CheckBounds(chunkX - 1, chunkY))
        {
            yield return chunksArray[chunkX - 1, chunkY];
        }
        if (GameTileMap.Tilemap.CheckBounds(chunkX + 1, chunkY))
        {
            yield return chunksArray[chunkX + 1, chunkY];
        }
    }

    private int Distance(ChunkData a, ChunkData b)
    {
        var (ax, ay) = a.GetIndexes();
        var (bx, by) = b.GetIndexes();
        return Math.Abs(ax - bx) + Math.Abs(ay - by) == 1 ? 1 : Int32.MaxValue;
    }

    private List<ChunkData> ReconstructPath(System.Collections.Generic.Dictionary<ChunkData, ChunkData> parentMap, ChunkData startNode, ChunkData current)
    {
        List<ChunkData> path = new List<ChunkData>();
        // Reconstruct the path from startNode to current by following the parentMap
        for (ChunkData node = current; !node.Equals(startNode); node = parentMap[node])
        {
            path.Insert(0, node);
        }
        path.Insert(0, startNode); // Add the start node at the beginning
        return path;
    }
    //--------------------------------------------------------------------------------------------------------------
}