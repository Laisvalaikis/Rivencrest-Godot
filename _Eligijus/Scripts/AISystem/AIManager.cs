using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using Godot.Collections;

namespace Rivencrestgodot._Eligijus.Scripts.AISystem;

//This class contains an AI team. It takes ownership of and makes decisions for that team
public partial class AIManager : Node
{
    public Team currentTeam;
    private List<(BaseAction, ChunkData, int)> possibleActions = new List<(BaseAction, ChunkData, int)>();
    private (BaseAction Action, ChunkData chunk, int weight) highestWeightAction;
    private List<Player> AIPlayers = new List<Player>();
    private int difficulty;
    
    public void OnTurnStart()
    {
        difficulty = Data.Instance.townData.difficultyLevel;
        if (difficulty == 1)
        {
            //idėja easy/hard mode'ui
            HealEveryAIPlayer();
        }
        GenerateAiPlayerList();
        PerformActions2();
    }

    //Tries to resolve abilities and move players in direction of their points of interest.
    private async void PerformActions2()
    {
        bool actionsPerformed = true;

        while (actionsPerformed)
        {
            actionsPerformed = false;
            if (await ResolveAbilities())
            {
                actionsPerformed = true;
                await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
            }
            if (await MoveAIPlayers())
            {
                actionsPerformed = true;
            }
        }
    }

    
    //Generates actions and their weights. If an action can be performed, performs the action with highest
    //weight and returns true. Otherwise returns false.
    private async Task<bool> ResolveAbilities()
    {
        GeneratePossibleActionsAndWeightsForTeamAbilities();
        if (highestWeightAction.Action == null)
        {
            return false;
        }
        bool actionTaken = false;

        while (highestWeightAction.Action != null)
        {
            PerformHighestWeightAction();
            highestWeightAction = new(null, null, int.MinValue);
            GeneratePossibleActionsAndWeightsForTeamAbilities();
            actionTaken = true;
            await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        }
        return actionTaken;
    }

    
    //Moves each AI player towards the nearest point of interest. If at least one player was moved,
    //returns true. Otherwise returns false
    private async Task<bool> MoveAIPlayers()
    {
        bool movementMade = false;

        foreach (var player in AIPlayers)
        {
            if (player.GetMovementPoints() > 0)
            {
                ChunkData chunk = FindNearestPointOfInterest(player);
                if (chunk != null && chunk != GameTileMap.Tilemap.GetChunk(player.GlobalPosition))
                {
                    bool moved = await MoveTowardsChunk(player, chunk);
                    if (moved)
                    {
                        movementMade = true;
                    }
                }
            }
        }
        return movementMade;
    }


    
    //Performs actions. see PerformActions2
    private async Task PerformActions()
    {
        bool actionsCanBeTaken = true;
        while (actionsCanBeTaken)
        {
            actionsCanBeTaken = false;
            while (possibleActions.Count > 0)
            {
                possibleActions.Sort((a, b) => b.Item3.CompareTo(a.Item3));
                possibleActions[0].Item1.ResolveAbility(possibleActions[0].Item2);
                GeneratePossibleActionsForTeamAbilities();
                actionsCanBeTaken = true;
            }
    
            foreach (var player in AIPlayers)
            {
                if (player.GetMovementPoints() > 0)
                {
                    ChunkData chunk = FindNearestPointOfInterest(player);
                    if (chunk != null && chunk != GameTileMap.Tilemap.GetChunk(player.GlobalPosition))
                    {
                        bool hasMoved = await MoveTowardsChunk(player, chunk);
                        if (hasMoved)
                        {
                            actionsCanBeTaken = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    //Generates list of players in the team that are AI. Currently, this is all players in team
    //But it could later be changed to where SOME players in a particular team ar AI
    private void GenerateAiPlayerList()
    {
        Godot.Collections.Dictionary<int, Player> characters = currentTeam.characters;
        AIPlayers.Clear();
        foreach (var key in characters.Keys)
        {
            AIPlayers.Add(characters[key]);
        }
    }

    //Checks to see if a character has any abilities that can be used
    //If a character has at least one ability (excluding the first ability, movement)
    //that is unlocked and is not under cooldown, method returns true
    private bool PlayerHasAvailableAbilities(Player player)
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
    
    
    //Generates possible actions for the whole team
    private void GeneratePossibleActionsForTeamAbilities()
    {
        possibleActions.Clear();
        foreach (var player in AIPlayers)
        {
            GeneratePossibleActionsForPlayerAbilities(player.actionManager.GetAllAbilities());
        }
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


    // Generates possible actions and weights for the whole team, keeping track of the highest-weight action
    // This is less flexible, but offers better performance
    private void GeneratePossibleActionsAndWeightsForTeamAbilities()
    {
        possibleActions.Clear();
        int highestWeight = int.MinValue;

        foreach (var player in AIPlayers)
        {
            var playerAbilities = player.actionManager.GetAllAbilities();
            foreach (var ability in playerAbilities)
            {
                if (ability.enabled && ability.Action.AbilityCanBeActivated())
                {
                    ability.Action.CreateAvailableChunkList(ability.Action.GetRange());
                    foreach (var chunk in ability.Action.GetChunkList())
                    {
                        if (ability.Action.CanBeUsedOnTile(chunk))
                        {
                            int weight = CalculateWeightForAction(ability.Action, chunk);
                            if (weight > highestWeight)
                            {
                                highestWeight = weight;
                                highestWeightAction=(ability.Action, chunk, weight);
                            }
                        }
                    }
                }
            }
        }
    }
    
    
    //This method iterates through every possible action and calculates the weight.
    private void GenerateWeights()
    {
        for (int i = 0; i < possibleActions.Count; i++)
        {
            var action = possibleActions[i];
            int weight = CalculateWeightForAction(action.Item1, action.Item2);
            possibleActions[i] = (action.Item1, action.Item2, weight);
        }
    }


    //Calculates the weight of a specific ability that can be used on a specific tile and returns it.
    //Current implementation is very simple and almost static. This method can be made as complicated
    //As we want it to be. Needs to be constantly tweaked for balancing 
    private int CalculateWeightForAction(BaseAction action, ChunkData chunk)
    {
        int weight = 0;

        if (!action.isAbilitySlow)
        {
            weight += 20;
        }
        if (action.IsAllegianceSame(chunk))
        {
            weight += 60;
        }
        else
        {
            weight += 40;
        }
        if (action.minAttackDamage + action.bonusDamage >= chunk.GetCurrentObject()?
                .objectInformation.GetObjectInformation().GetHealth())
        {
            if (difficulty == 1)
            {
                weight += 100;
            }
        }
        if (chunk.GetCurrentPlayer() != null)
        {
            if (difficulty == 1)
            {
                weight -= chunk.GetCurrentPlayer().objectInformation.GetObjectInformation().GetHealth();
            }
            else
            {
                weight += chunk.GetCurrentPlayer().objectInformation.GetObjectInformation().GetHealth();
            }
        }

        return weight;
    }

    //Performs the action that was found to have the highest weight
    private void PerformHighestWeightAction()
    {
        if (highestWeightAction.Action != null)
        {
            var (action, chunk, _) = highestWeightAction;
            action.ResolveAbility(chunk);
        }
    }

    //Uses A* to find a path from character to the desired chunk.
    //Character then moves chunk by chunk towards the desired chunk
    //It is likely that the character will not complete his journey
    //In this case, the A* will be recalculated next time he moves
    //This is done, because the map might have changed since last time
    private async Task<bool> MoveTowardsChunk(Player player, ChunkData chunk)
    {
        bool hasMoved = false;
        List<ChunkData> path = AStarSearch(GameTileMap.Tilemap.GetChunk(player.GlobalPosition), chunk);
        int tilesWalked = 0;

        while (player.GetMovementPoints() > 0 && path.Count > tilesWalked)
        {
            GameTileMap.Tilemap.MoveSelectedCharacter(path[tilesWalked], player);
            tilesWalked++;
            player.AddMovementPoints(-1);
            hasMoved = true;
            await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
        }

        return hasMoved;
    }


    //Returns chunk that is considered a point of interest for the character
    //This, currently, is just a character of a different allegiance. It could
    //Later be a flag / item / object (for example we spawn berries and this distracts the bear)
    private ChunkData  FindNearestPointOfInterest(Player player)
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
                ChunkData chunkData = FindNearestAdjacentChunk(player, currentChunk);
                if(chunkData!=null)
                {
                    return chunkData; // Found the nearest point of interest
                }
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
        return null; // No point of interest found
    }
    
    //Finds all points of interest for a player within specified and returns them as a list. 
    //If no radius is provided, returns all points of interest, ordered by distance.
    private List<ChunkData> FindPointsOfInterestWithinRadius(Player player, int? radius = null)
    {
        ChunkData chunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        Queue<(ChunkData chunk, int distance)> queue = new Queue<(ChunkData, int)>();
        HashSet<ChunkData> visited = new HashSet<ChunkData>();
        List<ChunkData> pointsOfInterest = new List<ChunkData>();

        queue.Enqueue((chunk, 0));
        visited.Add(chunk);

        while (queue.Count > 0)
        {
            (ChunkData currentChunk, int currentDistance) = queue.Dequeue();

            if ((radius == null || currentDistance <= radius) && currentChunk.GetCurrentPlayer() != null && !IsAllegianceSame(player, currentChunk))
            {
                pointsOfInterest.Add(currentChunk);
            }
            if (radius == null || currentDistance < radius)
            {
                foreach (ChunkData adjacentChunk in GetAdjacentChunks(currentChunk))
                {
                    if (!visited.Contains(adjacentChunk))
                    {
                        queue.Enqueue((adjacentChunk, currentDistance + 1));
                        visited.Add(adjacentChunk);
                    }
                }
            }
        }
        return pointsOfInterest;
    }



    //Finds an adjacent chunk for a given chunk that is 
    //1)Steppable on
    //2)Is the closest to our AI character
    private ChunkData FindNearestAdjacentChunk(Player player, ChunkData chunk)
    {
        List<ChunkData> adjacentChunks = GameTileMap.Tilemap.GetAllChunksAround(chunk);
        ChunkData resultChunk = null;
        ChunkData playerChunk = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
        var (ax, ay) = playerChunk.GetIndexes();
        var (bx, by) = chunk.GetIndexes();
        int minDistance = Math.Abs(ax - bx) + Math.Abs(ay - by);
        if (minDistance == 1)
            return playerChunk;
        foreach (var chunkData in adjacentChunks)
        {
            if (CanStepOnTile(chunkData))
            {
                (bx, by) = chunkData.GetIndexes();
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

    private void HealEveryAIPlayer()
    {
        foreach (var player in AIPlayers)
        {
            player.objectInformation.GetPlayerInformation().Heal(1);
        }
    }
    
    
    //--------------------------------------------Methods for pathfinding--------------------------------------------
    
    //AStarSearch method taken from PlayerMovement, but without caching because we dont need to cache here
    private List<ChunkData> AStarSearch(ChunkData start, ChunkData goal)
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
                if (current != null && current.Equals(goal))
                {
                    return ReconstructPath(parentMap, start, goal);
                }

                foreach (var neighbor in GetAdjacentChunks(current))
                {
                    if (!visited.Contains(neighbor) && CanStepOnTile(neighbor))
                    {
                        int predictedDistance = GetExpectedPathLength(neighbor, goal);
                        int neighborDistance = Distance(current, neighbor);
                        // ReSharper disable once AssignNullToNotNullAttribute
                        // This is fine
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

    private bool CanStepOnTile(ChunkData chunk)
    {
        return (chunk.GetCurrentPlayer()==null && chunk.GetCurrentObject() == null || (chunk.GetCurrentObject() != null && chunk.GetCurrentObject().CanStepOn() && 
               chunk.GetCurrentPlayer() == null));
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
        ChunkData[,] array2D = GameTileMap.Tilemap.GetChunksArray();
        for (int i = 0; i < array2D.GetLength(0); i++)
        {
            for (int j = 0; j < array2D.GetLength(1); j++)
            {
                distances[array2D[i, j]] = int.MaxValue;
            }
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