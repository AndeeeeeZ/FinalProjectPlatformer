using System.Text;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FishingInventory : ScriptableObject
{
    public VoidEvent OnFishingInventoryChange;
    public Dictionary<FishData, int> allFish;

    private void OnEnable()
    {
        // NOTE THIS WILL CAUSE THE INVENTORY TO RESET EACH TIME
        allFish = new Dictionary<FishData, int>();
    }
    public void AddFish(FishData fish)
    {
        if (allFish.ContainsKey(fish))
        {
            allFish[fish]++;
        }
        else
        {
            allFish[fish] = 1;
        }
        OnFishingInventoryChange.Raise();
    }

    public float GetInventoryValue()
    {
        float sum = 0f;
        foreach (KeyValuePair<FishData, int> fish in allFish)
        {
            sum += fish.Key.value * fish.Value;
        }
        return sum;
    }

    public string GetAllFishString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<FishData, int> fish in allFish)
        {
            stringBuilder.AppendLine(fish.Key.name + ": " + fish.Value);
        }
        return stringBuilder.ToString();
    }
}
