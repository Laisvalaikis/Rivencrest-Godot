using System.Threading;
using Godot;

public partial class SaveManager: Node
{
	public void SaveData(int slotIndex)
	{
		SaveSystem.SaveCurrentSlot(slotIndex);
		if (SaveSystem.LoadStatistics(true) == null)
		{
			SaveSystem.SaveStatistics(new Statistics(), true);
		}

		Thread thread = new Thread(() =>
		{
			if (SaveSystem.LoadStatistics() == null)
			{
				SaveSystem.SaveStatistics(new Statistics());
			}
		});
		ThreadManager.InsertThreadAndWaitOthers(thread);
	}


	public void DeleteSlot(int slotIndex)
	{
		SaveSystem.DeleteSlot(slotIndex);
	}

	public void ClearGameData()
	{
		SaveSystem.ClearGameData();
	}

	public void CreateSave(Color color, int difficulty, string teamName)
	{
		// SaveSystem.SaveTownData(TownData.NewGameData(color, difficulty, teamName));
		SaveSystem.SaveTownDataThread(TownData.NewGameData(color.ToString(), difficulty, teamName));
	}
}

