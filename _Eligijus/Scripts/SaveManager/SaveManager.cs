using System.Threading;
using Godot;

public partial class SaveManager: Node
{
	private Thread _thread;

	public void SaveData(int slotIndex)
	{
		SaveSystem.SaveCurrentSlot(slotIndex);
		if (SaveSystem.LoadStatistics(true) == null)
		{
			SaveSystem.SaveStatistics(new Statistics(), true);
		}
		if (SaveSystem.LoadStatistics() == null)
		{
			Thread thread = new Thread(() => { SaveSystem.SaveStatistics(new Statistics()); });
			ThreadManager.InsertThreadAndWaitOthers(thread);
		}
	}

	public void DeleteSlot(int slotIndex)
	{
		SaveSystem.DeleteSlot(slotIndex);
	}

	public void ClearGameData()
	{
		SaveSystem.ClearGameData();
	}

	public void CreateSave(string color, int difficulty, string teamName)
	{
		// SaveSystem.SaveTownData(TownData.NewGameData(color, difficulty, teamName));
		SaveSystem.SaveTownDataThread(TownData.NewGameData(color, difficulty, teamName));
	}
}

