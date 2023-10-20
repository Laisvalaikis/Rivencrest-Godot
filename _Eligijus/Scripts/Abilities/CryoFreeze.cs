using System.Collections.Generic;
using System.Threading;
using Godot;

public partial class CryoFreeze : BaseAction
{
	private PlayerInformation _playerInformation;
	private bool _isAbilityActive = false;

	public CryoFreeze()
	{
 		
	}
	public CryoFreeze(CryoFreeze cryoFreeze): base(cryoFreeze)
	{
		_isAbilityActive = cryoFreeze._isAbilityActive;
	}
 	
	public override BaseAction CreateNewInstance(BaseAction action)
	{
		CryoFreeze cryoFreeze = new CryoFreeze((CryoFreeze)action);
		return cryoFreeze;
	}
	public override void CreateAvailableChunkList(int attackRange)
	{
		_chunkList.Add(GameTileMap.Tilemap.GetChunk(player.GlobalPosition));
	}
	protected override void HighlightGridTile(ChunkData chunkData)
	{
			SetNonHoveredAttackColor(chunkData);
			chunkData.GetTileHighlight().EnableTile(true);
			chunkData.GetTileHighlight().ActivateColorGridTile(true);
	}
	
	public override void OnTurnStart()
	{
		if (_isAbilityActive && (player.playerInformation.GetHealth() > 0))
		{
			Thread thread = new Thread(() =>
			{
				Thread.Sleep(40);
				var pushDirectionVectors = new List<(int, int)>
				{
					(attackRange, 0),
					(0, attackRange),
					(-attackRange, 0),
					(0, -attackRange)
				};
				foreach (var x in pushDirectionVectors)
				{
					if (CheckIfSpecificInformationType(GetSpecificGroundTile(player.GlobalPosition), InformationType.Player)) //wrong
					{
						ChunkData chunkData = GameTileMap.Tilemap.GetChunk(player.GlobalPosition);
						if (IsAllegianceSame(chunkData))
						{
							DealRandomDamageToTarget(chunkData, minAttackDamage / 2, maxAttackDamage / 2);
						}
						else
						{
							DealRandomDamageToTarget(chunkData, minAttackDamage, maxAttackDamage);
						}
					}
				}
			});
			thread.Start();
			ThreadManager.InsertThread(thread);
		}
		_isAbilityActive = false;
	}

	public override bool CanTileBeClicked(ChunkData chunkData)
	{
		return true;
	}
	
	protected override void EnableDamagePreview(ChunkData chunk, string customText = null)
	{
		
	}

	public override void ResolveAbility(ChunkData chunk)
	{
		base.ResolveAbility(chunk);
		_isAbilityActive = true;
		_playerInformation.Stasis = true;
		FinishAbility();
	}
}
