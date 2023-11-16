
using System.Collections.Generic;

[System.Serializable]
public class AbilityBlessings
{
    public List<UnlockedBlessings> UnlockedBlessingsList { get; set; }
    
    public AbilityBlessings(AbilityBlessings data)
    {
        UnlockedBlessingsList = new List<UnlockedBlessings>(data.UnlockedBlessingsList);
    }
    
    public AbilityBlessings(AbilityBlessingsResource data)
    {
        UnlockedBlessingsList = new List<UnlockedBlessings>();
        for (int i = 0; i < data.UnlockedBlessingsList.Count; i++)
        {
            UnlockedBlessingsList.Add(new UnlockedBlessings(data.UnlockedBlessingsList[i]));
        }
    }
}