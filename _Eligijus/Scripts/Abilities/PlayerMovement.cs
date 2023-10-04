using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerMovement : BaseAction
{
	private bool isFacingRight = true;
	private GameTileMap _gameTileMap;

	private List<ChunkData> _path;
	private ChunkData[,] _chunkArray;

	public override Variant _Get(StringName property)
	{
		return base._Get(property);
	}

	public override void Start()
	{
		base.Start();
		// AttackHighlight = new Color32(130, 255, 95, 255);
		// AttackHighlightHover = new Color32(255, 227, 0, 255);
		// AttackAbility = true;
		_gameTileMap = GameTileMap.Tilemap;
		_chunkArray = _gameTileMap.GetChunksArray();
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
			if (_path != null && _path.Any() && IsAdjacent(hoveredChunk, _path[^1]))
			{
				UpdatePath(hoveredChunk, _path, _chunkArray);
			}
			else
			{
				_path = GetDiagonalPath(_gameTileMap.GetChunk(player.GlobalPosition), hoveredChunk, _chunkArray);
			}
			SetTileArrow(_path,0,_path.Count-1);
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
	
	protected override void HighlightGridTile(ChunkData chunkData)
	{
		if (chunkData.GetCurrentCharacter() == null)
		{
			chunkData.GetTileHighlight().ActivateColorGridTile(true);
			chunkData.GetTileHighlight().SetHighlightColor(AbilityHighlight);
		}
	}
	
	public override void ResolveAbility(ChunkData chunk)
	{
		ClearArrowPath();
		_path = null;
		base.ResolveAbility(chunk);
		if (!GameTileMap.Tilemap.CharacterIsOnTile(chunk))
		{
			GameTileMap.Tilemap.MoveSelectedCharacter(chunk);
		}
		FinishAbility();
		CreateGrid();
	}
	
	protected override void FinishAbility()
	{
		//Remove movement points?
	}
	
	private List<ChunkData> GetDiagonalPath(ChunkData start, ChunkData end, ChunkData[,] chunkArray)
	{
		List<ChunkData> stairStepPath = new List<ChunkData>();
		// Get the starting and ending indexes
		int startX = start.GetIndexes().Item2;
		int startY = start.GetIndexes().Item1;
		int endX = end.GetIndexes().Item2;
		int endY = end.GetIndexes().Item1;
	
		int x = startX, y = startY;
	
		// Determine the direction of the moves
		int xStep = (endX > startX) ? 1 : -1;
		int yStep = (endY > startY) ? 1 : -1;
	
		while (x != endX || y != endY)
		{
			// Checking the corners for out-of-bounds and deciding the path accordingly
			if (x != endX)
			{
				stairStepPath.Add(chunkArray[y, x]);
				if (!chunkArray[y, x + xStep].GetTileHighlight().isHighlighted && chunkArray[y,x+xStep].GetCurrentCharacter()==null)
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
				stairStepPath.Add(chunkArray[y, x]);
				if (!chunkArray[y + yStep, x].GetTileHighlight().isHighlighted && chunkArray[y+yStep,x].GetCurrentCharacter()==null)
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
		stairStepPath.Add(chunkArray[endY, endX]);
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
		for (int i = start; i <= end; i++)
		{
			ChunkData current = path[i];
			ChunkData prev = i > 0 ? path[i - 1] : null;
			ChunkData next = i < path.Count - 1 ? path[i + 1] : null;
	
			int arrowType = DetermineArrowType(current, prev, next);
			path[i].GetTileHighlight().SetArrowSprite(arrowType);
		}
	}
	
	private int DetermineArrowType(ChunkData current, ChunkData prev, ChunkData next)
	{
		if (prev == null && next == null) return 0;  // Invalid case

		var (cy, cx) = current.GetIndexes();
		var (py, px) = prev?.GetIndexes() ?? (0, 0);
		var (ny, nx) = next?.GetIndexes() ?? (0, 0);
        
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
	
}
