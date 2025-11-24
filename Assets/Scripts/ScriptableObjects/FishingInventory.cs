using System.Text;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FishingInventory : ScriptableObject
{
    public Dictionary<FishData, int> allFish;

    public float GetInventoryValue()
    {
        float sum = 0f;
        foreach (KeyValuePair<FishData, int> fish in allFish)
        {
            sum += fish.Key.value * fish.Value;
        }
        return sum;
    }

    public string GetAllFish()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (KeyValuePair<FishData, int> fish in allFish)
        {
            stringBuilder.AppendLine(fish.Key.name + ": " + fish.Value);
        }
        return stringBuilder.ToString();
    }
}
